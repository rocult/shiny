using System.Collections.Immutable;

namespace MoonsecDeobfuscator.Ast.Expressions;

public enum BinaryOperator
{
    Add,
    Sub,
    Mul,
    Div,
    Mod,
    Pow,
    Equals,
    NotEquals,
    LessThan,
    GreaterThan,
    LessThanOrEquals,
    GreaterThanOrEquals,
    And,
    Or,
    Concat
}

public class BinaryExpression(BinaryOperator @operator, Expression left, Expression right) : Expression
{
    public BinaryOperator Operator { get; set; } = @operator;
    public Expression Left { get; set; } = left;
    public Expression Right { get; set; } = right;

    public override ImmutableList<Node> ChildNodes() => [Left, Right];

    public override BinaryExpression Clone() => new(Operator, Left.Clone(), Right.Clone());

    public override void Accept(AstWalker visitor)
    {
        visitor.Visit(this);
    }

    public override Node Accept(AstRewriter visitor) => visitor.Visit(this);
}