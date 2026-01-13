using System.Collections.Immutable;
using MoonsecDeobfuscator.Ast.Expressions;

namespace MoonsecDeobfuscator.Ast.Statements;

public class Repeat(Block body, Expression condition) : Statement
{
    public Block Body { get; set; } = body;
    public Expression Condition { get; set; } = condition;

    public override ImmutableList<Node> ChildNodes() => [Body, Condition];

    public override Repeat Clone() => new(Body.Clone(), Condition.Clone());

    public override void Accept(AstWalker visitor)
    {
        visitor.Visit(this);
    }

    public override Node Accept(AstRewriter visitor) => visitor.Visit(this);
}