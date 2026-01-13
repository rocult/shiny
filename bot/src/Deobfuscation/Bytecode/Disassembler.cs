using System.Text;
using MoonsecDeobfuscator.Bytecode.Models;

namespace MoonsecDeobfuscator.Deobfuscation.Bytecode;

public class Disassembler(Function rootFunction)
{
    private readonly StringBuilder _builder = new();

    public string Disassemble()
    {
        _builder.AppendLine("-- wsg hello join galactic services rn!");
        DisassembleFunction(rootFunction);
        return _builder.ToString();
    }

    private void DisassembleFunction(Function function)
    {
        _builder.Append($"function {function.Name}(");

        for (var i = 0; i < function.NumParams; i++)
        {
            _builder.Append($"R{i}");

            if (i + 1 < function.NumParams)
                _builder.Append(", ");
        }

        if (function.IsVarArgFlag == 2)
            _builder.Append(function.NumParams > 0 ? ", ..." : "...");

        _builder.AppendLine(")");
        _builder.AppendLine(
            $"\t[Slots: {function.MaxStackSize}, Upvalues: {function.NumUpvalues}, Constants: {function.Constants.Count}]");

        var instructions = function.Instructions;

        for (var i = 0; i < instructions.Count; i++)
        {
            _builder.Append($"\t[{i,4}]\t");
            DisassembleInstruction(instructions[i]);
        }

        _builder.AppendLine("end");

        foreach (var childFunction in function.Functions)
            DisassembleFunction(childFunction);
    }

    private void DisassembleInstruction(Instruction instruction)
    {
        var A = instruction.A;
        var B = instruction.B;
        var C = instruction.C;

        if (instruction.OpCode != OpCode.Unknown)
            _builder.Append($"{instruction.OpCode,12}\t| {A,4} | {B,4} | {C,4} |");
        else if (instruction.IsDead)
            _builder.Append($"{"DEAD",12}\t| {"-",4} | {"-",4} | {"-",4} |");
        else
            _builder.Append($"{instruction.OpNum,12}\t| {A,4} | {B,4} | {C,4} |");

        if (instruction.OpCode != OpCode.Unknown)
        {
            _builder.Append('\t');
            _builder.Append(GenAnnotation(instruction));
        }

        _builder.AppendLine();
    }

