using MoonsecDeobfuscator.Ast.Expressions;

namespace MoonsecDeobfuscator.Ast.Literals;

public abstract class Literal : Expression
{
    public abstract override Literal Clone();
}