namespace MoonsecDeobfuscator.Ast.Literals;

public class NumberLiteral(double value) : Literal
{
    public double Value { get; set; } = value;

    public override NumberLiteral Clone() => new(Value);

    public override void Accept(AstWalker visitor)
    {
        visitor.Visit(this);
    }

    public override Node Accept(AstRewriter visitor) => visitor.Visit(this);
}