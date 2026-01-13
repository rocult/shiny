using MoonsecDeobfuscator.Bytecode.Models;
using MoonsecDeobfuscator.Deobfuscation.Utils;

namespace MoonsecDeobfuscator.Deobfuscation.Bytecode;

public enum ProtoStep
{
    Instructions,
    Constants,
    Functions,
    NumParams,

    StringConstant,
    NumberConstant,
    BooleanConstant
}

public class Deserializer(byte[] bytes, Context ctx) : BinaryReader(new MemoryStream(bytes))
{
    private List<Instruction> ReadInstructions(Function function)
    {
        var size = ReadInt32();
        var instructions = new List<Instruction>(size);

        for (var i = 0; i < size; i++)
        {
            var descriptor = ReadByte();

            if (GetBits(descriptor, 1, 1) != 0)
                continue;

            var type = GetBits(descriptor, 2, 3);
            var mask = GetBits(descriptor, 4, 6);
            var instruction = new Instruction
            {
                OpNum = ReadInt16(),
                A = ReadInt16(),
                PC = i,
                Function = function
            };

            switch (type)
            {
                case 0:
                    instruction.B = ReadInt16();
                    instruction.C = ReadInt16();
                    break;
                case 1:
                    instruction.B = ReadInt32();
                    break;
                case 2:
                    instruction.B = ReadInt32() - (1 << 16);
                    break;
                case 3:
                    instruction.B = ReadInt32() - (1 << 16);
                    instruction.C = ReadInt16();
                    break;
            }

            instruction.IsKA = GetBits(mask, 1, 1) == 1;
            instruction.IsKB = GetBits(mask, 2, 2) == 1;
            instruction.IsKC = GetBits(mask, 3, 3) == 1;

            instructions.Add(instruction);
        }

        return instructions;
    }

    private List<Function> ReadPrototypes()
    {
        var size = ReadInt32();
        var prototypes = new List<Function>(size);

        for (var i = 0; i < size; i++)
            prototypes.Add(ReadFunction());

        return prototypes;
    }

    private List<Constant> ReadConstants()
    {
        var size = ReadInt32();
        var constants = new List<Constant>(size);

        for (var i = 0; i < size; i++)
        {
            var typeFlag = ReadByte();

            if (!ctx.ConstantFormat.TryGetValue(typeFlag, out var type))
            {
                constants.Add(new NilConstant());
                continue;
            }

            switch (type)
            {
                case ProtoStep.BooleanConstant:
                    constants.Add(new BooleanConstant(ReadBoolean()));
                    break;
                case ProtoStep.NumberConstant:
                    constants.Add(new NumberConstant(ReadDouble()));
                    break;
                case ProtoStep.StringConstant:
                    var str = StringDecoding.DecodeConstant(ctx.ConstantKey, ReadBytes(ReadInt32()));
                    constants.Add(new StringConstant(str));
                    break;
            }
        }

        return constants;
    }

    public Function ReadFunction()
    {
        var function = new Function();

        foreach (var step in ctx.ProtoFormat)
        {
            switch (step)
            {
                case ProtoStep.Constants:
                    function.Constants = ReadConstants();
                    break;
                case ProtoStep.Instructions:
                    function.Instructions = ReadInstructions(function);
                    break;
                case ProtoStep.NumParams:
                    function.NumParams = ReadByte();
                    break;
                case ProtoStep.Functions:
                    function.Functions = ReadPrototypes();
                    break;
            }
        }

        return function;
    }

    private static int GetBits(int source, int start, int end) =>
        (source >> (start - 1)) & ((1 << (end - start + 1)) - 1);
}