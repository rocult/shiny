using System.Text;
using MoonsecDeobfuscator.Bytecode.Models;

namespace MoonsecDeobfuscator.Deobfuscation.Bytecode;

public class Serializer(Stream stream) : BinaryWriter(stream)
{
    private void WriteString(string data)
    {
        var bytes = Encoding.UTF8.GetBytes(data);

        Write((ulong) (bytes.Length + 1));
        Write(bytes);
        Write((byte) 0);
    }

    private void WriteFunction(Function function)
    {
        WriteString("");
        Write(0);
        Write(0);

        Write(function.NumUpvalues);
        Write(function.NumParams);
        Write(function.IsVarArgFlag);
        Write(function.MaxStackSize);

        WriteInstructions(function.Instructions);
        WriteConstants(function.Constants);
        WriteFunctions(function.Functions);

        Write(0);
        Write(0);
        Write(0);
    }

    private void WriteFunctions(List<Function> functions)
    {
        Write(functions.Count);

        foreach (var function in functions)
            WriteFunction(function);
    }

    private void WriteConstants(List<Constant> constants)
    {
        Write(constants.Count);

        foreach (var constant in constants)
        {
            switch (constant)
            {
                case StringConstant sc:
                    Write((byte) 4);
                    WriteString(sc.Value);
                    break;
                case NumberConstant nc:
                    Write((byte) 3);
                    Write(nc.Value);
                    break;
                case BooleanConstant bc:
                    Write((byte) 1);
                    Write((byte) (bc.Value ? 1 : 0));
                    break;
                default:
                    Write((byte) 0);
                    break;
            }
        }
    }

    private void WriteInstructions(List<Instruction> instructions)
    {
        Write(instructions.Count);

        foreach (var instruction in instructions)
        {
            var data = 0u;
            data |= (uint) instruction.OpCode;

            switch (instruction.GetOpType())
            {
                case OpType.A:
                    data |= Mask(instruction.A, 8) << 6;
                    break;
                case OpType.AB:
                    data |= Mask(instruction.A, 8) << 6;
                    data |= Mask(instruction.B, 9) << 23;
                    break;
                case OpType.AC:
                    data |= Mask(instruction.A, 8) << 6;
                    data |= Mask(instruction.C, 9) << 14;
                    break;
                case OpType.ABC:
                    data |= Mask(instruction.A, 8) << 6;
                    data |= Mask(instruction.B, 9) << 23;
                    data |= Mask(instruction.C, 9) << 14;
                    break;
                case OpType.ABx:
                    data |= Mask(instruction.A, 8) << 6;
                    data |= Mask(instruction.B, 18) << 14;
                    break;
                case OpType.AsBx:
                    data |= Mask(instruction.A, 8) << 6;
                    data |= Mask(instruction.B + 131_071, 18) << 14;
                    break;
                case OpType.sBx:
                    data |= Mask(instruction.B + 131_071, 18) << 14;
                    break;
            }

            Write(data);
        }
    }

    private void WriteHeader()
    {
        Write(
        [
            0x1B, 0x4C, 0x75, 0x61,
            0x51,
            0,
            1,
            4,
            8,
            4,
            8,
            0
        ]);
    }

    public void Serialize(Function function)
    {
        WriteHeader();
        WriteFunction(function);
    }

    private static uint Mask(int value, int bits) => (uint) value & ((1u << bits) - 1);
}