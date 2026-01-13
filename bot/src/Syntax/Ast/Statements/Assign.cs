using System.Collections.Immutable;
using MoonsecDeobfuscator.Ast.Expressions;

namespace MoonsecDeobfuscator.Ast.Statements;

public class Assign(NodeList<Variable> variables, NodeList<Expression> values) : Statement
{
    public NodeList<Variable> Variables { get; set; } = variables;
    public NodeList<Expression> Values { get; set; } = values;

    public override ImmutableList<Node> ChildNodes() => [..Variables, ..Values];

    public override Assign Clone() => new(Variables.Clone(), Values.Clone());

    public override void Accept(AstWalker visitor)
    {
        visitor.Visit(this);
    }

    public override Node Accept(AstRewriter visitor) => visitor.Visit(this);
}