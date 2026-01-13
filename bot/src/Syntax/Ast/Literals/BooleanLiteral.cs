namespace MoonsecDeobfuscator.Ast.Literals;

public class BooleanLiteral(bool value) : Literal
{
    public bool Value { get; set; } = value;

    public override BooleanLiteral Clone() => new(Value);

    public override void Accept(AstWalker visitor)
    {
        visitor.Visit(this);
    }

    public override Node Accept(AstRewriter visitor) => visitor.Visit(this);
}