using System.Collections.Immutable;
using MoonsecDeobfuscator.Ast.Expressions;

namespace MoonsecDeobfuscator.Ast.Statements;

public class While(Expression condition, Block body) : Statement
{
    public Expression Condition { get; set; } = condition;
    public Block Body { get; set; } = body;

    public override ImmutableList<Node> ChildNodes() => [Condition, Body];

    public override While Clone() => new(Condition.Clone(), Body.Clone());

    public override void Accept(AstWalker visitor)
    {
        visitor.Visit(this);
    }

    public override Node Accept(AstRewriter visitor) => visitor.Visit(this);
}