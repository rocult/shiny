namespace MoonsecDeobfuscator.Bytecode.Models;

public class Function
{
    public List<Instruction> Instructions = null!;
    public List<Constant> Constants = null!;
    public List<Function> Functions = null!;

    public byte MaxStackSize, NumParams, NumUpvalues, IsVarArgFlag;
    public readonly string Name = $"func_{Guid.NewGuid().ToString("N")[..8]}";
}