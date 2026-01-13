namespace MoonsecDeobfuscator.Ast.Expressions;

public abstract class Expression : Node
{
    public abstract override Expression Clone();
}