    private static string GenAnnotation(Instruction instruction)
    {
        var A = instruction.A;
        var B = instruction.B;
        var C = instruction.C;
        var function = instruction.Function;

        switch (instruction.OpCode)
        {
            case OpCode.GetGlobal:
                return $"R{A} = {((StringConstant) function.Constants[B]).Value}";
            case OpCode.SetGlobal:
                return $"{((StringConstant) function.Constants[B]).Value} = R{A}";
            case OpCode.LoadK:
                return $"R{A} = {function.Constants.ElementAtOrDefault(B)}";
            case OpCode.LoadNil:
                return B - A == 0 ? $"R{A} = nil" : $"R{A}->R{B} = nil";
            case OpCode.LoadBool:
                var value = B != 0 ? "true" : "false";
                var skip = C != 0 ? "; PC += 1" : "";
                return $"R{A} = {value}{skip}";
            case OpCode.Move:
                return $"R{A} = R{B}";
            case OpCode.Jmp:
                return $"PC += {B}";
            case OpCode.GetUpval:
                return $"R{A} = UPVALUE_{B}";
            case OpCode.SetUpval:
                return $"UPVALUE_{B} = R{A}";
            case OpCode.GetTable:
                return $"R{A} = R{B}[{RegisterOrConstant(C, function)}]";
            case OpCode.SetTable:
                return $"R{A}[{RegisterOrConstant(B, function)}] = {RegisterOrConstant(C, function)}";
            case OpCode.NewTable:
                return $"R{A} = {{}}";
            case OpCode.Self:
                return $"R{A + 1} = R{B}; R{A} = R{B}[{RegisterOrConstant(C, function)}]";
            case OpCode.Add:
                return $"R{A} = {RegisterOrConstant(B, function)} + {RegisterOrConstant(C, function)}";
            case OpCode.Sub:
                return $"R{A} = {RegisterOrConstant(B, function)} - {RegisterOrConstant(C, function)}";
            case OpCode.Mul:
                return $"R{A} = {RegisterOrConstant(B, function)} * {RegisterOrConstant(C, function)}";
            case OpCode.Div:
                return $"R{A} = {RegisterOrConstant(B, function)} / {RegisterOrConstant(C, function)}";
            case OpCode.Mod:
                return $"R{A} = {RegisterOrConstant(B, function)} % {RegisterOrConstant(C, function)}";
            case OpCode.Pow:
                return $"R{A} = {RegisterOrConstant(B, function)} ^ {RegisterOrConstant(C, function)}";
            case OpCode.Unm:
                return $"R{A} = -R{B}";
            case OpCode.Not:
                return $"R{A} = not R{B}";
            case OpCode.Len:
                return $"R{A} = #R{B}";
            case OpCode.Concat:
                var result = new StringBuilder();
                result.Append($"R{A} = ");

                for (var i = B; i <= C; i++)
                {
                    result.Append($"R{i}");

                    if (i + 1 <= C)
                        result.Append(" .. ");
                }

                return result.ToString();
            case OpCode.Eq:
            case OpCode.Lt:
            case OpCode.Le:
                var op = instruction.OpCode switch
                {
                    OpCode.Eq => A == 0 ? "==" : "~=",
                    OpCode.Lt => A == 0 ? "<" : ">",
                    OpCode.Le => A == 0 ? "<=" : ">=",
                    _ => " ?? "
                };
                return $"if {RegisterOrConstant(B, function)} {op} {RegisterOrConstant(C, function)} then PC += 1";
            case OpCode.Test:
                return $"if {(C == 0 ? $"not R{A}" : $"R{A}")} then PC += 1";
            case OpCode.TestSet:
                return $"if {(C == 0 ? $"not R{B}" : $"R{B}")} then R{A} = R{B} else PC += 1";
            case OpCode.Call:
                var lhs = C switch
                {
                    0 => $"R{A}->top = ",
                    1 => "",
                    _ => string.Join(", ", Enumerable.Range(0, C - 1).Select(i => $"R{A + i}")) + " = "
                };
                var args = B switch
                {
                    0 => $"R{A + 1}->top",
                    1 => "",
                    _ => string.Join(", ", Enumerable.Range(1, B - 1).Select(i => $"R{A + i}"))
                };

                var r = $"{lhs}R{A}({args})";
                return r;
            case OpCode.TailCall:
                return B switch
                {
                    > 1 => $"return R{A}({string.Join(", ", Enumerable.Range(1, B - 1).Select(i => $"R{A + i}"))})",
                    0 => $"return R{A}()",
                    1 => $"return R{A}(R{A + 1})",
                    _ => $"return R{A}()"
                };
            case OpCode.Return:
                return B switch
                {
                    0 => $"return R{A}->top",
                    1 => "return",
                    _ => $"return {string.Join(", ", Enumerable.Range(0, B - 1).Select(i => $"R{A + i}"))}"
                };
            case OpCode.ForLoop:
                return $"R{A} += R{A + 2}; if loop continues then PC += {B}; R{A + 3} = R{A};";
            case OpCode.ForPrep:
                return $"R{A} -= R{A + 2}; PC += {B}";
            case OpCode.TForLoop:
                var targets = string.Join(", ", Enumerable.Range(3, C).Select(i => $"R{A + i}"));
                var call = $"R{A}(R{A + 1}, R{A + 2})";
                var body = $"if R{A + 3} ~= nil then R{A + 2} = R{A + 3} else PC += 1 end";
                return $"{targets} = {call}; {body}";
            case OpCode.Closure:
                return $"R{A} = {function.Functions[B].Name}";
            case OpCode.VarArg:
                var a = B switch
                {
                    0 => $"R{A}->top",
                    1 => "",
                    _ => string.Join(", ", Enumerable.Range(0, B - 1).Select(i => $"R{A + i}"))
                };
                return $"{a} = ...";
        }

        return "";
    }

    private static string RegisterOrConstant(int register, Function function) =>
        register > 255 ? function.Constants[register - 256].ToString()! : $"R{register}";
}