using System.Globalization;
using MoonsecDeobfuscator.Ast;
using MoonsecDeobfuscator.Ast.Expressions;
using MoonsecDeobfuscator.Ast.Literals;
using MoonsecDeobfuscator.Ast.Statements;
using MoonsecDeobfuscator.Syntax.Parser;

namespace MoonsecDeobfuscator.Syntax;

public class CstToAstVisitor : LuaBaseVisitor<Node>
{
    public override Block VisitBlock(LuaParser.BlockContext ctx)
    {
        var statContext = ctx.stat().Where(stat => stat is not LuaParser.StatSemiContext).ToList();
        var stats = new NodeList<Statement>(statContext.Count);

        foreach (var stat in statContext)
        {
            var node = Visit(stat);

            if (node is Expression expression)
                node = new ExpressionStatement(expression);

            stats.Add((Statement) node);
        }

        if (ctx.retstat() != null)
            stats.Add(VisitRetstat(ctx.retstat()));

        return new Block(stats);
    }

    public override Assign VisitStatAssign(LuaParser.StatAssignContext ctx) =>
        new(GetVarList(ctx.varlist()), GetExpList(ctx.explist()));

    public override Call VisitStatCall(LuaParser.StatCallContext ctx) => VisitFunctioncall(ctx.functioncall());

    public override Break VisitStatBreak(LuaParser.StatBreakContext ctx) => new();

    public override Do VisitStatDo(LuaParser.StatDoContext ctx) => new(VisitBlock(ctx.block()));

    public override While VisitStatWhile(LuaParser.StatWhileContext ctx) =>
        new((Expression) Visit(ctx.exp()), VisitBlock(ctx.block()));

    public override Repeat VisitStatRepeat(LuaParser.StatRepeatContext ctx) =>
        new(VisitBlock(ctx.block()), (Expression) Visit(ctx.exp()));

    public override If VisitStatIf(LuaParser.StatIfContext ctx)
    {
        var ifCtx = ctx.ifstmt();
        var elseIfCtx = ctx.elseifstmt();
        var elseCtx = ctx.elsestmt();

        var ifClause = new If.Clause((Expression) Visit(ifCtx.exp()), VisitBlock(ifCtx.block()));
        var elseIfClause = elseIfCtx.exp()
            .Zip(elseIfCtx.block())
            .Select(it => new If.Clause((Expression) Visit(it.First), VisitBlock(it.Second)));
        var elseBody = elseCtx.block() != null
            ? VisitBlock(elseCtx.block())
            : null;

        return new If(ifClause, new NodeList<If.Clause>(elseIfClause), elseBody);
    }

    public override NumericFor VisitStatNumericFor(LuaParser.StatNumericForContext ctx)
    {
        var expCtx = ctx.exp();

        return new NumericFor(
            new Name(ctx.NAME().GetText()),
            (Expression) Visit(expCtx[0]),
            (Expression) Visit(expCtx[1]),
            expCtx.Length > 2 ? (Expression) Visit(expCtx[2]) : null,
            VisitBlock(ctx.block())
        );
    }

    public override GenericFor VisitStatGenericFor(LuaParser.StatGenericForContext ctx) =>
        new(GetNameList(ctx.namelist()), GetExpList(ctx.explist()), VisitBlock(ctx.block()));

    public override Function VisitStatFunction(LuaParser.StatFunctionContext ctx)
    {
        var funcBodyCtx = ctx.funcbody();

        return new Function(
            new Name(ctx.funcname().GetText()),
            GetParList(funcBodyCtx.parlist()),
            VisitBlock(funcBodyCtx.block())
        );
    }

    public override LocalFunction VisitStatLocalFunction(LuaParser.StatLocalFunctionContext ctx)
    {
        var funcBodyCtx = ctx.funcbody();

        return new LocalFunction(
            new Name(ctx.NAME().GetText()),
            GetParList(funcBodyCtx.parlist()),
            VisitBlock(funcBodyCtx.block())
        );
    }

    public override LocalDeclare VisitStatLocalDeclare(LuaParser.StatLocalDeclareContext ctx) =>
        new(GetNameList(ctx.namelist()), GetExpList(ctx.explist()));

    public override Goto VisitStatGoto(LuaParser.StatGotoContext ctx) => new(new Name(ctx.NAME().GetText()));

    public override Label VisitStatLabel(LuaParser.StatLabelContext ctx) => new(new Name(ctx.label().NAME().GetText()));

