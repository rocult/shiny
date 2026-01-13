namespace MoonsecDeobfuscator.Ast.Expressions;

public class Name(string value) : Variable
{
    public string Value { get; set; } = value;

    public override Name Clone() => new(Value);

    public override void Accept(AstWalker visitor)
    {
        visitor.Visit(this);
    }

    public override Node Accept(AstRewriter visitor) => visitor.Visit(this);
}