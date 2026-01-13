using System.Collections.Immutable;

namespace MoonsecDeobfuscator.Ast.Expressions;

public class Table(NodeList<Table.Entry> entries) : Expression
{
    public NodeList<Entry> Entries { get; set; } = entries;

    public Table() : this([])
    {
    }

    public override ImmutableList<Node> ChildNodes() => [..Entries];

    public override Table Clone() => new(Entries.Clone());

    public override void Accept(AstWalker visitor)
    {
        visitor.Visit(this);
    }

    public override Node Accept(AstRewriter visitor) => visitor.Visit(this);

    public class Entry(Expression? key, Expression value) : Expression
    {
        public Expression? Key { get; set; } = key;
        public Expression Value { get; set; } = value;

        public Entry(Expression value) : this(null, value)
        {
        }

        public override ImmutableList<Node> ChildNodes() => Key != null ? [Key!, Value] : [Value];

        public override Entry Clone() => new(Key?.Clone(), Value.Clone());

        public override void Accept(AstWalker visitor)
        {
            visitor.Visit(this);
        }

        public override Node Accept(AstRewriter visitor) => visitor.Visit(this);
    }
}