using System.Collections.Immutable;
using MoonsecDeobfuscator.Ast.Expressions;

namespace MoonsecDeobfuscator.Ast.Statements;

public class Return(NodeList<Expression> values) : Statement
{
    public NodeList<Expression> Values { get; set; } = values;

    public Return() : this([])
    {
    }

    public override ImmutableList<Node> ChildNodes() => [..Values];

    public override Return Clone() => new(Values.Clone());

    public override void Accept(AstWalker visitor)
    {
        visitor.Visit(this);
    }

    public override Node Accept(AstRewriter visitor) => visitor.Visit(this);
}