using System.Collections.Immutable;
using MoonsecDeobfuscator.Ast.Expressions;

namespace MoonsecDeobfuscator.Ast.Statements;

public class ExpressionStatement(Expression expression) : Statement
{
    public Expression Expression { get; set; } = expression;

    public override ImmutableList<Node> ChildNodes() => [Expression];

    public override ExpressionStatement Clone() => new(Expression.Clone());

    public override void Accept(AstWalker visitor)
    {
        visitor.Visit(this);
    }

    public override Node Accept(AstRewriter visitor) => visitor.Visit(this);
}