using MoonsecDeobfuscator.Ast.Expressions;
using MoonsecDeobfuscator.Ast.Literals;
using MoonsecDeobfuscator.Ast.Statements;
using MoonsecDeobfuscator.Syntax.Semantic;

namespace MoonsecDeobfuscator.Ast;

// children -> self
public class PostOrderRewriteDispatch(AstRewriter rewriter) : AstRewriter, IRewriteDispatch
{
    private bool _changed;

    public void Rewrite(Node node, bool symbols, bool fixedPoint)
    {
        do
        {
            if (symbols)
                rewriter.SymbolTable = SymbolTableBuilder.Build(node);

            _changed = false;
            node.Accept(this);
        } 
        while (_changed && fixedPoint);
    }

    private void VisitList<TNode>(NodeList<TNode> list) where TNode : Node
    {
        for (var i = 0; i < list.Count; i++)
        {
            var oldNode = list[i];
            var newNode = Visit(oldNode);

            if (oldNode != newNode)
            {
                _changed = true;
                list[i] = (TNode) newNode;
            }
        }
    }

    public override Node Visit(Name node)
    {
        var newNode = rewriter.Visit(node);

        if (newNode != node)
            _changed = true;

        return newNode;
    }

    public override Node Visit(ElementAccess node)
    {
        node.Table = (Expression) Visit(node.Table);
        node.Key = (Expression) Visit(node.Key);

        var newNode = rewriter.Visit(node);

        if (newNode != node)
            _changed = true;

        return newNode;
    }

    public override Node Visit(MemberAccess node)
    {
        node.Table = (Expression) Visit(node.Table);
        node.Key = (Name) Visit(node.Key);

        var newNode = rewriter.Visit(node);

        if (newNode != node)
            _changed = true;

        return newNode;
    }

    public override Node Visit(BinaryExpression node)
    {
        node.Left = (Expression) Visit(node.Left);
        node.Right = (Expression) Visit(node.Right);

        var newNode = rewriter.Visit(node);

        if (newNode != node)
            _changed = true;

        return newNode;
    }

    public override Node Visit(UnaryExpression node)
    {
        node.Operand = (Expression) Visit(node.Operand);

        var newNode = rewriter.Visit(node);

        if (newNode != node)
            _changed = true;

        return newNode;
    }

    public override Node Visit(Call node)
    {
        node.Function = (Expression) Visit(node.Function);
        VisitList(node.Arguments);

        var newNode = rewriter.Visit(node);

        if (newNode != node)
            _changed = true;

        return newNode;
    }

    public override Node Visit(MethodCall node)
    {
        node.Function = (Expression) Visit(node.Function);
        node.MethodName = (Name) Visit(node.MethodName);
        VisitList(node.Arguments);

        var newNode = rewriter.Visit(node);

        if (newNode != node)
            _changed = true;

        return newNode;
    }

    public override Node Visit(Table node)
    {
        VisitList(node.Entries);

        var newNode = rewriter.Visit(node);

        if (newNode != node)
            _changed = true;

        return newNode;
    }

    public override Node Visit(Table.Entry node)
    {
        var key = node.Key;

        if (key != null)
            node.Key = (Expression) Visit(key);

        node.Value = (Expression) Visit(node.Value);

        var newNode = rewriter.Visit(node);

        if (newNode != node)
            _changed = true;

        return newNode;
    }

    public override Node Visit(AnonymousFunction node)
    {
        node.Parameters = (ParameterList) Visit(node.Parameters);
        node.Body = (Block) Visit(node.Body);

        var newNode = rewriter.Visit(node);

        if (newNode != node)
            _changed = true;

        return newNode;
    }

    public override Node Visit(VarArg node)
    {
        var newNode = rewriter.Visit(node);

        if (newNode != node)
            _changed = true;

        return newNode;
    }

    public override Node Visit(ExpressionStatement node)
    {
        node.Expression = (Expression) Visit(node.Expression);

        var newNode = rewriter.Visit(node);

        if (newNode != node)
            _changed = true;

        return newNode;
    }

    public override Node Visit(While node)
    {
        node.Condition = (Expression) Visit(node.Condition);
        node.Body = (Block) Visit(node.Body);

        var newNode = rewriter.Visit(node);

        if (newNode != node)
            _changed = true;

        return newNode;
    }

    public override Node Visit(LocalDeclare node)
    {
        VisitList(node.Names);
        VisitList(node.Values);

        var newNode = rewriter.Visit(node);

        if (newNode != node)
            _changed = true;

        return newNode;
    }