    public override Return VisitRetstat(LuaParser.RetstatContext ctx) => new(GetExpList(ctx.explist()));

    public override Nil VisitExpNil(LuaParser.ExpNilContext ctx) => new();

    public override BooleanLiteral VisitExpFalse(LuaParser.ExpFalseContext ctx) => new(false);

    public override BooleanLiteral VisitExpTrue(LuaParser.ExpTrueContext ctx) => new(true);

    public override NumberLiteral VisitExpNumber(LuaParser.ExpNumberContext ctx)
    {
        var numberCtx = ctx.number();

        if (numberCtx.INT() != null || numberCtx.FLOAT() != null)
            return new NumberLiteral(double.Parse(numberCtx.GetText()));

        return new NumberLiteral(int.Parse(numberCtx.GetText().Replace("0x", ""), NumberStyles.HexNumber));
    }

    public override StringLiteral VisitExpString(LuaParser.ExpStringContext ctx) => new(ctx.@string().GetText());

    public override VarArg VisitExpVarArg(LuaParser.ExpVarArgContext ctx) => new();

    public override AnonymousFunction VisitExpFunction(LuaParser.ExpFunctionContext ctx)
    {
        var funcBodyCtx = ctx.functiondef().funcbody();
        return new AnonymousFunction(GetParList(funcBodyCtx.parlist()), VisitBlock(funcBodyCtx.block()));
    }

    public override Expression VisitPrefixexp(LuaParser.PrefixexpContext ctx)
    {
        var nameAndArgsCtx = ctx.nameAndArgs();
        var varOrExp = VisitVarOrExp(ctx.varOrExp());

        return nameAndArgsCtx.Length == 0 ? varOrExp : GetCallChain(varOrExp, nameAndArgsCtx);
    }

    public override Table VisitExpTable(LuaParser.ExpTableContext ctx) => GetTable(ctx.tableconstructor());

    public override BinaryExpression VisitExpPow(LuaParser.ExpPowContext ctx) =>
        new(BinaryOperator.Pow, (Expression) Visit(ctx.exp(0)), (Expression) Visit(ctx.exp(1)));

    public override UnaryExpression VisitExpUnary(LuaParser.ExpUnaryContext ctx)
    {
        var @operator = ctx.operatorUnary().GetText() switch
        {
            "not" => UnaryOperator.Not,
            "#" => UnaryOperator.Length,
            "-" => UnaryOperator.Negate,
            _ => throw new Exception($"Invalid operator: {ctx.GetText()}")
        };

        return new UnaryExpression(@operator, (Expression) Visit(ctx.exp()));
    }

    public override BinaryExpression VisitExpMulDivMod(LuaParser.ExpMulDivModContext ctx)
    {
        var @operator = ctx.operatorMulDivMod().GetText() switch
        {
            "*" => BinaryOperator.Mul,
            "/" => BinaryOperator.Div,
            "%" => BinaryOperator.Mod,
            _ => throw new Exception($"Invalid operator: {ctx.GetText()}")
        };

        return new BinaryExpression(@operator, (Expression) Visit(ctx.exp(0)), (Expression) Visit(ctx.exp(1)));
    }

    public override BinaryExpression VisitExpAddSub(LuaParser.ExpAddSubContext ctx)
    {
        var @operator = ctx.operatorAddSub().GetText() == "+" ? BinaryOperator.Add : BinaryOperator.Sub;
        return new BinaryExpression(@operator, (Expression) Visit(ctx.exp(0)), (Expression) Visit(ctx.exp(1)));
    }

    public override BinaryExpression VisitExpConcat(LuaParser.ExpConcatContext ctx) =>
        new(BinaryOperator.Concat, (Expression) Visit(ctx.exp(0)), (Expression) Visit(ctx.exp(1)));

    public override BinaryExpression VisitExpCompare(LuaParser.ExpCompareContext ctx)
    {
        var @operator = ctx.operatorComparison().GetText() switch
        {
            "==" => BinaryOperator.Equals,
            "~=" => BinaryOperator.NotEquals,
            "<=" => BinaryOperator.LessThanOrEquals,
            ">=" => BinaryOperator.GreaterThanOrEquals,
            "<" => BinaryOperator.LessThan,
            ">" => BinaryOperator.GreaterThan,
            _ => throw new Exception($"Invalid operator: {ctx.GetText()}")
        };

        return new BinaryExpression(@operator, (Expression) Visit(ctx.exp(0)), (Expression) Visit(ctx.exp(1)));
    }

