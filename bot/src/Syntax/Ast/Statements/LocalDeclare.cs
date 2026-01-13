using System.Collections.Immutable;
using MoonsecDeobfuscator.Ast.Expressions;

namespace MoonsecDeobfuscator.Ast.Statements;

public class LocalDeclare(NodeList<Name> names, NodeList<Expression> values) : Statement
{
    public NodeList<Name> Names { get; set; } = names;
    public NodeList<Expression> Values { get; set; } = values;

    public LocalDeclare(NodeList<Name> names) : this(names, [])
    {
    }

    public override ImmutableList<Node> ChildNodes() => [..Names, ..Values];

    public override LocalDeclare Clone() => new(Names.Clone(), Values.Clone());

    public override void Accept(AstWalker visitor)
    {
        visitor.Visit(this);
    }

    public override Node Accept(AstRewriter visitor) => visitor.Visit(this);
}