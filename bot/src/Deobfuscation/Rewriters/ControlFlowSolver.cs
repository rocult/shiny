using MoonsecDeobfuscator.Ast;
using MoonsecDeobfuscator.Ast.Expressions;
using MoonsecDeobfuscator.Ast.Literals;
using MoonsecDeobfuscator.Ast.Statements;
using MoonsecDeobfuscator.Deobfuscation.Utils;

namespace MoonsecDeobfuscator.Deobfuscation.Rewriters;

public class ControlFlowSolver : AstRewriter
{
    public override Node Visit(Repeat node) =>
        node is { Condition: BooleanLiteral { Value: true } } ? CreateReplacement(node.Body) : node;

    public override Node Visit(NumericFor node)
    {
        var match = node is
        {
            Body.Statements: [If { IfClause.Body.Statements: [.., Break or Do] }, ..] or [.., Break or Do]
        };

        return match ? CreateReplacement(node.Body) : node;
    }

    /*
        FiveM does this shit
        if (-3 ~= n) then
            if (0 < n) then
                r = f
                goto omluRWOc
            end
            d = e
            ::omluRWOc::
        else
    */
    public override Node Visit(If node) =>
        node.IfClause.Body.Statements is [If, .., Label] ? CreateReplacement(node.IfClause.Body) : node;

    public override Node Visit(Block node)
    {
        RemoveStateMachines(node);
        RemoveUnreachable(node);
        return node;
    }

    private static void RemoveUnreachable(Block node)
    {
        var statements = node.Statements;
        var idx = statements.FindIndex(stat => stat is Do or Return or Break);

        if (idx != -1 && idx + 1 < statements.Count)
            statements.RemoveRange(idx + 1, statements.Count - idx - 1);
    }

    private static void RemoveStateMachines(Block node)
    {
        var stats = node.Statements;
        var stateMachines = stats.Where(IsStateMachine).ToList();

        if (stateMachines.Count == 0)
            return;

        foreach (var stateMachine in stateMachines)
        {
            var idx = stats.IndexOf(stateMachine);
            var stateName = GetStateName(stateMachine);
            var tree = GetTree(stateMachine);

            var solved = new TreeSolver(tree, stateName).Solve()
                .OrderBy(it => it.Key)
                .SelectMany(it => it.Value.Statements);

            stats.RemoveAt(idx);
            stats.InsertRange(idx, solved);
        }
    }

    private static If CreateReplacement(Block node)
    {
        var innerIf = (If) node.Statements[0];
        var ifBody = new Block();
        var elseBody = new Block();

        ifBody.Statements.AddRange(innerIf.IfClause.Body.Statements
            .TakeWhile(stat => stat is not (Break or Label or Goto)));
        elseBody.Statements.AddRange(node.Statements
            .Skip(1)
            .TakeWhile(stat => stat is not (Break or Label or Goto)));

        var clause = new If.Clause(innerIf.IfClause.Condition, ifBody);

        return new If(clause, [], elseBody);
    }

    private static bool IsStateMachine(Statement node)
    {
        if (node is While { Condition: BinaryExpression { Right: NumberLiteral { Value: -1 } } })
            return true;

        return node is NumericFor
        {
            InitialValue: NumberLiteral,
            FinalValue: NumberLiteral,
            Body.Statements: [If]
        };
    }

    private static string GetStateName(Statement node)
    {
        return node is While w
            ? ((Name) ((BinaryExpression) w.Condition).Left).Value
            : ((NumericFor) node).IteratorName.Value;
    }

    private static If GetTree(Statement node) =>
        (If) (node is While w ? w.Body.Statements[0] : ((NumericFor) node).Body.Statements[0]);
}