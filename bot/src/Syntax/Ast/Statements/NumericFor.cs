using System.Collections.Immutable;
using MoonsecDeobfuscator.Ast.Expressions;

namespace MoonsecDeobfuscator.Ast.Statements;

public class NumericFor : Statement
{
    public Name IteratorName { get; set; }
    public Expression InitialValue { get; set; }
    public Expression FinalValue { get; set; }
    public Expression? Step { get; set; }
    public Block Body { get; set; }

    public NumericFor(Name iteratorName, Expression initialValue, Expression finalValue, Expression? step, Block body)
    {
        IteratorName = iteratorName;
        InitialValue = initialValue;
        FinalValue = finalValue;
        Step = step;
        Body = body;
    }

    public NumericFor(Name iteratorName, Expression initialValue, Expression finalValue, Block body)
    {
        IteratorName = iteratorName;
        InitialValue = initialValue;
        FinalValue = finalValue;
        Body = body;
    }

    public override ImmutableList<Node> ChildNodes() => Step != null
        ? [IteratorName, InitialValue, FinalValue, Step!, Body]
        : [IteratorName, InitialValue, FinalValue, Body];

    public override NumericFor Clone() =>
        new(IteratorName.Clone(), InitialValue.Clone(), FinalValue.Clone(), Step?.Clone(), Body.Clone());

    public override void Accept(AstWalker visitor)
    {
        visitor.Visit(this);
    }

    public override Node Accept(AstRewriter visitor) => visitor.Visit(this);
}