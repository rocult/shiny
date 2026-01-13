using System.Collections.Immutable;
using MoonsecDeobfuscator.Ast.Expressions;

namespace MoonsecDeobfuscator.Ast.Statements;

public class Label(Name name) : Statement
{
    public Name Name { get; set; } = name;

    public override ImmutableList<Node> ChildNodes() => [Name];

    public override Label Clone() => new(Name.Clone());

    public override void Accept(AstWalker visitor)
    {
        visitor.Visit(this);
    }

    public override Node Accept(AstRewriter visitor) => visitor.Visit(this);
}