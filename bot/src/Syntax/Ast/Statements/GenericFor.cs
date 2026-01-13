using System.Collections.Immutable;
using MoonsecDeobfuscator.Ast.Expressions;

namespace MoonsecDeobfuscator.Ast.Statements;

public class GenericFor(NodeList<Name> names, NodeList<Expression> expressions, Block body) : Statement
{
    public NodeList<Name> Names { get; set; } = names;
    public NodeList<Expression> Expressions { get; set; } = expressions;
    public Block Body { get; set; } = body;

    public override ImmutableList<Node> ChildNodes() => [..Names, ..Expressions, Body];

    public override GenericFor Clone() => new(Names.Clone(), Expressions.Clone(), Body.Clone());

    public override void Accept(AstWalker visitor)
    {
        visitor.Visit(this);
    }

    public override Node Accept(AstRewriter visitor) => visitor.Visit(this);
}