using System.Collections.Immutable;

namespace MoonsecDeobfuscator.Ast.Expressions;

public class Call(Expression function, NodeList<Expression> arguments) : Expression
{
    public Expression Function { get; set; } = function;
    public NodeList<Expression> Arguments { get; set; } = arguments;

    public Call(Expression function) : this(function, [])
    {
    }

    public override ImmutableList<Node> ChildNodes() => [Function, ..Arguments];

    public override Call Clone() => new(Function.Clone(), Arguments.Clone());

    public override void Accept(AstWalker visitor)
    {
        visitor.Visit(this);
    }

    public override Node Accept(AstRewriter visitor) => visitor.Visit(this);
}