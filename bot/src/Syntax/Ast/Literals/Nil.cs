namespace MoonsecDeobfuscator.Ast.Literals;

public class Nil : Literal
{
    public override Nil Clone() => new();

    public override void Accept(AstWalker visitor)
    {
        visitor.Visit(this);
    }

    public override Node Accept(AstRewriter visitor) => visitor.Visit(this);
}