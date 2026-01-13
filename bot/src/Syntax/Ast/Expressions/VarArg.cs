namespace MoonsecDeobfuscator.Ast.Expressions;

public class VarArg : Expression
{
    public override VarArg Clone() => new();

    public override void Accept(AstWalker visitor)
    {
        visitor.Visit(this);
    }

    public override Node Accept(AstRewriter visitor) => visitor.Visit(this);
}