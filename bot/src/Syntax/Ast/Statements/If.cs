using System.Collections.Immutable;
using MoonsecDeobfuscator.Ast.Expressions;

namespace MoonsecDeobfuscator.Ast.Statements;

public class If(If.Clause ifClause, NodeList<If.Clause> elseIfClauses, Block? elseBody = null) : Statement
{
    public Clause IfClause { get; set; } = ifClause;
    public NodeList<Clause> ElseIfClauses { get; set; } = elseIfClauses;
    public Block? ElseBody { get; set; } = elseBody;

    public If(Clause ifClause) : this(ifClause, [])
    {
    }

    public override ImmutableList<Node> ChildNodes() =>
        ElseBody != null ? [IfClause, ..ElseIfClauses, ElseBody] : [IfClause, ..ElseIfClauses];

    public override If Clone() => new(IfClause.Clone(), ElseIfClauses.Clone(), ElseBody?.Clone());

    public override void Accept(AstWalker visitor)
    {
        visitor.Visit(this);
    }

    public override Node Accept(AstRewriter visitor) => visitor.Visit(this);

    public class Clause(Expression condition, Block body) : Node
    {
        public Expression Condition { get; set; } = condition;
        public Block Body { get; set; } = body;

        public override ImmutableList<Node> ChildNodes() => [Condition, Body];

        public override Clause Clone() => new(Condition.Clone(), Body.Clone());

        public override void Accept(AstWalker visitor)
        {
            visitor.Visit(this);
        }

        public override Node Accept(AstRewriter visitor) => visitor.Visit(this);
    }
}