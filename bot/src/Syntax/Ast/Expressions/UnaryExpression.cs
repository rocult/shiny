using System.Collections.Immutable;

namespace MoonsecDeobfuscator.Ast.Expressions;

public enum UnaryOperator
{
    Not,
    Length,
    Negate
}

public class UnaryExpression(UnaryOperator @operator, Expression operand) : Expression
{
    public Expression Operand { get; set; } = operand;
    public UnaryOperator Operator { get; set; } = @operator;

    public override ImmutableList<Node> ChildNodes() => [Operand];

    public override UnaryExpression Clone() => new(Operator, Operand.Clone());

    public override void Accept(AstWalker visitor)
    {
        visitor.Visit(this);
    }

    public override Node Accept(AstRewriter visitor) => visitor.Visit(this);
}