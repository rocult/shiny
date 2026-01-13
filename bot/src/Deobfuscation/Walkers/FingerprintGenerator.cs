using System.Text;
using MoonsecDeobfuscator.Ast;
using MoonsecDeobfuscator.Ast.Expressions;
using MoonsecDeobfuscator.Ast.Statements;

namespace MoonsecDeobfuscator.Deobfuscation.Walkers;

public class FingerprintGenerator : AstWalker
{
    private enum Operation
    {
        InstRef,
        StkRef,
        UpvRef,
        EnvRef,
        VarArgRef,
        UnpackRef,
        InsertRef,
        SetMetatableRef,
        WrapProtoRef,
        Access,
        If,
        ElseIf,
        Else,
        For,
        Call,
        Add,
        Sub,
        Mul,
        Div,
        Mod,
        Pow,
        LessThan,
        GreaterThan,
        LessThanOrEquals,
        GreaterThanOrEquals,
        Equals,
        NotEquals,
        Return,
        Len,
        PcRef,
        TopRef,
        Negate,
        Concat,
        Table,
        Not,
        And,
        Or
    }

    private readonly List<Operation> _operations = [];

    public override void Visit(Name node)
    {
        switch (node.Value)
        {
            case "stk":
                _operations.Add(Operation.StkRef);
                break;
            case "env":
                _operations.Add(Operation.EnvRef);
                break;
            case "upv":
                _operations.Add(Operation.UpvRef);
                break;
            case "inst":
                _operations.Add(Operation.InstRef);
                break;
            case "vararg":
                _operations.Add(Operation.VarArgRef);
                break;
            case "unpack":
                _operations.Add(Operation.UnpackRef);
                break;
            case "setmetatable":
                _operations.Add(Operation.SetMetatableRef);
                break;
            case "pc":
                _operations.Add(Operation.PcRef);
                break;
            case "top":
                _operations.Add(Operation.TopRef);
                break;
            case "wrap_proto":
                _operations.Add(Operation.WrapProtoRef);
                break;
        }
    }

    public override void Visit(MemberAccess node)
    {
        if (node is { Table: Name { Value: "table" }, Key.Value: "insert" })
            _operations.Add(Operation.InsertRef);
        else
            _operations.Add(Operation.Access);

        base.Visit(node);
    }

    public override void Visit(ElementAccess node)
    {
        _operations.Add(Operation.Access);
        base.Visit(node);
    }

    public override void Visit(If node)
    {
        _operations.Add(Operation.If);

        foreach (var _ in node.ElseIfClauses)
            _operations.Add(Operation.ElseIf);

        if (node.ElseBody != null)
            _operations.Add(Operation.Else);

        base.Visit(node);
    }

    public override void Visit(NumericFor node)
    {
        _operations.Add(Operation.For);
        base.Visit(node);
    }

    public override void Visit(Call node)
    {
        _operations.Add(Operation.Call);
        base.Visit(node);
    }

    public override void Visit(BinaryExpression node)
    {
        // Java switches are much better...
        switch (node.Operator)
        {
            case BinaryOperator.Add:
                _operations.Add(Operation.Add);
                break;
            case BinaryOperator.Sub:
                _operations.Add(Operation.Sub);
                break;
            case BinaryOperator.Mul:
                _operations.Add(Operation.Mul);
                break;
            case BinaryOperator.Div:
                _operations.Add(Operation.Div);
                break;
            case BinaryOperator.Mod:
                _operations.Add(Operation.Mod);
                break;
            case BinaryOperator.Pow:
                _operations.Add(Operation.Pow);
                break;
            case BinaryOperator.LessThan:
                _operations.Add(Operation.LessThan);
                break;
            case BinaryOperator.LessThanOrEquals:
                _operations.Add(Operation.LessThanOrEquals);
                break;
            case BinaryOperator.GreaterThan:
                _operations.Add(Operation.GreaterThan);
                break;
            case BinaryOperator.GreaterThanOrEquals:
                _operations.Add(Operation.GreaterThanOrEquals);
                break;
            case BinaryOperator.Equals:
                _operations.Add(Operation.Equals);
                break;
            case BinaryOperator.NotEquals:
                _operations.Add(Operation.NotEquals);
                break;
            case BinaryOperator.Concat:
                _operations.Add(Operation.Concat);
                break;
            case BinaryOperator.And:
                _operations.Add(Operation.And);
                break;
            case BinaryOperator.Or:
                _operations.Add(Operation.Or);
                break;
        }

        base.Visit(node);
    }

    public override void Visit(UnaryExpression node)
    {
        switch (node.Operator)
        {
            case UnaryOperator.Length:
                _operations.Add(Operation.Len);
                break;
            case UnaryOperator.Negate:
                _operations.Add(Operation.Negate);
                break;
            case UnaryOperator.Not:
                _operations.Add(Operation.Not);
                break;
        }

        base.Visit(node);
    }

    public override void Visit(Return node)
    {
        _operations.Add(Operation.Return);
        base.Visit(node);
    }

    public override void Visit(Table node)
    {
        _operations.Add(Operation.Table);
        base.Visit(node);
    }

    public static string Generate(Block node)
    {
        var walker = new FingerprintGenerator();
        walker.Visit(node);

        var result = new StringBuilder();

        foreach (var operation in walker._operations)
            result.Append((int) operation);

        return result.ToString();
    }
}