    public override BinaryExpression VisitExpAnd(LuaParser.ExpAndContext ctx) =>
        new(BinaryOperator.And, (Expression) Visit(ctx.exp(0)), (Expression) Visit(ctx.exp(1)));

    public override BinaryExpression VisitExpOr(LuaParser.ExpOrContext ctx) =>
        new(BinaryOperator.Or, (Expression) Visit(ctx.exp(0)), (Expression) Visit(ctx.exp(1)));

    public override Call VisitFunctioncall(LuaParser.FunctioncallContext ctx) =>
        GetCallChain(VisitVarOrExp(ctx.varOrExp()), ctx.nameAndArgs());

    public override Expression VisitVarOrExp(LuaParser.VarOrExpContext ctx) =>
        ctx.exp() != null ? (Expression) Visit(ctx.exp()) : VisitVar(ctx.var());

    public override Variable VisitVar(LuaParser.VarContext ctx)
    {
        var varSuffixCtx = ctx.varSuffix();
        var name = ctx.NAME() != null ? new Name(ctx.NAME().GetText()) : (Expression) Visit(ctx.exp());

        if (varSuffixCtx.Length == 0)
            return (Variable) name;

        return (Variable) varSuffixCtx.Aggregate(name, (expression, suffix) => suffix.NAME() != null
            ? new MemberAccess(expression, new Name(suffix.NAME().GetText()))
            : new ElementAccess(expression, (Expression) Visit(suffix.exp()))
        );
    }

    private NodeList<Expression> GetExpList(LuaParser.ExplistContext? ctx)
    {
        if (ctx == null)
            return [];

        var exprs = ctx.exp();
        var list = new NodeList<Expression>(exprs.Length);

        foreach (var expression in exprs)
            list.Add((Expression) Visit(expression));

        return list;
    }

    private static NodeList<Name> GetNameList(LuaParser.NamelistContext context)
    {
        var names = context.NAME();
        var list = new NodeList<Name>(names.Length);

        foreach (var name in names)
            list.Add(new Name(name.GetText()));

        return list;
    }

    private NodeList<Variable> GetVarList(LuaParser.VarlistContext context)
    {
        var vars = context.var();
        var list = new NodeList<Variable>(vars.Length);

        foreach (var variable in vars)
            list.Add(VisitVar(variable));

        return list;
    }

    private NodeList<Expression> GetArgs(LuaParser.ArgsContext ctx)
    {
        if (ctx.explist() != null)
            return GetExpList(ctx.explist());

        var args = new NodeList<Expression>();

        if (ctx.tableconstructor() != null)
            args.Add(GetTable(ctx.tableconstructor()));
        else if (ctx.@string() != null)
            args.Add(new StringLiteral(ctx.@string().GetText()));

        return args;
    }

    private Call GetCallChain(Expression varOrExp, LuaParser.NameAndArgsContext[] nameAndArgsCtx)
    {
        return (Call) nameAndArgsCtx.Aggregate(varOrExp, (expression, nameAndArgs) =>
        {
            var args = GetArgs(nameAndArgs.args());

            return nameAndArgs.NAME() != null
                ? new MethodCall(expression, new Name(nameAndArgs.NAME().GetText()), args)
                : new Call(expression, args);
        });
    }

    private Table.Entry GetField(LuaParser.FieldContext ctx)
    {
        var expCtx = ctx.exp();

        if (expCtx.Length == 2)
        {
            var key = (Expression) Visit(expCtx[0]);
            var value = (Expression) Visit(expCtx[1]);

            return new Table.Entry(key, value);
        }

        if (ctx.NAME() != null)
        {
            var key = new Name(ctx.NAME().GetText());
            var value = (Expression) Visit(expCtx[0]);

            return new Table.Entry(key, value);
        }

        return new Table.Entry((Expression) Visit(expCtx[0]));
    }

    private Table GetTable(LuaParser.TableconstructorContext ctx)
    {
        var entries = new NodeList<Table.Entry>();
        var fieldListCtx = ctx.fieldlist();

        if (fieldListCtx != null)
        {
            foreach (var fieldCtx in fieldListCtx.field())
                entries.Add(GetField(fieldCtx));
        }

        return new Table(entries);
    }

    private static ParameterList GetParList(LuaParser.ParlistContext? ctx)
    {
        if (ctx == null)
            return new ParameterList();

        var names = ctx.namelist() != null ? GetNameList(ctx.namelist()) : [];
        var isVarArg = ctx.children.Any(child => child.GetText() == "...");

        return new ParameterList(names, isVarArg);
    }
}