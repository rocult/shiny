using System.Collections.Immutable;
using MoonsecDeobfuscator.Ast.Expressions;

namespace MoonsecDeobfuscator.Ast.Statements;

public class LocalFunction(Name name, ParameterList parameters, Block body) : Statement
{
    public Name Name { get; set; } = name;
    public ParameterList Parameters { get; set; } = parameters;
    public Block Body { get; set; } = body;

    public LocalFunction(Name name, Block body) : this(name, new ParameterList(), body)
    {
    }

    public override ImmutableList<Node> ChildNodes() => [Name, Parameters, Body];

    public override LocalFunction Clone() => new(Name.Clone(), Parameters.Clone(), Body.Clone());

    public override void Accept(AstWalker visitor)
    {
        visitor.Visit(this);
    }

    public override Node Accept(AstRewriter visitor) => visitor.Visit(this);
}