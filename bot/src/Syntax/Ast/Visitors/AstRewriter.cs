using MoonsecDeobfuscator.Ast.Expressions;
using MoonsecDeobfuscator.Ast.Literals;
using MoonsecDeobfuscator.Ast.Statements;
using MoonsecDeobfuscator.Syntax.Semantic;

namespace MoonsecDeobfuscator.Ast;

public abstract class AstRewriter
{
    public SymbolTable? SymbolTable;

    public void Rewrite(Node node, Order order, bool symbols = false, bool fixedPoint = false)
    {
        IRewriteDispatch disp = order == Order.PostOrder
            ? new PostOrderRewriteDispatch(this)
            : new PreOrderRewriteDispatch(this);

        disp.Rewrite(node, symbols, fixedPoint);
    }

    protected VariableInfo? GetVariable(string name) => SymbolTable?.GetVariable(name);

    public virtual Node Visit(Name node) => node;

    public virtual Node Visit(ElementAccess node) => node;

    public virtual Node Visit(MemberAccess node) => node;

    public virtual Node Visit(BinaryExpression node) => node;

    public virtual Node Visit(UnaryExpression node) => node;

    public virtual Node Visit(Call node) => node;

    public virtual Node Visit(MethodCall node) => node;

    public virtual Node Visit(Table node) => node;

    public virtual Node Visit(Table.Entry node) => node;

    public virtual Node Visit(AnonymousFunction node) => node;

    public virtual Node Visit(VarArg node) => node;

    public virtual Node Visit(ExpressionStatement node) => node;

    public virtual Node Visit(While node) => node;

    public virtual Node Visit(LocalDeclare node) => node;

    public virtual Node Visit(Assign node) => node;

    public virtual Node Visit(GenericFor node) => node;

    public virtual Node Visit(NumericFor node) => node;

    public virtual Node Visit(If node) => node;

    public virtual Node Visit(If.Clause node) => node;

    public virtual Node Visit(LocalFunction node) => node;

    public virtual Node Visit(Function node) => node;

    public virtual Node Visit(Break node) => node;

    public virtual Node Visit(Do node) => node;

    public virtual Node Visit(Repeat node) => node;

    public virtual Node Visit(Return node) => node;

    public virtual Node Visit(ParameterList node) => node;

    public virtual Node Visit(StringLiteral node) => node;

    public virtual Node Visit(NumberLiteral node) => node;

    public virtual Node Visit(BooleanLiteral node) => node;

    public virtual Node Visit(Nil node) => node;

    public virtual Node Visit(Block node) => node;

    public virtual Node Visit(Goto node) => node;

    public virtual Node Visit(Label node) => node;

    public Node Visit(Node node) => node.Accept(this);
}