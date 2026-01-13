namespace MoonsecDeobfuscator.Bytecode.Models;

public abstract class Constant;

public class StringConstant(string value) : Constant
{
    public readonly string Value = value;

    public override string ToString() => $"\"{Value}\"";
}

public class NumberConstant(double value) : Constant
{
    public readonly double Value = value;

    public override string ToString() => Value.ToString();
}

public class BooleanConstant(bool value) : Constant
{
    public readonly bool Value = value;

    public override string ToString() => Value.ToString().ToLower();
}

public class NilConstant : Constant
{
    public override string ToString() => "nil";
}