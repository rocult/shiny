namespace MoonsecDeobfuscator.Ast.Literals;

public class StringLiteral(string value) : Literal
{
    public string Value { get; set; } = value;

    public override StringLiteral Clone() => new(Value);

    public override void Accept(AstWalker visitor)
    {
        visitor.Visit(this);
    }

    public override Node Accept(AstRewriter visitor) => visitor.Visit(this);
}