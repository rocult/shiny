using MoonsecDeobfuscator.Ast;
using MoonsecDeobfuscator.Bytecode.Models;
using MoonsecDeobfuscator.Deobfuscation.Rewriters;
using MoonsecDeobfuscator.Deobfuscation.Utils;
using MoonsecDeobfuscator.Deobfuscation.Walkers;
using Function = MoonsecDeobfuscator.Bytecode.Models.Function;

namespace MoonsecDeobfuscator.Deobfuscation;

public class Handler(Block body, string fingerprint)
{
    public readonly Block Body = body;
    public readonly string Fingerprint = fingerprint;
}

public class BytecodeDeobfuscator(Function function, Context ctx)
{
    private readonly Dictionary<int, List<Handler>> _handlerMapping = [];
    private readonly Function _rootFunction = function;

    public Function Deobfuscate()
    {
        CreateHandlerMapping();
        DeobfuscateOpcodes(_rootFunction);
        DeobfuscateControlFlow(_rootFunction);
        FixProgramEntry();
        RebuildConstantPool(_rootFunction);
        SetFlags(_rootFunction);
        _rootFunction.IsVarArgFlag = 2;
        return _rootFunction;
    }

    private static void DeobfuscateControlFlow(Function function)
    {
        var jumpReferences = ComputeJumpReferences(function);

        RemoveDeadCode(function);
        RemoveTestFlip(function);
        FixTailCall(function);
        FixJumpOffsets(function, jumpReferences);

        foreach (var childFunction in function.Functions)
            DeobfuscateControlFlow(childFunction);
    }

    private static void RebuildConstantPool(Function function)
    {
        var remapping = new Dictionary<int, int>(function.Constants.Count);
        var newOrder = new List<Constant>(function.Constants.Count);
        var next = 0;

        foreach (var instruction in function.Instructions)
        {
            switch (instruction.OpCode)
            {
                case OpCode.LoadK:
                case OpCode.GetGlobal:
                case OpCode.SetGlobal:
                    instruction.B = Remap(instruction.B);
                    break;
                case OpCode.SetTable:
                case OpCode.Eq:
                case OpCode.Lt:
                case OpCode.Le:
                case OpCode.Add:
                case OpCode.Sub:
                case OpCode.Mul:
                case OpCode.Div:
                case OpCode.Mod:
                case OpCode.Pow:
                    instruction.B = RemapRK(instruction.B);
                    instruction.C = RemapRK(instruction.C);
                    break;
                case OpCode.GetTable:
                case OpCode.Self:
                    instruction.C = RemapRK(instruction.C);
                    break;
            }
        }

        function.Constants = newOrder;
        function.Functions.ForEach(RebuildConstantPool);
        return;

        int Remap(int oldIdx)
        {
            if (!remapping.TryGetValue(oldIdx, out var newIdx))
            {
                newIdx = next++;
                remapping[oldIdx] = newIdx;
                newOrder.Add(function.Constants[oldIdx]);
            }

            return newIdx;
        }

        int RemapRK(int operand) => operand >= 256 ? Remap(operand - 256) + 256 : operand;
    }

    private static void RemoveTestFlip(Function function)
    {
        var instructions = function.Instructions;

        for (var i = 0; i < instructions.Count; i++)
        {
            var instruction = instructions[i];

            if (instruction.OpCode is not (OpCode.Eq or OpCode.Lt or OpCode.Le or OpCode.Test))
                continue;

            var next1 = instructions[i + 1];
            var next2 = instructions[i + 2];

            if (!(next1.OpCode == OpCode.Jmp && next2.OpCode == OpCode.Jmp))
                continue;

            switch (instruction.OpCode)
            {
                case OpCode.Eq:
                case OpCode.Lt:
                case OpCode.Le:
                    instruction.A = instruction.A == 1 ? 0 : 1;
                    break;
                case OpCode.Test:
                    instruction.C = instruction.C == 1 ? 0 : 1;
                    break;
            }

            instructions.RemoveAt(i + 1);
        }
    }

    /*
        Insert Return after TailCall if not present
    */
    private static void FixTailCall(Function function)
    {
        var instructions = function.Instructions;

        for (var i = 0; i < instructions.Count; i++)
        {
            var instruction = instructions[i];

            if (instruction.OpCode != OpCode.TailCall)
                continue;

            if (i + 1 < instructions.Count && instructions[i + 1].OpCode == OpCode.Return)
                continue;

            instructions.Insert(i + 1, new Instruction
            {
                OpCode = OpCode.Return,
                A = instruction.A
            });
        }
    }

    /*
        The actual program starts after this check and anything before is to be removed.

        if GLOBAL ~= "This file was protected with MoonSec V3" then
            return
        end
    */
    private void FixProgramEntry()
    {
        var check = _rootFunction.Instructions
            .FirstOrDefault(instr => instr is { OpCode: OpCode.Eq, C: > 255 }
                                     && instr.Function.Constants[instr.C - 256] is StringConstant sc
                                     && sc.Value.StartsWith("This file was protected with MoonSec V3"));

        if (check == null)
            return;

        var idx = _rootFunction.Instructions.IndexOf(check);

        for (var i = idx; i < _rootFunction.Instructions.Count; i++)
        {
            var instruction = _rootFunction.Instructions[i];

            if (instruction.OpCode == OpCode.Return)
            {
                _rootFunction.Instructions.RemoveRange(0, i + 1);
                break;
            }
        }

        RemoveUnusedFunctions();
    }

