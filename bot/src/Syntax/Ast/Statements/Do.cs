using System.Collections.Immutable;

namespace MoonsecDeobfuscator.Ast.Statements;

public class Do(Block body) : Statement
{
    public Block Body { get; set; } = body;

    public override ImmutableList<Node> ChildNodes() => [Body];

    public override Do Clone() => new(Body.Clone());

    public override void Accept(AstWalker visitor)
    {
        visitor.Visit(this);
    }

    public override Node Accept(AstRewriter visitor) => visitor.Visit(this);
}