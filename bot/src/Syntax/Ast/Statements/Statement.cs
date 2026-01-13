namespace MoonsecDeobfuscator.Ast.Statements;

public abstract class Statement : Node
{
    public abstract override Statement Clone();
}