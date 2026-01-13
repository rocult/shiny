using System.Collections.Immutable;

namespace MoonsecDeobfuscator.Ast.Expressions;

public class MethodCall(Expression function, Name methodName, NodeList<Expression> arguments) : Call(function, arguments)
{
    public Name MethodName { get; set; } = methodName;

    public override ImmutableList<Node> ChildNodes() => [Function, MethodName, ..Arguments];

    public override MethodCall Clone() => new(Function.Clone(), MethodName.Clone(), Arguments.Clone());

    public override void Accept(AstWalker visitor)
    {
        visitor.Visit(this);
    }

    public override Node Accept(AstRewriter visitor) => visitor.Visit(this);
}