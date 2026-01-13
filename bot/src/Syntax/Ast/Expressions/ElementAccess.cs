using System.Collections.Immutable;

namespace MoonsecDeobfuscator.Ast.Expressions;

public class ElementAccess(Expression table, Expression key) : Variable
{
    public Expression Table { get; set; } = table;
    public Expression Key { get; set; } = key;

    public override ImmutableList<Node> ChildNodes() => [Table, Key];

    public override ElementAccess Clone() => new(Table.Clone(), Key.Clone());

    public override void Accept(AstWalker visitor)
    {
        visitor.Visit(this);
    }

    public override Node Accept(AstRewriter visitor) => visitor.Visit(this);
}