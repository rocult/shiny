using MoonsecDeobfuscator.Ast.Expressions;
using MoonsecDeobfuscator.Ast.Literals;
using MoonsecDeobfuscator.Ast.Statements;
using MoonsecDeobfuscator.Syntax.Semantic;

namespace MoonsecDeobfuscator.Ast;

// self -> children
public class PreOrderRewriteDispatch(AstRewriter rewriter) : AstRewriter, IRewriteDispatch
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
        {
            _changed = true;
            return newNode;
        }

        return node;
    }

    public override Node Visit(ElementAccess node)
    {
        var newNode = rewriter.Visit(node);

        if (newNode != node)
        {
            _changed = true;
            return newNode;
        }

        node.Table = (Expression) Visit(node.Table);
        node.Key = (Expression) Visit(node.Key);

        return node;
    }

    public override Node Visit(MemberAccess node)
    {
        var newNode = rewriter.Visit(node);

        if (newNode != node)
        {
            _changed = true;
            return newNode;
        }

        node.Table = (Expression) Visit(node.Table);
        node.Key = (Name) Visit(node.Key);

        return node;
    }

    public override Node Visit(BinaryExpression node)
    {
        var newNode = rewriter.Visit(node);

        if (newNode != node)
        {
            _changed = true;
            return newNode;
        }

        node.Left = (Expression) Visit(node.Left);
        node.Right = (Expression) Visit(node.Right);

        return node;
    }

    public override Node Visit(UnaryExpression node)
    {
        var newNode = rewriter.Visit(node);

        if (newNode != node)
        {
            _changed = true;
            return newNode;
        }

        node.Operand = (Expression) Visit(node.Operand);

        return node;
    }

    public override Node Visit(Call node)
    {
        var newNode = rewriter.Visit(node);

        if (newNode != node)
        {
            _changed = true;
            return newNode;
        }

        node.Function = (Expression) Visit(node.Function);
        VisitList(node.Arguments);

        return node;
    }

    public override Node Visit(MethodCall node)
    {
        var newNode = rewriter.Visit(node);

        if (newNode != node)
        {
            _changed = true;
            return newNode;
        }

        node.Function = (Expression) Visit(node.Function);
        node.MethodName = (Name) Visit(node.MethodName);
        VisitList(node.Arguments);

        return node;
    }

    public override Node Visit(Table node)
    {
        var newNode = rewriter.Visit(node);

        if (newNode != node)
        {
            _changed = true;
            return newNode;
        }

        VisitList(node.Entries);

        return node;
    }

    public override Node Visit(Table.Entry node)
    {
        var newNode = rewriter.Visit(node);

        if (newNode != node)
        {
            _changed = true;
            return newNode;
        }

        var key = node.Key;

        if (key != null)
            node.Key = (Expression) Visit(key);

        node.Value = (Expression) Visit(node.Value);

        return node;
    }

    public override Node Visit(AnonymousFunction node)
    {
        var newNode = rewriter.Visit(node);

        if (newNode != node)
        {
            _changed = true;
            return newNode;
        }

        node.Parameters = (ParameterList) Visit(node.Parameters);
        node.Body = (Block) Visit(node.Body);

        return node;
    }

    public override Node Visit(VarArg node)
    {
        var newNode = rewriter.Visit(node);

        if (newNode != node)
        {
            _changed = true;
            return newNode;
        }

        return node;
    }

    public override Node Visit(ExpressionStatement node)
    {
        var newNode = rewriter.Visit(node);

        if (newNode != node)
        {
            _changed = true;
            return newNode;
        }

        node.Expression = (Expression) Visit(node.Expression);

        return node;
    }

    public override Node Visit(While node)
    {
        var newNode = rewriter.Visit(node);

        if (newNode != node)
        {
            _changed = true;
            return newNode;
        }

        node.Condition = (Expression) Visit(node.Condition);
        node.Body = (Block) Visit(node.Body);

        return node;
    }

    public override Node Visit(LocalDeclare node)
    {
        var newNode = rewriter.Visit(node);

        if (newNode != node)
        {
            _changed = true;
            return newNode;
        }

        VisitList(node.Names);
        VisitList(node.Values);

        return node;
    }

    public override Node Visit(Assign node)
    {
        var newNode = rewriter.Visit(node);

        if (newNode != node)
        {
            _changed = true;
            return newNode;
        }

        VisitList(node.Variables);
        VisitList(node.Values);

        return node;
    }

    public override Node Visit(GenericFor node)
    {
        var newNode = rewriter.Visit(node);

        if (newNode != node)
        {
            _changed = true;
            return newNode;
        }

        VisitList(node.Names);
        VisitList(node.Expressions);
        node.Body = (Block) Visit(node.Body);

        return node;
    }

    public override Node Visit(NumericFor node)
    {
        var newNode = rewriter.Visit(node);

        if (newNode != node)
        {
            _changed = true;
            return newNode;
        }

        node.IteratorName = (Name) Visit(node.IteratorName);
        node.InitialValue = (Expression) Visit(node.InitialValue);
        node.FinalValue = (Expression) Visit(node.FinalValue);

        var step = node.Step;

        if (step != null)
            node.Step = (Expression) Visit(step);

        node.Body = (Block) Visit(node.Body);

        return node;
    }

    public override Node Visit(If node)
    {
        var newNode = rewriter.Visit(node);

        if (newNode != node)
        {
            _changed = true;
            return newNode;
        }

        node.IfClause = (If.Clause) Visit(node.IfClause);
        VisitList(node.ElseIfClauses);

        var elseBody = node.ElseBody;

        if (elseBody != null)
            node.ElseBody = (Block) Visit(elseBody);

        return node;
    }

    public override Node Visit(If.Clause node)
    {
        var newNode = rewriter.Visit(node);

        if (newNode != node)
        {
            _changed = true;
            return newNode;
        }

        node.Condition = (Expression) Visit(node.Condition);
        node.Body = (Block) Visit(node.Body);

        return node;
    }

    public override Node Visit(LocalFunction node)
    {
        var newNode = rewriter.Visit(node);

        if (newNode != node)
        {
            _changed = true;
            return newNode;
        }

        node.Name = (Name) Visit(node.Name);
        node.Parameters = (ParameterList) Visit(node.Parameters);
        node.Body = (Block) Visit(node.Body);

        return node;
    }

    public override Node Visit(Function node)
    {
        var newNode = rewriter.Visit(node);

        if (newNode != node)
        {
            _changed = true;
            return newNode;
        }

        node.Name = (Name) Visit(node.Name);
        node.Parameters = (ParameterList) Visit(node.Parameters);
        node.Body = (Block) Visit(node.Body);

        return node;
    }

    public override Node Visit(Break node)
    {
        var newNode = rewriter.Visit(node);

        if (newNode != node)
        {
            _changed = true;
            return newNode;
        }

        return node;
    }

    public override Node Visit(Do node)
    {
        var newNode = rewriter.Visit(node);

        if (newNode != node)
        {
            _changed = true;
            return newNode;
        }

        node.Body = (Block) Visit(node.Body);

        return node;
    }

    public override Node Visit(Repeat node)
    {
        var newNode = rewriter.Visit(node);

        if (newNode != node)
        {
            _changed = true;
            return newNode;
        }

        node.Body = (Block) Visit(node.Body);
        node.Condition = (Expression) Visit(node.Condition);

        return node;
    }

    public override Node Visit(Return node)
    {
        var newNode = rewriter.Visit(node);

        if (newNode != node)
        {
            _changed = true;
            return newNode;
        }

        VisitList(node.Values);

        return node;
    }

    public override Node Visit(ParameterList node)
    {
        var newNode = rewriter.Visit(node);

        if (newNode != node)
        {
            _changed = true;
            return newNode;
        }

        VisitList(node.Names);

        return node;
    }

    public override Node Visit(StringLiteral node)
    {
        var newNode = rewriter.Visit(node);

        if (newNode != node)
        {
            _changed = true;
            return newNode;
        }

        return node;
    }

    public override Node Visit(NumberLiteral node)
    {
        var newNode = rewriter.Visit(node);

        if (newNode != node)
        {
            _changed = true;
            return newNode;
        }

        return node;
    }

    public override Node Visit(BooleanLiteral node)
    {
        var newNode = rewriter.Visit(node);

        if (newNode != node)
        {
            _changed = true;
            return newNode;
        }

        return node;
    }

    public override Node Visit(Nil node)
    {
        var newNode = rewriter.Visit(node);

        if (newNode != node)
        {
            _changed = true;
            return newNode;
        }

        return node;
    }

    public override Node Visit(Block node)
    {
        var newNode = rewriter.Visit(node);

        if (newNode != node)
        {
            _changed = true;
            return newNode;
        }

        VisitList(node.Statements);

        return node;
    }

    public override Node Visit(Goto node)
    {
        var newNode = rewriter.Visit(node);

        if (newNode != node)
        {
            _changed = true;
            return newNode;
        }

        node.Name = (Name) Visit(node.Name);

        return node;
    }

    public override Node Visit(Label node)
    {
        var newNode = rewriter.Visit(node);

        if (newNode != node)
        {
            _changed = true;
            return newNode;
        }

        node.Name = (Name) Visit(node.Name);

        return node;
    }
}