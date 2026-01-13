using System.Collections.Immutable;

namespace MoonsecDeobfuscator.Ast.Expressions;

public class AnonymousFunction(ParameterList parameters, Block body) : Expression
{
    public ParameterList Parameters { get; set; } = parameters;
    public Block Body { get; set; } = body;

    public AnonymousFunction(Block body) : this(new ParameterList(), body)
    {
    }

    public override ImmutableList<Node> ChildNodes() => [Parameters, Body];

    public override AnonymousFunction Clone() => new(Parameters.Clone(), Body.Clone());

    public override void Accept(AstWalker visitor)
    {
        visitor.Visit(this);
    }

    public override Node Accept(AstRewriter visitor) => visitor.Visit(this);
}