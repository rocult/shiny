using MoonsecDeobfuscator.Ast.Expressions;
using MoonsecDeobfuscator.Ast.Literals;
using MoonsecDeobfuscator.Ast.Statements;
using MoonsecDeobfuscator.Syntax.Semantic;

namespace MoonsecDeobfuscator.Ast;

public abstract class AstWalker
{
    public SymbolTable? SymbolTable;

    public void Walk(Node node, bool symbols = false)
    {
        if (symbols)
            SymbolTable = SymbolTableBuilder.Build(node);

        Visit(node);
    }

    protected VariableInfo? GetVariable(string name) => SymbolTable?.GetVariable(name);

    protected virtual void VisitList<TNode>(NodeList<TNode> list) where TNode : Node
    {
        foreach (var node in list)
            Visit(node);
    }

    public virtual void Visit(Name node)
    {
    }

    public virtual void Visit(ElementAccess node)
    {
        Visit(node.Table);
        Visit(node.Key);
    }

    public virtual void Visit(MemberAccess node)
    {
        Visit(node.Table);
        Visit(node.Key);
    }

    public virtual void Visit(BinaryExpression node)
    {
        Visit(node.Left);
        Visit(node.Right);
    }

    public virtual void Visit(UnaryExpression node)
    {
        Visit(node.Operand);
    }

    public virtual void Visit(Call node)
    {
        Visit(node.Function);
        VisitList(node.Arguments);
    }

    public virtual void Visit(MethodCall node)
    {
        Visit(node.Function);
        Visit(node.MethodName);
        VisitList(node.Arguments);
    }

    public virtual void Visit(Table node)
    {
        VisitList(node.Entries);
    }

    public virtual void Visit(Table.Entry node)
    {
        var key = node.Key;

        if (key != null)
            Visit(key);

        Visit(node.Value);
    }

    public virtual void Visit(AnonymousFunction node)
    {
        Visit(node.Parameters);
        Visit(node.Body);
    }

    public virtual void Visit(VarArg node)
    {
    }

    public virtual void Visit(ExpressionStatement node)
    {
        Visit(node.Expression);
    }

    public virtual void Visit(While node)
    {
        Visit(node.Condition);
        Visit(node.Body);
    }

    public virtual void Visit(LocalDeclare node)
    {
        VisitList(node.Names);
        VisitList(node.Values);
    }

    public virtual void Visit(Assign node)
    {
        VisitList(node.Variables);
        VisitList(node.Values);
    }

    public virtual void Visit(GenericFor node)
    {
        VisitList(node.Names);
        VisitList(node.Expressions);
        Visit(node.Body);
    }

    public virtual void Visit(NumericFor node)
    {
        Visit(node.IteratorName);
        Visit(node.InitialValue);
        Visit(node.FinalValue);

        var step = node.Step;

        if (step != null)
            Visit(step);

        Visit(node.Body);
    }

    public virtual void Visit(If node)
    {
        Visit(node.IfClause);
        VisitList(node.ElseIfClauses);

        var elseBody = node.ElseBody;

        if (elseBody != null)
            Visit(elseBody);
    }

    public virtual void Visit(If.Clause node)
    {
        Visit(node.Condition);
        Visit(node.Body);
    }

    public virtual void Visit(LocalFunction node)
    {
        Visit(node.Name);
        Visit(node.Parameters);
        Visit(node.Body);
    }

    public virtual void Visit(Function node)
    {
        Visit(node.Name);
        Visit(node.Parameters);
        Visit(node.Body);
    }

    public virtual void Visit(Break node)
    {
    }

    public virtual void Visit(Do node)
    {
        Visit(node.Body);
    }

    public virtual void Visit(Repeat node)
    {
        Visit(node.Body);
        Visit(node.Condition);
    }

    public virtual void Visit(Return node)
    {
        VisitList(node.Values);
    }

    public virtual void Visit(ParameterList node)
    {
        VisitList(node.Names);
    }

    public virtual void Visit(StringLiteral node)
    {
    }

    public virtual void Visit(NumberLiteral node)
    {
    }

    public virtual void Visit(BooleanLiteral node)
    {
    }

    public virtual void Visit(Nil node)
    {
    }

    public virtual void Visit(Block node)
    {
        VisitList(node.Statements);
    }

    public virtual void Visit(Goto node)
    {
        Visit(node.Name);
    }

    public virtual void Visit(Label node)
    {
        Visit(node.Name);
    }

    public void Visit(Node node)
    {
        node.Accept(this);
    }
}