    public override Node Visit(Assign node)
    {
        VisitList(node.Variables);
        VisitList(node.Values);

        var newNode = rewriter.Visit(node);

        if (newNode != node)
            _changed = true;

        return newNode;
    }

    public override Node Visit(GenericFor node)
    {
        VisitList(node.Names);
        VisitList(node.Expressions);
        node.Body = (Block) Visit(node.Body);

        var newNode = rewriter.Visit(node);

        if (newNode != node)
            _changed = true;

        return newNode;
    }

    public override Node Visit(NumericFor node)
    {
        node.IteratorName = (Name) Visit(node.IteratorName);
        node.InitialValue = (Expression) Visit(node.InitialValue);
        node.FinalValue = (Expression) Visit(node.FinalValue);

        var step = node.Step;

        if (step != null)
            node.Step = (Expression) Visit(step);

        node.Body = (Block) Visit(node.Body);

        var newNode = rewriter.Visit(node);

        if (newNode != node)
            _changed = true;

        return newNode;
    }

    public override Node Visit(If node)
    {
        node.IfClause = (If.Clause) Visit(node.IfClause);
        VisitList(node.ElseIfClauses);

        var elseBody = node.ElseBody;

        if (elseBody != null)
            node.ElseBody = (Block) Visit(elseBody);

        var newNode = rewriter.Visit(node);

        if (newNode != node)
            _changed = true;

        return newNode;
    }

    public override Node Visit(If.Clause node)
    {
        node.Condition = (Expression) Visit(node.Condition);
        node.Body = (Block) Visit(node.Body);

        var newNode = rewriter.Visit(node);

        if (newNode != node)
            _changed = true;

        return newNode;
    }

    public override Node Visit(LocalFunction node)
    {
        node.Name = (Name) Visit(node.Name);
        node.Parameters = (ParameterList) Visit(node.Parameters);
        node.Body = (Block) Visit(node.Body);

        var newNode = rewriter.Visit(node);

        if (newNode != node)
            _changed = true;

        return newNode;
    }

    public override Node Visit(Function node)
    {
        node.Name = (Name) Visit(node.Name);
        node.Parameters = (ParameterList) Visit(node.Parameters);
        node.Body = (Block) Visit(node.Body);

        var newNode = rewriter.Visit(node);

        if (newNode != node)
            _changed = true;

        return newNode;
    }

    public override Node Visit(Break node)
    {
        var newNode = rewriter.Visit(node);

        if (newNode != node)
            _changed = true;

        return newNode;
    }

    public override Node Visit(Do node)
    {
        node.Body = (Block) Visit(node.Body);

        var newNode = rewriter.Visit(node);

        if (newNode != node)
            _changed = true;

        return newNode;
    }

    public override Node Visit(Repeat node)
    {
        node.Body = (Block) Visit(node.Body);
        node.Condition = (Expression) Visit(node.Condition);

        var newNode = rewriter.Visit(node);

        if (newNode != node)
            _changed = true;

        return newNode;
    }

    public override Node Visit(Return node)
    {
        VisitList(node.Values);

        var newNode = rewriter.Visit(node);

        if (newNode != node)
            _changed = true;

        return newNode;
    }

    public override Node Visit(ParameterList node)
    {
        VisitList(node.Names);

        var newNode = rewriter.Visit(node);

        if (newNode != node)
            _changed = true;

        return newNode;
    }

    public override Node Visit(StringLiteral node)
    {
        var newNode = rewriter.Visit(node);

        if (newNode != node)
            _changed = true;

        return newNode;
    }

    public override Node Visit(NumberLiteral node)
    {
        var newNode = rewriter.Visit(node);

        if (newNode != node)
            _changed = true;

        return newNode;
    }

    public override Node Visit(BooleanLiteral node)
    {
        var newNode = rewriter.Visit(node);

        if (newNode != node)
            _changed = true;

        return newNode;
    }

    public override Node Visit(Nil node)
    {
        var newNode = rewriter.Visit(node);

        if (newNode != node)
            _changed = true;

        return newNode;
    }

    public override Node Visit(Block node)
    {
        VisitList(node.Statements);

        var newNode = rewriter.Visit(node);

        if (newNode != node)
            _changed = true;

        return newNode;
    }

    public override Node Visit(Goto node)
    {
        node.Name = (Name) Visit(node.Name);

        var newNode = rewriter.Visit(node);

        if (newNode != node)
            _changed = true;

        return newNode;
    }

    public override Node Visit(Label node)
    {
        node.Name = (Name) Visit(node.Name);

        var newNode = rewriter.Visit(node);

        if (newNode != node)
            _changed = true;

        return newNode;
    }
}