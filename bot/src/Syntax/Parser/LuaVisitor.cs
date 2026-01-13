// Unreachable code detected
#pragma warning disable 0162
// The variable '...' is assigned but its value is never used
#pragma warning disable 0219
// Missing XML comment for publicly visible type or member '...'
#pragma warning disable 1591
// Ambiguous reference in cref attribute
#pragma warning disable 419

using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;

namespace MoonsecDeobfuscator.Syntax.Parser;

/// <summary>
/// This interface defines a complete generic visitor for a parse tree produced
/// by <see cref="LuaParser"/>.
/// </summary>
/// <typeparam name="Result">The return type of the visit operation.</typeparam>
[System.CodeDom.Compiler.GeneratedCode("ANTLR", "4.13.2")]
[System.CLSCompliant(false)]
public interface ILuaVisitor<Result> : IParseTreeVisitor<Result> {
	/// <summary>
	/// Visit a parse tree produced by <see cref="LuaParser.chunk"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitChunk([NotNull] LuaParser.ChunkContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="LuaParser.block"/>.
	/// </summary>
	/// <param name="ctx">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitBlock([NotNull] LuaParser.BlockContext ctx);
	/// <summary>
	/// Visit a parse tree produced by the <c>statSemi</c>
	/// labeled alternative in <see cref="LuaParser.stat"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitStatSemi([NotNull] LuaParser.StatSemiContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>statAssign</c>
	/// labeled alternative in <see cref="LuaParser.stat"/>.
	/// </summary>
	/// <param name="ctx">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitStatAssign([NotNull] LuaParser.StatAssignContext ctx);
	/// <summary>
	/// Visit a parse tree produced by the <c>statCall</c>
	/// labeled alternative in <see cref="LuaParser.stat"/>.
	/// </summary>
	/// <param name="ctx">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitStatCall([NotNull] LuaParser.StatCallContext ctx);
	/// <summary>
	/// Visit a parse tree produced by the <c>statLabel</c>
	/// labeled alternative in <see cref="LuaParser.stat"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitStatLabel([NotNull] LuaParser.StatLabelContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>statBreak</c>
	/// labeled alternative in <see cref="LuaParser.stat"/>.
	/// </summary>
	/// <param name="ctx">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitStatBreak([NotNull] LuaParser.StatBreakContext ctx);
	/// <summary>
	/// Visit a parse tree produced by the <c>statGoto</c>
	/// labeled alternative in <see cref="LuaParser.stat"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitStatGoto([NotNull] LuaParser.StatGotoContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>statDo</c>
	/// labeled alternative in <see cref="LuaParser.stat"/>.
	/// </summary>
	/// <param name="ctx">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitStatDo([NotNull] LuaParser.StatDoContext ctx);
	/// <summary>
	/// Visit a parse tree produced by the <c>statWhile</c>
	/// labeled alternative in <see cref="LuaParser.stat"/>.
	/// </summary>
	/// <param name="ctx">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitStatWhile([NotNull] LuaParser.StatWhileContext ctx);
	/// <summary>
	/// Visit a parse tree produced by the <c>statRepeat</c>
	/// labeled alternative in <see cref="LuaParser.stat"/>.
	/// </summary>
	/// <param name="ctx">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitStatRepeat([NotNull] LuaParser.StatRepeatContext ctx);
	/// <summary>
	/// Visit a parse tree produced by the <c>statIf</c>
	/// labeled alternative in <see cref="LuaParser.stat"/>.
	/// </summary>
	/// <param name="ctx">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitStatIf([NotNull] LuaParser.StatIfContext ctx);
	/// <summary>
	/// Visit a parse tree produced by the <c>statNumericFor</c>
	/// labeled alternative in <see cref="LuaParser.stat"/>.
	/// </summary>
	/// <param name="ctx">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitStatNumericFor([NotNull] LuaParser.StatNumericForContext ctx);
	/// <summary>
	/// Visit a parse tree produced by the <c>statGenericFor</c>
	/// labeled alternative in <see cref="LuaParser.stat"/>.
	/// </summary>
	/// <param name="ctx">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitStatGenericFor([NotNull] LuaParser.StatGenericForContext ctx);
	/// <summary>
	/// Visit a parse tree produced by the <c>statFunction</c>
	/// labeled alternative in <see cref="LuaParser.stat"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitStatFunction([NotNull] LuaParser.StatFunctionContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>statLocalFunction</c>
	/// labeled alternative in <see cref="LuaParser.stat"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitStatLocalFunction([NotNull] LuaParser.StatLocalFunctionContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>statLocalDeclare</c>
	/// labeled alternative in <see cref="LuaParser.stat"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitStatLocalDeclare([NotNull] LuaParser.StatLocalDeclareContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="LuaParser.ifstmt"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitIfstmt([NotNull] LuaParser.IfstmtContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="LuaParser.elseifstmt"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitElseifstmt([NotNull] LuaParser.ElseifstmtContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="LuaParser.elsestmt"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitElsestmt([NotNull] LuaParser.ElsestmtContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="LuaParser.retstat"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitRetstat([NotNull] LuaParser.RetstatContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="LuaParser.label"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitLabel([NotNull] LuaParser.LabelContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="LuaParser.funcname"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitFuncname([NotNull] LuaParser.FuncnameContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="LuaParser.varlist"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitVarlist([NotNull] LuaParser.VarlistContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="LuaParser.namelist"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitNamelist([NotNull] LuaParser.NamelistContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="LuaParser.explist"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitExplist([NotNull] LuaParser.ExplistContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>expNumber</c>
	/// labeled alternative in <see cref="LuaParser.exp"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitExpNumber([NotNull] LuaParser.ExpNumberContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>expTrue</c>
	/// labeled alternative in <see cref="LuaParser.exp"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitExpTrue([NotNull] LuaParser.ExpTrueContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>expOr</c>
	/// labeled alternative in <see cref="LuaParser.exp"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitExpOr([NotNull] LuaParser.ExpOrContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>expBitwise</c>
	/// labeled alternative in <see cref="LuaParser.exp"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitExpBitwise([NotNull] LuaParser.ExpBitwiseContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>expMulDivMod</c>
	/// labeled alternative in <see cref="LuaParser.exp"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitExpMulDivMod([NotNull] LuaParser.ExpMulDivModContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>expFalse</c>
	/// labeled alternative in <see cref="LuaParser.exp"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitExpFalse([NotNull] LuaParser.ExpFalseContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>expString</c>
	/// labeled alternative in <see cref="LuaParser.exp"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitExpString([NotNull] LuaParser.ExpStringContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>expPrefix</c>
	/// labeled alternative in <see cref="LuaParser.exp"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitExpPrefix([NotNull] LuaParser.ExpPrefixContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>expUnary</c>
	/// labeled alternative in <see cref="LuaParser.exp"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitExpUnary([NotNull] LuaParser.ExpUnaryContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>expAnd</c>
	/// labeled alternative in <see cref="LuaParser.exp"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitExpAnd([NotNull] LuaParser.ExpAndContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>expPow</c>
	/// labeled alternative in <see cref="LuaParser.exp"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitExpPow([NotNull] LuaParser.ExpPowContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>expFunction</c>
	/// labeled alternative in <see cref="LuaParser.exp"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitExpFunction([NotNull] LuaParser.ExpFunctionContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>expCompare</c>
	/// labeled alternative in <see cref="LuaParser.exp"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitExpCompare([NotNull] LuaParser.ExpCompareContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>expConcat</c>
	/// labeled alternative in <see cref="LuaParser.exp"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitExpConcat([NotNull] LuaParser.ExpConcatContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>expNil</c>
	/// labeled alternative in <see cref="LuaParser.exp"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitExpNil([NotNull] LuaParser.ExpNilContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>expAddSub</c>
	/// labeled alternative in <see cref="LuaParser.exp"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitExpAddSub([NotNull] LuaParser.ExpAddSubContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>expVarArg</c>
	/// labeled alternative in <see cref="LuaParser.exp"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitExpVarArg([NotNull] LuaParser.ExpVarArgContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>expTable</c>
	/// labeled alternative in <see cref="LuaParser.exp"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitExpTable([NotNull] LuaParser.ExpTableContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="LuaParser.prefixexp"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitPrefixexp([NotNull] LuaParser.PrefixexpContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="LuaParser.functioncall"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitFunctioncall([NotNull] LuaParser.FunctioncallContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="LuaParser.varOrExp"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitVarOrExp([NotNull] LuaParser.VarOrExpContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="LuaParser.var"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitVar([NotNull] LuaParser.VarContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="LuaParser.varSuffix"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitVarSuffix([NotNull] LuaParser.VarSuffixContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="LuaParser.nameAndArgs"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitNameAndArgs([NotNull] LuaParser.NameAndArgsContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="LuaParser.args"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitArgs([NotNull] LuaParser.ArgsContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="LuaParser.functiondef"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitFunctiondef([NotNull] LuaParser.FunctiondefContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="LuaParser.funcbody"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitFuncbody([NotNull] LuaParser.FuncbodyContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="LuaParser.parlist"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitParlist([NotNull] LuaParser.ParlistContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="LuaParser.tableconstructor"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitTableconstructor([NotNull] LuaParser.TableconstructorContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="LuaParser.fieldlist"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitFieldlist([NotNull] LuaParser.FieldlistContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="LuaParser.field"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitField([NotNull] LuaParser.FieldContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="LuaParser.fieldsep"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitFieldsep([NotNull] LuaParser.FieldsepContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="LuaParser.operatorOr"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitOperatorOr([NotNull] LuaParser.OperatorOrContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="LuaParser.operatorAnd"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitOperatorAnd([NotNull] LuaParser.OperatorAndContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="LuaParser.operatorComparison"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitOperatorComparison([NotNull] LuaParser.OperatorComparisonContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="LuaParser.operatorStrcat"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitOperatorStrcat([NotNull] LuaParser.OperatorStrcatContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="LuaParser.operatorAddSub"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitOperatorAddSub([NotNull] LuaParser.OperatorAddSubContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="LuaParser.operatorMulDivMod"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitOperatorMulDivMod([NotNull] LuaParser.OperatorMulDivModContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="LuaParser.operatorBitwise"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitOperatorBitwise([NotNull] LuaParser.OperatorBitwiseContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="LuaParser.operatorUnary"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitOperatorUnary([NotNull] LuaParser.OperatorUnaryContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="LuaParser.operatorPower"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitOperatorPower([NotNull] LuaParser.OperatorPowerContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="LuaParser.number"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitNumber([NotNull] LuaParser.NumberContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="LuaParser.string"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitString([NotNull] LuaParser.StringContext context);
}
