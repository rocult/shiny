namespace MoonsecDeobfuscator.Ast.Statements;

public class Break : Statement
{
    public override Break Clone() => new();

    public override void Accept(AstWalker visitor)
    {
        visitor.Visit(this);
    }

    public override Node Accept(AstRewriter visitor) => visitor.Visit(this);
}