    /*
        FixProgramEntry might leave some unused functions so we remove them here
    */
    private void RemoveUnusedFunctions()
    {
        var functionReferences = new Dictionary<Instruction, Function>();

        foreach (var instruction in _rootFunction.Instructions)
        {
            if (instruction.OpCode == OpCode.Closure)
                functionReferences[instruction] = _rootFunction.Functions[instruction.B];
        }

        _rootFunction.Functions.RemoveAll(f => !functionReferences.ContainsValue(f));

        foreach (var (instr, function) in functionReferences)
            instr.B = _rootFunction.Functions.IndexOf(function);
    }

    private static void RemoveDeadCode(Function function)
    {
        function.Instructions.RemoveAll(instruction => instruction.IsDead);
    }

    private static void FixJumpOffsets(Function function, Dictionary<Instruction, Instruction> jumpReferences)
    {
        var instructions = function.Instructions;
        var pos = new Dictionary<Instruction, int>();

        for (var i = 0; i < instructions.Count; i++)
            pos[instructions[i]] = i;

        for (var i = 0; i < instructions.Count; i++)
        {
            var instruction = instructions[i];

            if (jumpReferences.TryGetValue(instruction, out var target))
                instruction.B = pos[target] - (i + 1);
        }
    }

    private static void SetFlags(Function function)
    {
        function.IsVarArgFlag = 0;

        var maxA = 0;

        foreach (var instruction in function.Instructions)
        {
            var opcode = instruction.OpCode;
            var A = instruction.A;

            if (A > maxA)
                maxA = A;

            if (opcode == OpCode.Closure)
                function.Functions[instruction.B].NumUpvalues = (byte) instruction.C;
            else if (opcode == OpCode.VarArg)
                function.IsVarArgFlag = 2;
        }

        function.MaxStackSize = (byte) (maxA + 1);
        function.Functions.ForEach(SetFlags);
    }

    private static Dictionary<Instruction, Instruction> ComputeJumpReferences(Function function)
    {
        var jumpReferences = new Dictionary<Instruction, Instruction>();
        var instructions = function.Instructions;

        for (var i = 0; i < instructions.Count; i++)
        {
            var instruction = instructions[i];

            if (instruction.OpCode is OpCode.Jmp or OpCode.ForLoop or OpCode.ForPrep)
                jumpReferences[instruction] = instructions[(i + instruction.B) + 1];
        }

        return jumpReferences;
    }

    private void DeobfuscateOpcodes(Function function)
    {
        var instructions = function.Instructions;
        var jmpSet = new HashSet<int>();

        for (var i = 0; i < instructions.Count; i++)
        {
            var instruction = instructions[i];
            var handlers = _handlerMapping[instruction.OpNum];

            foreach (var handler in handlers)
            {
                DeobfuscateOpcode(instruction, handler);

                if (instruction.OpCode == OpCode.Jmp)
                    jmpSet.Add(i + instruction.B + 1);

                if (instruction.OpCode is OpCode.Return or OpCode.TailCall)
                {
                    SkipDeadCode(instructions, jmpSet, ref i);
                    break;
                }

                if (handler != handlers.Last() && i + 1 < instructions.Count)
                    instruction = instructions[++i];
            }
        }

        function.Functions.ForEach(DeobfuscateOpcodes);
    }

    private static void SkipDeadCode(List<Instruction> instructions, HashSet<int> jmpSet, ref int i)
    {
        while (++i < instructions.Count)
        {
            var nextIndex = i;

            if (jmpSet.Contains(nextIndex))
            {
                i--;
                break;
            }

            instructions[i].IsDead = true;
        }
    }

    private static void DeobfuscateOpcode(Instruction instruction, Handler handler)
    {
        if (OpCodes.StaticOpcodes.TryGetValue(handler.Fingerprint, out var it))
        {
            instruction.OpCode = it.Item1;
            it.Item2?.Invoke(instruction);
        }
        else
        {
            Console.Error.WriteLine($"Could not identify handler: {handler.Fingerprint}");
            Console.Error.WriteLine(PrettyPrinter.AsString(handler.Body));
        }
    }

    private void CreateHandlerMapping()
    {
        var solvedVmTree = new TreeSolver(ctx.VmTree, "enum").Solve();

        foreach (var entry in solvedVmTree)
            CreateHandler(entry.Key, entry.Value);
    }

    private void CreateHandler(int opcode, Block body)
    {
        var handlers = body.Statements
            .WindowedByDiscardedPairs((a, b) => Matching.IsPcIncrement(a) && Matching.IsInstAssign(b))
            .Select(stats =>
            {
                var block = new Block
                {
                    Statements = [..stats]
                }.Clone();
                new HandlerRewriter().Rewrite(block, Order.PostOrder, symbols: true, fixedPoint: true);
                return block;
            })
            .Select(block => new Handler(block, FingerprintGenerator.Generate(block)))
            .ToList();

        _handlerMapping[opcode] = handlers;
    }
}