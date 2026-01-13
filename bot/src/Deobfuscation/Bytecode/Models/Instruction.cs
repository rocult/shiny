namespace MoonsecDeobfuscator.Bytecode.Models;

public class Instruction
{
    public int A, B, C, OpNum, PC;
    public bool IsKA, IsKB, IsKC, IsDead;

    public Function Function = null!;
    public OpCode OpCode = OpCode.Unknown;

    public override string ToString()
    {
        if (OpCode != OpCode.Unknown)
            return $"{OpCode,12}\t| {A,4} | {B,4} | {C,4} |";

        if (IsDead)
            return $"{"DEAD",12}\t| {"-",4} | {"-",4} | {"-",4} |";

        return $"{OpNum,12}\t| {A,4} | {B,4} | {C,4} |";
    }

    public OpType GetOpType()
    {
        switch (OpCode)
        {
            case OpCode.Move:
            case OpCode.LoadNil:
            case OpCode.GetUpval:
            case OpCode.SetUpval:
            case OpCode.Unm:
            case OpCode.Not:
            case OpCode.Len:
            case OpCode.Return:
            case OpCode.VarArg:
                return OpType.AB;
            case OpCode.LoadK:
            case OpCode.GetGlobal:
            case OpCode.SetGlobal:
            case OpCode.Closure:
                return OpType.ABx;
            case OpCode.LoadBool:
            case OpCode.GetTable:
            case OpCode.SetTable:
            case OpCode.Add:
            case OpCode.Sub:
            case OpCode.Mul:
            case OpCode.Div:
            case OpCode.Mod:
            case OpCode.Pow:
            case OpCode.Concat:
            case OpCode.Call:
            case OpCode.TailCall:
            case OpCode.Self:
            case OpCode.Eq:
            case OpCode.Lt:
            case OpCode.Le:
            case OpCode.TestSet:
            case OpCode.NewTable:
            case OpCode.SetList:
                return OpType.ABC;
            case OpCode.Jmp:
                return OpType.sBx;
            case OpCode.Test:
            case OpCode.TForLoop:
                return OpType.AC;
            case OpCode.ForPrep:
            case OpCode.ForLoop:
                return OpType.AsBx;
            case OpCode.Close:
                return OpType.A;
        }

        throw new Exception("Invalid opcode in GetOpType");
    }
}