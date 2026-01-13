// Unreachable code detected
#pragma warning disable 0162
// The variable '...' is assigned but its value is never used
#pragma warning disable 0219
// Missing XML comment for publicly visible type or member '...'
#pragma warning disable 1591
// Ambiguous reference in cref attribute
#pragma warning disable 419

using Antlr4.Runtime;
using Antlr4.Runtime.Atn;
using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;
using DFA = Antlr4.Runtime.Dfa.DFA;

namespace MoonsecDeobfuscator.Syntax.Parser;

[System.CodeDom.Compiler.GeneratedCode("ANTLR", "4.13.2")]
[System.CLSCompliant(false)]
public partial class LuaParser : Antlr4.Runtime.Parser {
	protected static DFA[] decisionToDFA;
	protected static PredictionContextCache sharedContextCache = new PredictionContextCache();
	public const int
		T__0=1, T__1=2, T__2=3, T__3=4, T__4=5, T__5=6, T__6=7, T__7=8, T__8=9, 
		T__9=10, T__10=11, T__11=12, T__12=13, T__13=14, T__14=15, T__15=16, T__16=17, 
		T__17=18, T__18=19, T__19=20, T__20=21, T__21=22, T__22=23, T__23=24, 
		T__24=25, T__25=26, T__26=27, T__27=28, T__28=29, T__29=30, T__30=31, 
		T__31=32, T__32=33, T__33=34, T__34=35, T__35=36, T__36=37, T__37=38, 
		T__38=39, T__39=40, T__40=41, T__41=42, T__42=43, T__43=44, T__44=45, 
		T__45=46, T__46=47, T__47=48, T__48=49, T__49=50, T__50=51, T__51=52, 
		T__52=53, T__53=54, T__54=55, NAME=56, NORMALSTRING=57, CHARSTRING=58, 
		LONGSTRING=59, INT=60, HEX=61, FLOAT=62, HEX_FLOAT=63, COMMENT=64, LINE_COMMENT=65, 
		WS=66, SHEBANG=67;
	public const int
		RULE_chunk = 0, RULE_block = 1, RULE_stat = 2, RULE_ifstmt = 3, RULE_elseifstmt = 4, 
		RULE_elsestmt = 5, RULE_retstat = 6, RULE_label = 7, RULE_funcname = 8, 
		RULE_varlist = 9, RULE_namelist = 10, RULE_explist = 11, RULE_exp = 12, 
		RULE_prefixexp = 13, RULE_functioncall = 14, RULE_varOrExp = 15, RULE_var = 16, 
		RULE_varSuffix = 17, RULE_nameAndArgs = 18, RULE_args = 19, RULE_functiondef = 20, 
		RULE_funcbody = 21, RULE_parlist = 22, RULE_tableconstructor = 23, RULE_fieldlist = 24, 
		RULE_field = 25, RULE_fieldsep = 26, RULE_operatorOr = 27, RULE_operatorAnd = 28, 
		RULE_operatorComparison = 29, RULE_operatorStrcat = 30, RULE_operatorAddSub = 31, 
		RULE_operatorMulDivMod = 32, RULE_operatorBitwise = 33, RULE_operatorUnary = 34, 
		RULE_operatorPower = 35, RULE_number = 36, RULE_string = 37;
	public static readonly string[] ruleNames = {
		"chunk", "block", "stat", "ifstmt", "elseifstmt", "elsestmt", "retstat", 
		"label", "funcname", "varlist", "namelist", "explist", "exp", "prefixexp", 
		"functioncall", "varOrExp", "var", "varSuffix", "nameAndArgs", "args", 
		"functiondef", "funcbody", "parlist", "tableconstructor", "fieldlist", 
		"field", "fieldsep", "operatorOr", "operatorAnd", "operatorComparison", 
		"operatorStrcat", "operatorAddSub", "operatorMulDivMod", "operatorBitwise", 
		"operatorUnary", "operatorPower", "number", "string"
	};

	private static readonly string[] _LiteralNames = {
		null, "';'", "'='", "'break'", "'goto'", "'do'", "'end'", "'while'", "'repeat'", 
		"'until'", "'for'", "','", "'in'", "'function'", "'local'", "'if'", "'then'", 
		"'elseif'", "'else'", "'return'", "'::'", "'.'", "':'", "'nil'", "'false'", 
		"'true'", "'...'", "'('", "')'", "'['", "']'", "'{'", "'}'", "'or'", "'and'", 
		"'<'", "'>'", "'<='", "'>='", "'~='", "'=='", "'..'", "'+'", "'-'", "'*'", 
		"'/'", "'%'", "'//'", "'&'", "'|'", "'~'", "'<<'", "'>>'", "'not'", "'#'", 
		"'^'"
	};
	private static readonly string[] _SymbolicNames = {
		null, null, null, null, null, null, null, null, null, null, null, null, 
		null, null, null, null, null, null, null, null, null, null, null, null, 
		null, null, null, null, null, null, null, null, null, null, null, null, 
		null, null, null, null, null, null, null, null, null, null, null, null, 
		null, null, null, null, null, null, null, null, "NAME", "NORMALSTRING", 
		"CHARSTRING", "LONGSTRING", "INT", "HEX", "FLOAT", "HEX_FLOAT", "COMMENT", 
		"LINE_COMMENT", "WS", "SHEBANG"
	};
	public static readonly IVocabulary DefaultVocabulary = new Vocabulary(_LiteralNames, _SymbolicNames);

	[NotNull]
	public override IVocabulary Vocabulary
	{
		get
		{
			return DefaultVocabulary;
		}
	}

	public override string GrammarFileName { get { return "Lua.g4"; } }

	public override string[] RuleNames { get { return ruleNames; } }

	public override int[] SerializedAtn { get { return _serializedATN; } }

	static LuaParser() {
		decisionToDFA = new DFA[_ATN.NumberOfDecisions];
		for (int i = 0; i < _ATN.NumberOfDecisions; i++) {
			decisionToDFA[i] = new DFA(_ATN.GetDecisionState(i), i);
		}
	}

		public LuaParser(ITokenStream input) : this(input, Console.Out, Console.Error) { }

		public LuaParser(ITokenStream input, TextWriter output, TextWriter errorOutput)
		: base(input, output, errorOutput)
	{
		Interpreter = new ParserATNSimulator(this, _ATN, decisionToDFA, sharedContextCache);
	}

	public partial class ChunkContext : ParserRuleContext {
		[System.Diagnostics.DebuggerNonUserCode] public BlockContext block() {
			return GetRuleContext<BlockContext>(0);
		}
		[System.Diagnostics.DebuggerNonUserCode] public ITerminalNode Eof() { return GetToken(LuaParser.Eof, 0); }
		public ChunkContext(ParserRuleContext parent, int invokingState)
			: base(parent, invokingState)
		{
		}
		public override int RuleIndex { get { return RULE_chunk; } }
		[System.Diagnostics.DebuggerNonUserCode]
		public override TResult Accept<TResult>(IParseTreeVisitor<TResult> visitor) {
			ILuaVisitor<TResult> typedVisitor = visitor as ILuaVisitor<TResult>;
			if (typedVisitor != null) return typedVisitor.VisitChunk(this);
			else return visitor.VisitChildren(this);
		}
	}

	[RuleVersion(0)]
	public ChunkContext chunk() {
		ChunkContext _localctx = new ChunkContext(Context, State);
		EnterRule(_localctx, 0, RULE_chunk);
		try {
			EnterOuterAlt(_localctx, 1);
			{
			State = 76;
			block();
			State = 77;
			Match(Eof);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			ErrorHandler.ReportError(this, re);
			ErrorHandler.Recover(this, re);
		}
		finally {
			ExitRule();
		}
		return _localctx;
	}

	public partial class BlockContext : ParserRuleContext {
		[System.Diagnostics.DebuggerNonUserCode] public StatContext[] stat() {
			return GetRuleContexts<StatContext>();
		}
		[System.Diagnostics.DebuggerNonUserCode] public StatContext stat(int i) {
			return GetRuleContext<StatContext>(i);
		}
		[System.Diagnostics.DebuggerNonUserCode] public RetstatContext retstat() {
			return GetRuleContext<RetstatContext>(0);
		}
		public BlockContext(ParserRuleContext parent, int invokingState)
			: base(parent, invokingState)
		{
		}
		public override int RuleIndex { get { return RULE_block; } }
		[System.Diagnostics.DebuggerNonUserCode]
		public override TResult Accept<TResult>(IParseTreeVisitor<TResult> visitor) {
			ILuaVisitor<TResult> typedVisitor = visitor as ILuaVisitor<TResult>;
			if (typedVisitor != null) return typedVisitor.VisitBlock(this);
			else return visitor.VisitChildren(this);
		}
	}

	[RuleVersion(0)]
	public BlockContext block() {
		BlockContext _localctx = new BlockContext(Context, State);
		EnterRule(_localctx, 2, RULE_block);
		int _la;
		try {
			EnterOuterAlt(_localctx, 1);
			{
			State = 82;
			ErrorHandler.Sync(this);
			_la = TokenStream.LA(1);
			while ((((_la) & ~0x3f) == 0 && ((1L << _la) & 72057594173253050L) != 0)) {
				{
				{
				State = 79;
				stat();
				}
				}
				State = 84;
				ErrorHandler.Sync(this);
				_la = TokenStream.LA(1);
			}
			State = 86;
			ErrorHandler.Sync(this);
			_la = TokenStream.LA(1);
			if (_la==T__18) {
				{
				State = 85;
				retstat();
				}
			}

			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			ErrorHandler.ReportError(this, re);
			ErrorHandler.Recover(this, re);
		}
		finally {
			ExitRule();
		}
		return _localctx;
	}

	public partial class StatContext : ParserRuleContext {
		public StatContext(ParserRuleContext parent, int invokingState)
			: base(parent, invokingState)
		{
		}
		public override int RuleIndex { get { return RULE_stat; } }
	 
		public StatContext() { }
		public virtual void CopyFrom(StatContext context) {
			base.CopyFrom(context);
		}
	}
	public partial class StatGenericForContext : StatContext {
		[System.Diagnostics.DebuggerNonUserCode] public NamelistContext namelist() {
			return GetRuleContext<NamelistContext>(0);
		}
		[System.Diagnostics.DebuggerNonUserCode] public ExplistContext explist() {
			return GetRuleContext<ExplistContext>(0);
		}
		[System.Diagnostics.DebuggerNonUserCode] public BlockContext block() {
			return GetRuleContext<BlockContext>(0);
		}
		public StatGenericForContext(StatContext context) { CopyFrom(context); }
		[System.Diagnostics.DebuggerNonUserCode]
		public override TResult Accept<TResult>(IParseTreeVisitor<TResult> visitor) {
			ILuaVisitor<TResult> typedVisitor = visitor as ILuaVisitor<TResult>;
			if (typedVisitor != null) return typedVisitor.VisitStatGenericFor(this);
			else return visitor.VisitChildren(this);
		}
	}
	public partial class StatFunctionContext : StatContext {
		[System.Diagnostics.DebuggerNonUserCode] public FuncnameContext funcname() {
			return GetRuleContext<FuncnameContext>(0);
		}
		[System.Diagnostics.DebuggerNonUserCode] public FuncbodyContext funcbody() {
			return GetRuleContext<FuncbodyContext>(0);
		}
		public StatFunctionContext(StatContext context) { CopyFrom(context); }
		[System.Diagnostics.DebuggerNonUserCode]
		public override TResult Accept<TResult>(IParseTreeVisitor<TResult> visitor) {
			ILuaVisitor<TResult> typedVisitor = visitor as ILuaVisitor<TResult>;
			if (typedVisitor != null) return typedVisitor.VisitStatFunction(this);
			else return visitor.VisitChildren(this);
		}
	}
	public partial class StatBreakContext : StatContext {
		public StatBreakContext(StatContext context) { CopyFrom(context); }
		[System.Diagnostics.DebuggerNonUserCode]
		public override TResult Accept<TResult>(IParseTreeVisitor<TResult> visitor) {
			ILuaVisitor<TResult> typedVisitor = visitor as ILuaVisitor<TResult>;
			if (typedVisitor != null) return typedVisitor.VisitStatBreak(this);
			else return visitor.VisitChildren(this);
		}
	}
	public partial class StatSemiContext : StatContext {
		public StatSemiContext(StatContext context) { CopyFrom(context); }
		[System.Diagnostics.DebuggerNonUserCode]
		public override TResult Accept<TResult>(IParseTreeVisitor<TResult> visitor) {
			ILuaVisitor<TResult> typedVisitor = visitor as ILuaVisitor<TResult>;
			if (typedVisitor != null) return typedVisitor.VisitStatSemi(this);
			else return visitor.VisitChildren(this);
		}
	}
	public partial class StatLocalFunctionContext : StatContext {
		[System.Diagnostics.DebuggerNonUserCode] public ITerminalNode NAME() { return GetToken(LuaParser.NAME, 0); }
		[System.Diagnostics.DebuggerNonUserCode] public FuncbodyContext funcbody() {
			return GetRuleContext<FuncbodyContext>(0);
		}
		public StatLocalFunctionContext(StatContext context) { CopyFrom(context); }
		[System.Diagnostics.DebuggerNonUserCode]
		public override TResult Accept<TResult>(IParseTreeVisitor<TResult> visitor) {
			ILuaVisitor<TResult> typedVisitor = visitor as ILuaVisitor<TResult>;
			if (typedVisitor != null) return typedVisitor.VisitStatLocalFunction(this);
			else return visitor.VisitChildren(this);
		}
	}
	public partial class StatLocalDeclareContext : StatContext {
		[System.Diagnostics.DebuggerNonUserCode] public NamelistContext namelist() {
			return GetRuleContext<NamelistContext>(0);
		}
		[System.Diagnostics.DebuggerNonUserCode] public ExplistContext explist() {
			return GetRuleContext<ExplistContext>(0);
		}
		public StatLocalDeclareContext(StatContext context) { CopyFrom(context); }
		[System.Diagnostics.DebuggerNonUserCode]
		public override TResult Accept<TResult>(IParseTreeVisitor<TResult> visitor) {
			ILuaVisitor<TResult> typedVisitor = visitor as ILuaVisitor<TResult>;
			if (typedVisitor != null) return typedVisitor.VisitStatLocalDeclare(this);
			else return visitor.VisitChildren(this);
		}
	}
	public partial class StatCallContext : StatContext {
		[System.Diagnostics.DebuggerNonUserCode] public FunctioncallContext functioncall() {
			return GetRuleContext<FunctioncallContext>(0);
		}
		public StatCallContext(StatContext context) { CopyFrom(context); }
		[System.Diagnostics.DebuggerNonUserCode]
		public override TResult Accept<TResult>(IParseTreeVisitor<TResult> visitor) {
			ILuaVisitor<TResult> typedVisitor = visitor as ILuaVisitor<TResult>;
			if (typedVisitor != null) return typedVisitor.VisitStatCall(this);
			else return visitor.VisitChildren(this);
		}
	}
	public partial class StatLabelContext : StatContext {
		[System.Diagnostics.DebuggerNonUserCode] public LabelContext label() {
			return GetRuleContext<LabelContext>(0);
		}
		public StatLabelContext(StatContext context) { CopyFrom(context); }
		[System.Diagnostics.DebuggerNonUserCode]
		public override TResult Accept<TResult>(IParseTreeVisitor<TResult> visitor) {
			ILuaVisitor<TResult> typedVisitor = visitor as ILuaVisitor<TResult>;
			if (typedVisitor != null) return typedVisitor.VisitStatLabel(this);
			else return visitor.VisitChildren(this);
		}
	}
	public partial class StatDoContext : StatContext {
		[System.Diagnostics.DebuggerNonUserCode] public BlockContext block() {
			return GetRuleContext<BlockContext>(0);
		}
		public StatDoContext(StatContext context) { CopyFrom(context); }
		[System.Diagnostics.DebuggerNonUserCode]
		public override TResult Accept<TResult>(IParseTreeVisitor<TResult> visitor) {
			ILuaVisitor<TResult> typedVisitor = visitor as ILuaVisitor<TResult>;
			if (typedVisitor != null) return typedVisitor.VisitStatDo(this);
			else return visitor.VisitChildren(this);
		}
	}
	public partial class StatWhileContext : StatContext {
		[System.Diagnostics.DebuggerNonUserCode] public ExpContext exp() {
			return GetRuleContext<ExpContext>(0);
		}
		[System.Diagnostics.DebuggerNonUserCode] public BlockContext block() {
			return GetRuleContext<BlockContext>(0);
		}
		public StatWhileContext(StatContext context) { CopyFrom(context); }
		[System.Diagnostics.DebuggerNonUserCode]
		public override TResult Accept<TResult>(IParseTreeVisitor<TResult> visitor) {
			ILuaVisitor<TResult> typedVisitor = visitor as ILuaVisitor<TResult>;
			if (typedVisitor != null) return typedVisitor.VisitStatWhile(this);
			else return visitor.VisitChildren(this);
		}
	}
	public partial class StatGotoContext : StatContext {
		[System.Diagnostics.DebuggerNonUserCode] public ITerminalNode NAME() { return GetToken(LuaParser.NAME, 0); }
		public StatGotoContext(StatContext context) { CopyFrom(context); }
		[System.Diagnostics.DebuggerNonUserCode]
		public override TResult Accept<TResult>(IParseTreeVisitor<TResult> visitor) {
			ILuaVisitor<TResult> typedVisitor = visitor as ILuaVisitor<TResult>;
			if (typedVisitor != null) return typedVisitor.VisitStatGoto(this);
			else return visitor.VisitChildren(this);
		}
	}
	public partial class StatAssignContext : StatContext {
		[System.Diagnostics.DebuggerNonUserCode] public VarlistContext varlist() {
			return GetRuleContext<VarlistContext>(0);
		}
		[System.Diagnostics.DebuggerNonUserCode] public ExplistContext explist() {
			return GetRuleContext<ExplistContext>(0);
		}
		public StatAssignContext(StatContext context) { CopyFrom(context); }
		[System.Diagnostics.DebuggerNonUserCode]
		public override TResult Accept<TResult>(IParseTreeVisitor<TResult> visitor) {
			ILuaVisitor<TResult> typedVisitor = visitor as ILuaVisitor<TResult>;
			if (typedVisitor != null) return typedVisitor.VisitStatAssign(this);
			else return visitor.VisitChildren(this);
		}
	}
	public partial class StatRepeatContext : StatContext {
		[System.Diagnostics.DebuggerNonUserCode] public BlockContext block() {
			return GetRuleContext<BlockContext>(0);
		}
		[System.Diagnostics.DebuggerNonUserCode] public ExpContext exp() {
			return GetRuleContext<ExpContext>(0);
		}
		public StatRepeatContext(StatContext context) { CopyFrom(context); }
		[System.Diagnostics.DebuggerNonUserCode]
		public override TResult Accept<TResult>(IParseTreeVisitor<TResult> visitor) {
			ILuaVisitor<TResult> typedVisitor = visitor as ILuaVisitor<TResult>;
			if (typedVisitor != null) return typedVisitor.VisitStatRepeat(this);
			else return visitor.VisitChildren(this);
		}
	}
	public partial class StatIfContext : StatContext {
		[System.Diagnostics.DebuggerNonUserCode] public IfstmtContext ifstmt() {
			return GetRuleContext<IfstmtContext>(0);
		}
		[System.Diagnostics.DebuggerNonUserCode] public ElseifstmtContext elseifstmt() {
			return GetRuleContext<ElseifstmtContext>(0);
		}
		[System.Diagnostics.DebuggerNonUserCode] public ElsestmtContext elsestmt() {
			return GetRuleContext<ElsestmtContext>(0);
		}
		public StatIfContext(StatContext context) { CopyFrom(context); }
		[System.Diagnostics.DebuggerNonUserCode]
		public override TResult Accept<TResult>(IParseTreeVisitor<TResult> visitor) {
			ILuaVisitor<TResult> typedVisitor = visitor as ILuaVisitor<TResult>;
			if (typedVisitor != null) return typedVisitor.VisitStatIf(this);
			else return visitor.VisitChildren(this);
		}
	}
	public partial class StatNumericForContext : StatContext {
		[System.Diagnostics.DebuggerNonUserCode] public ITerminalNode NAME() { return GetToken(LuaParser.NAME, 0); }
		[System.Diagnostics.DebuggerNonUserCode] public ExpContext[] exp() {
			return GetRuleContexts<ExpContext>();
		}
		[System.Diagnostics.DebuggerNonUserCode] public ExpContext exp(int i) {
			return GetRuleContext<ExpContext>(i);
		}
		[System.Diagnostics.DebuggerNonUserCode] public BlockContext block() {
			return GetRuleContext<BlockContext>(0);
		}
		public StatNumericForContext(StatContext context) { CopyFrom(context); }
		[System.Diagnostics.DebuggerNonUserCode]
		public override TResult Accept<TResult>(IParseTreeVisitor<TResult> visitor) {
			ILuaVisitor<TResult> typedVisitor = visitor as ILuaVisitor<TResult>;
			if (typedVisitor != null) return typedVisitor.VisitStatNumericFor(this);
			else return visitor.VisitChildren(this);
		}
	}

	[RuleVersion(0)]
	public StatContext stat() {
		StatContext _localctx = new StatContext(Context, State);
		EnterRule(_localctx, 4, RULE_stat);
		int _la;
		try {
			State = 154;
			ErrorHandler.Sync(this);
			switch ( Interpreter.AdaptivePredict(TokenStream,4,Context) ) {
			case 1:
				_localctx = new StatSemiContext(_localctx);
				EnterOuterAlt(_localctx, 1);
				{
				State = 88;
				Match(T__0);
				}
				break;
			case 2:
				_localctx = new StatAssignContext(_localctx);
				EnterOuterAlt(_localctx, 2);
				{
				State = 89;
				varlist();
				State = 90;
				Match(T__1);
				State = 91;
				explist();
				}
				break;
			case 3:
				_localctx = new StatCallContext(_localctx);
				EnterOuterAlt(_localctx, 3);
				{
				State = 93;
				functioncall();
				}
				break;
			case 4:
				_localctx = new StatLabelContext(_localctx);
				EnterOuterAlt(_localctx, 4);
				{
				State = 94;
				label();
				}
				break;
			case 5:
				_localctx = new StatBreakContext(_localctx);
				EnterOuterAlt(_localctx, 5);
				{
				State = 95;
				Match(T__2);
				}
				break;
			case 6:
				_localctx = new StatGotoContext(_localctx);
				EnterOuterAlt(_localctx, 6);
				{
				State = 96;
				Match(T__3);
				State = 97;
				Match(NAME);
				}
				break;
			case 7:
				_localctx = new StatDoContext(_localctx);
				EnterOuterAlt(_localctx, 7);
				{
				State = 98;
				Match(T__4);
				State = 99;
				block();
				State = 100;
				Match(T__5);
				}
				break;
			case 8:
				_localctx = new StatWhileContext(_localctx);
				EnterOuterAlt(_localctx, 8);
				{
				State = 102;
				Match(T__6);
				State = 103;
				exp(0);
				State = 104;
				Match(T__4);
				State = 105;
				block();
				State = 106;
				Match(T__5);
				}
				break;
			case 9:
				_localctx = new StatRepeatContext(_localctx);
				EnterOuterAlt(_localctx, 9);
				{
				State = 108;
				Match(T__7);
				State = 109;
				block();
				State = 110;
				Match(T__8);
				State = 111;
				exp(0);
				}
				break;
			case 10:
				_localctx = new StatIfContext(_localctx);
				EnterOuterAlt(_localctx, 10);
				{
				State = 113;
				ifstmt();
				State = 114;
				elseifstmt();
				State = 115;
				elsestmt();
				State = 116;
				Match(T__5);
				}
				break;
			case 11:
				_localctx = new StatNumericForContext(_localctx);
				EnterOuterAlt(_localctx, 11);
				{
				State = 118;
				Match(T__9);
				State = 119;
				Match(NAME);
				State = 120;
				Match(T__1);
				State = 121;
				exp(0);
				State = 122;
				Match(T__10);
				State = 123;
				exp(0);
				State = 126;
				ErrorHandler.Sync(this);
				_la = TokenStream.LA(1);
				if (_la==T__10) {
					{
					State = 124;
					Match(T__10);
					State = 125;
					exp(0);
					}
				}

				State = 128;
				Match(T__4);
				State = 129;
				block();
				State = 130;
				Match(T__5);
				}
				break;
			case 12:
				_localctx = new StatGenericForContext(_localctx);
				EnterOuterAlt(_localctx, 12);
				{
				State = 132;
				Match(T__9);
				State = 133;
				namelist();
				State = 134;
				Match(T__11);
				State = 135;
				explist();
				State = 136;
				Match(T__4);
				State = 137;
				block();
				State = 138;
				Match(T__5);
				}
				break;
			case 13:
				_localctx = new StatFunctionContext(_localctx);
				EnterOuterAlt(_localctx, 13);
				{
				State = 140;
				Match(T__12);
				State = 141;
				funcname();
				State = 142;
				funcbody();
				}
				break;
			case 14:
				_localctx = new StatLocalFunctionContext(_localctx);
				EnterOuterAlt(_localctx, 14);
				{
				State = 144;
				Match(T__13);
				State = 145;
				Match(T__12);
				State = 146;
				Match(NAME);
				State = 147;
				funcbody();
				}
				break;
			case 15:
				_localctx = new StatLocalDeclareContext(_localctx);
				EnterOuterAlt(_localctx, 15);
				{
				State = 148;
				Match(T__13);
				State = 149;
				namelist();
				State = 152;
				ErrorHandler.Sync(this);
				_la = TokenStream.LA(1);
				if (_la==T__1) {
					{
					State = 150;
					Match(T__1);
					State = 151;
					explist();
					}
				}

				}
				break;
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			ErrorHandler.ReportError(this, re);
			ErrorHandler.Recover(this, re);
		}
		finally {
			ExitRule();
		}
		return _localctx;
	}

	public partial class IfstmtContext : ParserRuleContext {
		[System.Diagnostics.DebuggerNonUserCode] public ExpContext exp() {
			return GetRuleContext<ExpContext>(0);
		}
		[System.Diagnostics.DebuggerNonUserCode] public BlockContext block() {
			return GetRuleContext<BlockContext>(0);
		}
		public IfstmtContext(ParserRuleContext parent, int invokingState)
			: base(parent, invokingState)
		{
		}
		public override int RuleIndex { get { return RULE_ifstmt; } }
		[System.Diagnostics.DebuggerNonUserCode]
		public override TResult Accept<TResult>(IParseTreeVisitor<TResult> visitor) {
			ILuaVisitor<TResult> typedVisitor = visitor as ILuaVisitor<TResult>;
			if (typedVisitor != null) return typedVisitor.VisitIfstmt(this);
			else return visitor.VisitChildren(this);
		}
	}

	[RuleVersion(0)]
	public IfstmtContext ifstmt() {
		IfstmtContext _localctx = new IfstmtContext(Context, State);
		EnterRule(_localctx, 6, RULE_ifstmt);
		try {
			EnterOuterAlt(_localctx, 1);
			{
			State = 156;
			Match(T__14);
			State = 157;
			exp(0);
			State = 158;
			Match(T__15);
			State = 159;
			block();
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			ErrorHandler.ReportError(this, re);
			ErrorHandler.Recover(this, re);
		}
		finally {
			ExitRule();
		}
		return _localctx;
	}

	public partial class ElseifstmtContext : ParserRuleContext {
		[System.Diagnostics.DebuggerNonUserCode] public ExpContext[] exp() {
			return GetRuleContexts<ExpContext>();
		}
		[System.Diagnostics.DebuggerNonUserCode] public ExpContext exp(int i) {
			return GetRuleContext<ExpContext>(i);
		}
		[System.Diagnostics.DebuggerNonUserCode] public BlockContext[] block() {
			return GetRuleContexts<BlockContext>();
		}
		[System.Diagnostics.DebuggerNonUserCode] public BlockContext block(int i) {
			return GetRuleContext<BlockContext>(i);
		}
		public ElseifstmtContext(ParserRuleContext parent, int invokingState)
			: base(parent, invokingState)
		{
		}
		public override int RuleIndex { get { return RULE_elseifstmt; } }
		[System.Diagnostics.DebuggerNonUserCode]
		public override TResult Accept<TResult>(IParseTreeVisitor<TResult> visitor) {
			ILuaVisitor<TResult> typedVisitor = visitor as ILuaVisitor<TResult>;
			if (typedVisitor != null) return typedVisitor.VisitElseifstmt(this);
			else return visitor.VisitChildren(this);
		}
	}

	[RuleVersion(0)]
	public ElseifstmtContext elseifstmt() {
		ElseifstmtContext _localctx = new ElseifstmtContext(Context, State);
		EnterRule(_localctx, 8, RULE_elseifstmt);
		int _la;
		try {
			EnterOuterAlt(_localctx, 1);
			{
			State = 168;
			ErrorHandler.Sync(this);
			_la = TokenStream.LA(1);
			while (_la==T__16) {
				{
				{
				State = 161;
				Match(T__16);
				State = 162;
				exp(0);
				State = 163;
				Match(T__15);
				State = 164;
				block();
				}
				}
				State = 170;
				ErrorHandler.Sync(this);
				_la = TokenStream.LA(1);
			}
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			ErrorHandler.ReportError(this, re);
			ErrorHandler.Recover(this, re);
		}
		finally {
			ExitRule();
		}
		return _localctx;
	}

	public partial class ElsestmtContext : ParserRuleContext {
		[System.Diagnostics.DebuggerNonUserCode] public BlockContext block() {
			return GetRuleContext<BlockContext>(0);
		}
		public ElsestmtContext(ParserRuleContext parent, int invokingState)
			: base(parent, invokingState)
		{
		}
		public override int RuleIndex { get { return RULE_elsestmt; } }
		[System.Diagnostics.DebuggerNonUserCode]
		public override TResult Accept<TResult>(IParseTreeVisitor<TResult> visitor) {
			ILuaVisitor<TResult> typedVisitor = visitor as ILuaVisitor<TResult>;
			if (typedVisitor != null) return typedVisitor.VisitElsestmt(this);
			else return visitor.VisitChildren(this);
		}
	}

	[RuleVersion(0)]
	public ElsestmtContext elsestmt() {
		ElsestmtContext _localctx = new ElsestmtContext(Context, State);
		EnterRule(_localctx, 10, RULE_elsestmt);
		int _la;
		try {
			EnterOuterAlt(_localctx, 1);
			{
			State = 173;
			ErrorHandler.Sync(this);
			_la = TokenStream.LA(1);
			if (_la==T__17) {
				{
				State = 171;
				Match(T__17);
				State = 172;
				block();
				}
			}

			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			ErrorHandler.ReportError(this, re);
			ErrorHandler.Recover(this, re);
		}
		finally {
			ExitRule();
		}
		return _localctx;
	}

	public partial class RetstatContext : ParserRuleContext {
		[System.Diagnostics.DebuggerNonUserCode] public ExplistContext explist() {
			return GetRuleContext<ExplistContext>(0);
		}
		public RetstatContext(ParserRuleContext parent, int invokingState)
			: base(parent, invokingState)
		{
		}
		public override int RuleIndex { get { return RULE_retstat; } }
		[System.Diagnostics.DebuggerNonUserCode]
		public override TResult Accept<TResult>(IParseTreeVisitor<TResult> visitor) {
			ILuaVisitor<TResult> typedVisitor = visitor as ILuaVisitor<TResult>;
			if (typedVisitor != null) return typedVisitor.VisitRetstat(this);
			else return visitor.VisitChildren(this);
		}
	}

	[RuleVersion(0)]
	public RetstatContext retstat() {
		RetstatContext _localctx = new RetstatContext(Context, State);
		EnterRule(_localctx, 12, RULE_retstat);
		int _la;
		try {
			EnterOuterAlt(_localctx, 1);
			{
			State = 175;
			Match(T__18);
			State = 177;
			ErrorHandler.Sync(this);
			_la = TokenStream.LA(1);
			if ((((_la) & ~0x3f) == 0 && ((1L << _la) & -43901297866301440L) != 0)) {
				{
				State = 176;
				explist();
				}
			}

			State = 180;
			ErrorHandler.Sync(this);
			_la = TokenStream.LA(1);
			if (_la==T__0) {
				{
				State = 179;
				Match(T__0);
				}
			}

			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			ErrorHandler.ReportError(this, re);
			ErrorHandler.Recover(this, re);
		}
		finally {
			ExitRule();
		}
		return _localctx;
	}

	public partial class LabelContext : ParserRuleContext {
		[System.Diagnostics.DebuggerNonUserCode] public ITerminalNode NAME() { return GetToken(LuaParser.NAME, 0); }
		public LabelContext(ParserRuleContext parent, int invokingState)
			: base(parent, invokingState)
		{
		}
		public override int RuleIndex { get { return RULE_label; } }
		[System.Diagnostics.DebuggerNonUserCode]
		public override TResult Accept<TResult>(IParseTreeVisitor<TResult> visitor) {
			ILuaVisitor<TResult> typedVisitor = visitor as ILuaVisitor<TResult>;
			if (typedVisitor != null) return typedVisitor.VisitLabel(this);
			else return visitor.VisitChildren(this);
		}
	}

	[RuleVersion(0)]
	public LabelContext label() {
		LabelContext _localctx = new LabelContext(Context, State);
		EnterRule(_localctx, 14, RULE_label);
		try {
			EnterOuterAlt(_localctx, 1);
			{
			State = 182;
			Match(T__19);
			State = 183;
			Match(NAME);
			State = 184;
			Match(T__19);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			ErrorHandler.ReportError(this, re);
			ErrorHandler.Recover(this, re);
		}
		finally {
			ExitRule();
		}
		return _localctx;
	}

	public partial class FuncnameContext : ParserRuleContext {
		[System.Diagnostics.DebuggerNonUserCode] public ITerminalNode[] NAME() { return GetTokens(LuaParser.NAME); }
		[System.Diagnostics.DebuggerNonUserCode] public ITerminalNode NAME(int i) {
			return GetToken(LuaParser.NAME, i);
		}
		public FuncnameContext(ParserRuleContext parent, int invokingState)
			: base(parent, invokingState)
		{
		}
		public override int RuleIndex { get { return RULE_funcname; } }
		[System.Diagnostics.DebuggerNonUserCode]
		public override TResult Accept<TResult>(IParseTreeVisitor<TResult> visitor) {
			ILuaVisitor<TResult> typedVisitor = visitor as ILuaVisitor<TResult>;
			if (typedVisitor != null) return typedVisitor.VisitFuncname(this);
			else return visitor.VisitChildren(this);
		}
	}

	[RuleVersion(0)]
	public FuncnameContext funcname() {
		FuncnameContext _localctx = new FuncnameContext(Context, State);
		EnterRule(_localctx, 16, RULE_funcname);
		int _la;
		try {
			EnterOuterAlt(_localctx, 1);
			{
			State = 186;
			Match(NAME);
			State = 191;
			ErrorHandler.Sync(this);
			_la = TokenStream.LA(1);
			while (_la==T__20) {
				{
				{
				State = 187;
				Match(T__20);
				State = 188;
				Match(NAME);
				}
				}
				State = 193;
				ErrorHandler.Sync(this);
				_la = TokenStream.LA(1);
			}
			State = 196;
			ErrorHandler.Sync(this);
			_la = TokenStream.LA(1);
			if (_la==T__21) {
				{
				State = 194;
				Match(T__21);
				State = 195;
				Match(NAME);
				}
			}

			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			ErrorHandler.ReportError(this, re);
			ErrorHandler.Recover(this, re);
		}
		finally {
			ExitRule();
		}
		return _localctx;
	}

	public partial class VarlistContext : ParserRuleContext {
		[System.Diagnostics.DebuggerNonUserCode] public VarContext[] var() {
			return GetRuleContexts<VarContext>();
		}
		[System.Diagnostics.DebuggerNonUserCode] public VarContext var(int i) {
			return GetRuleContext<VarContext>(i);
		}
		public VarlistContext(ParserRuleContext parent, int invokingState)
			: base(parent, invokingState)
		{
		}
		public override int RuleIndex { get { return RULE_varlist; } }
		[System.Diagnostics.DebuggerNonUserCode]
		public override TResult Accept<TResult>(IParseTreeVisitor<TResult> visitor) {
			ILuaVisitor<TResult> typedVisitor = visitor as ILuaVisitor<TResult>;
			if (typedVisitor != null) return typedVisitor.VisitVarlist(this);
			else return visitor.VisitChildren(this);
		}
	}

	[RuleVersion(0)]
	public VarlistContext varlist() {
		VarlistContext _localctx = new VarlistContext(Context, State);
		EnterRule(_localctx, 18, RULE_varlist);
		int _la;
		try {
			EnterOuterAlt(_localctx, 1);
			{
			State = 198;
			var();
			State = 203;
			ErrorHandler.Sync(this);
			_la = TokenStream.LA(1);
			while (_la==T__10) {
				{
				{
				State = 199;
				Match(T__10);
				State = 200;
				var();
				}
				}
				State = 205;
				ErrorHandler.Sync(this);
				_la = TokenStream.LA(1);
			}
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			ErrorHandler.ReportError(this, re);
			ErrorHandler.Recover(this, re);
		}
		finally {
			ExitRule();
		}
		return _localctx;
	}

	public partial class NamelistContext : ParserRuleContext {
		[System.Diagnostics.DebuggerNonUserCode] public ITerminalNode[] NAME() { return GetTokens(LuaParser.NAME); }
		[System.Diagnostics.DebuggerNonUserCode] public ITerminalNode NAME(int i) {
			return GetToken(LuaParser.NAME, i);
		}
		public NamelistContext(ParserRuleContext parent, int invokingState)
			: base(parent, invokingState)
		{
		}
		public override int RuleIndex { get { return RULE_namelist; } }
		[System.Diagnostics.DebuggerNonUserCode]
		public override TResult Accept<TResult>(IParseTreeVisitor<TResult> visitor) {
			ILuaVisitor<TResult> typedVisitor = visitor as ILuaVisitor<TResult>;
			if (typedVisitor != null) return typedVisitor.VisitNamelist(this);
			else return visitor.VisitChildren(this);
		}
	}

	[RuleVersion(0)]
	public NamelistContext namelist() {
		NamelistContext _localctx = new NamelistContext(Context, State);
		EnterRule(_localctx, 20, RULE_namelist);
		try {
			int _alt;
			EnterOuterAlt(_localctx, 1);
			{
			State = 206;
			Match(NAME);
			State = 211;
			ErrorHandler.Sync(this);
			_alt = Interpreter.AdaptivePredict(TokenStream,12,Context);
			while ( _alt!=2 && _alt!=global::Antlr4.Runtime.Atn.ATN.INVALID_ALT_NUMBER ) {
				if ( _alt==1 ) {
					{
					{
					State = 207;
					Match(T__10);
					State = 208;
					Match(NAME);
					}
					} 
				}
				State = 213;
				ErrorHandler.Sync(this);
				_alt = Interpreter.AdaptivePredict(TokenStream,12,Context);
			}
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			ErrorHandler.ReportError(this, re);
			ErrorHandler.Recover(this, re);
		}
		finally {
			ExitRule();
		}
		return _localctx;
	}

	public partial class ExplistContext : ParserRuleContext {
		[System.Diagnostics.DebuggerNonUserCode] public ExpContext[] exp() {
			return GetRuleContexts<ExpContext>();
		}
		[System.Diagnostics.DebuggerNonUserCode] public ExpContext exp(int i) {
			return GetRuleContext<ExpContext>(i);
		}
		public ExplistContext(ParserRuleContext parent, int invokingState)
			: base(parent, invokingState)
		{
		}
		public override int RuleIndex { get { return RULE_explist; } }
		[System.Diagnostics.DebuggerNonUserCode]
		public override TResult Accept<TResult>(IParseTreeVisitor<TResult> visitor) {
			ILuaVisitor<TResult> typedVisitor = visitor as ILuaVisitor<TResult>;
			if (typedVisitor != null) return typedVisitor.VisitExplist(this);
			else return visitor.VisitChildren(this);
		}
	}

	[RuleVersion(0)]
	public ExplistContext explist() {
		ExplistContext _localctx = new ExplistContext(Context, State);
		EnterRule(_localctx, 22, RULE_explist);
		int _la;
		try {
			EnterOuterAlt(_localctx, 1);
			{
			State = 214;
			exp(0);
			State = 219;
			ErrorHandler.Sync(this);
			_la = TokenStream.LA(1);
			while (_la==T__10) {
				{
				{
				State = 215;
				Match(T__10);
				State = 216;
				exp(0);
				}
				}
				State = 221;
				ErrorHandler.Sync(this);
				_la = TokenStream.LA(1);
			}
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			ErrorHandler.ReportError(this, re);
			ErrorHandler.Recover(this, re);
		}
		finally {
			ExitRule();
		}
		return _localctx;
	}

	public partial class ExpContext : ParserRuleContext {
		public ExpContext(ParserRuleContext parent, int invokingState)
			: base(parent, invokingState)
		{
		}
		public override int RuleIndex { get { return RULE_exp; } }
	 
		public ExpContext() { }
		public virtual void CopyFrom(ExpContext context) {
			base.CopyFrom(context);
		}
	}
	public partial class ExpNumberContext : ExpContext {
		[System.Diagnostics.DebuggerNonUserCode] public NumberContext number() {
			return GetRuleContext<NumberContext>(0);
		}
		public ExpNumberContext(ExpContext context) { CopyFrom(context); }
		[System.Diagnostics.DebuggerNonUserCode]
		public override TResult Accept<TResult>(IParseTreeVisitor<TResult> visitor) {
			ILuaVisitor<TResult> typedVisitor = visitor as ILuaVisitor<TResult>;
			if (typedVisitor != null) return typedVisitor.VisitExpNumber(this);
			else return visitor.VisitChildren(this);
		}
	}
	public partial class ExpTrueContext : ExpContext {
		public ExpTrueContext(ExpContext context) { CopyFrom(context); }
		[System.Diagnostics.DebuggerNonUserCode]
		public override TResult Accept<TResult>(IParseTreeVisitor<TResult> visitor) {
			ILuaVisitor<TResult> typedVisitor = visitor as ILuaVisitor<TResult>;
			if (typedVisitor != null) return typedVisitor.VisitExpTrue(this);
			else return visitor.VisitChildren(this);
		}
	}
	public partial class ExpOrContext : ExpContext {
		[System.Diagnostics.DebuggerNonUserCode] public ExpContext[] exp() {
			return GetRuleContexts<ExpContext>();
		}
		[System.Diagnostics.DebuggerNonUserCode] public ExpContext exp(int i) {
			return GetRuleContext<ExpContext>(i);
		}
		[System.Diagnostics.DebuggerNonUserCode] public OperatorOrContext operatorOr() {
			return GetRuleContext<OperatorOrContext>(0);
		}
		public ExpOrContext(ExpContext context) { CopyFrom(context); }
		[System.Diagnostics.DebuggerNonUserCode]
		public override TResult Accept<TResult>(IParseTreeVisitor<TResult> visitor) {
			ILuaVisitor<TResult> typedVisitor = visitor as ILuaVisitor<TResult>;
			if (typedVisitor != null) return typedVisitor.VisitExpOr(this);
			else return visitor.VisitChildren(this);
		}
	}
	public partial class ExpBitwiseContext : ExpContext {
		[System.Diagnostics.DebuggerNonUserCode] public ExpContext[] exp() {
			return GetRuleContexts<ExpContext>();
		}
		[System.Diagnostics.DebuggerNonUserCode] public ExpContext exp(int i) {
			return GetRuleContext<ExpContext>(i);
		}
		[System.Diagnostics.DebuggerNonUserCode] public OperatorBitwiseContext operatorBitwise() {
			return GetRuleContext<OperatorBitwiseContext>(0);
		}
		public ExpBitwiseContext(ExpContext context) { CopyFrom(context); }
		[System.Diagnostics.DebuggerNonUserCode]
		public override TResult Accept<TResult>(IParseTreeVisitor<TResult> visitor) {
			ILuaVisitor<TResult> typedVisitor = visitor as ILuaVisitor<TResult>;
			if (typedVisitor != null) return typedVisitor.VisitExpBitwise(this);
			else return visitor.VisitChildren(this);
		}
	}
	public partial class ExpMulDivModContext : ExpContext {
		[System.Diagnostics.DebuggerNonUserCode] public ExpContext[] exp() {
			return GetRuleContexts<ExpContext>();
		}
		[System.Diagnostics.DebuggerNonUserCode] public ExpContext exp(int i) {
			return GetRuleContext<ExpContext>(i);
		}
		[System.Diagnostics.DebuggerNonUserCode] public OperatorMulDivModContext operatorMulDivMod() {
			return GetRuleContext<OperatorMulDivModContext>(0);
		}
		public ExpMulDivModContext(ExpContext context) { CopyFrom(context); }
		[System.Diagnostics.DebuggerNonUserCode]
		public override TResult Accept<TResult>(IParseTreeVisitor<TResult> visitor) {
			ILuaVisitor<TResult> typedVisitor = visitor as ILuaVisitor<TResult>;
			if (typedVisitor != null) return typedVisitor.VisitExpMulDivMod(this);
			else return visitor.VisitChildren(this);
		}
	}
	public partial class ExpFalseContext : ExpContext {
		public ExpFalseContext(ExpContext context) { CopyFrom(context); }
		[System.Diagnostics.DebuggerNonUserCode]
		public override TResult Accept<TResult>(IParseTreeVisitor<TResult> visitor) {
			ILuaVisitor<TResult> typedVisitor = visitor as ILuaVisitor<TResult>;
			if (typedVisitor != null) return typedVisitor.VisitExpFalse(this);
			else return visitor.VisitChildren(this);
		}
	}
	public partial class ExpStringContext : ExpContext {
		[System.Diagnostics.DebuggerNonUserCode] public StringContext @string() {
			return GetRuleContext<StringContext>(0);
		}
		public ExpStringContext(ExpContext context) { CopyFrom(context); }
		[System.Diagnostics.DebuggerNonUserCode]
		public override TResult Accept<TResult>(IParseTreeVisitor<TResult> visitor) {
			ILuaVisitor<TResult> typedVisitor = visitor as ILuaVisitor<TResult>;
			if (typedVisitor != null) return typedVisitor.VisitExpString(this);
			else return visitor.VisitChildren(this);
		}
	}
	public partial class ExpPrefixContext : ExpContext {
		[System.Diagnostics.DebuggerNonUserCode] public PrefixexpContext prefixexp() {
			return GetRuleContext<PrefixexpContext>(0);
		}
		public ExpPrefixContext(ExpContext context) { CopyFrom(context); }
		[System.Diagnostics.DebuggerNonUserCode]
		public override TResult Accept<TResult>(IParseTreeVisitor<TResult> visitor) {
			ILuaVisitor<TResult> typedVisitor = visitor as ILuaVisitor<TResult>;
			if (typedVisitor != null) return typedVisitor.VisitExpPrefix(this);
			else return visitor.VisitChildren(this);
		}
	}
	public partial class ExpUnaryContext : ExpContext {
		[System.Diagnostics.DebuggerNonUserCode] public OperatorUnaryContext operatorUnary() {
			return GetRuleContext<OperatorUnaryContext>(0);
		}
		[System.Diagnostics.DebuggerNonUserCode] public ExpContext exp() {
			return GetRuleContext<ExpContext>(0);
		}
		public ExpUnaryContext(ExpContext context) { CopyFrom(context); }
		[System.Diagnostics.DebuggerNonUserCode]
		public override TResult Accept<TResult>(IParseTreeVisitor<TResult> visitor) {
			ILuaVisitor<TResult> typedVisitor = visitor as ILuaVisitor<TResult>;
			if (typedVisitor != null) return typedVisitor.VisitExpUnary(this);
			else return visitor.VisitChildren(this);
		}
	}
	public partial class ExpAndContext : ExpContext {
		[System.Diagnostics.DebuggerNonUserCode] public ExpContext[] exp() {
			return GetRuleContexts<ExpContext>();
		}
		[System.Diagnostics.DebuggerNonUserCode] public ExpContext exp(int i) {
			return GetRuleContext<ExpContext>(i);
		}
		[System.Diagnostics.DebuggerNonUserCode] public OperatorAndContext operatorAnd() {
			return GetRuleContext<OperatorAndContext>(0);
		}
		public ExpAndContext(ExpContext context) { CopyFrom(context); }
		[System.Diagnostics.DebuggerNonUserCode]
		public override TResult Accept<TResult>(IParseTreeVisitor<TResult> visitor) {
			ILuaVisitor<TResult> typedVisitor = visitor as ILuaVisitor<TResult>;
			if (typedVisitor != null) return typedVisitor.VisitExpAnd(this);
			else return visitor.VisitChildren(this);
		}
	}
	public partial class ExpPowContext : ExpContext {
		[System.Diagnostics.DebuggerNonUserCode] public ExpContext[] exp() {
			return GetRuleContexts<ExpContext>();
		}
		[System.Diagnostics.DebuggerNonUserCode] public ExpContext exp(int i) {
			return GetRuleContext<ExpContext>(i);
		}
		[System.Diagnostics.DebuggerNonUserCode] public OperatorPowerContext operatorPower() {
			return GetRuleContext<OperatorPowerContext>(0);
		}
		public ExpPowContext(ExpContext context) { CopyFrom(context); }
		[System.Diagnostics.DebuggerNonUserCode]
		public override TResult Accept<TResult>(IParseTreeVisitor<TResult> visitor) {
			ILuaVisitor<TResult> typedVisitor = visitor as ILuaVisitor<TResult>;
			if (typedVisitor != null) return typedVisitor.VisitExpPow(this);
			else return visitor.VisitChildren(this);
		}
	}
	public partial class ExpFunctionContext : ExpContext {
		[System.Diagnostics.DebuggerNonUserCode] public FunctiondefContext functiondef() {
			return GetRuleContext<FunctiondefContext>(0);
		}
		public ExpFunctionContext(ExpContext context) { CopyFrom(context); }
		[System.Diagnostics.DebuggerNonUserCode]
		public override TResult Accept<TResult>(IParseTreeVisitor<TResult> visitor) {
			ILuaVisitor<TResult> typedVisitor = visitor as ILuaVisitor<TResult>;
			if (typedVisitor != null) return typedVisitor.VisitExpFunction(this);
			else return visitor.VisitChildren(this);
		}
	}
	public partial class ExpCompareContext : ExpContext {
		[System.Diagnostics.DebuggerNonUserCode] public ExpContext[] exp() {
			return GetRuleContexts<ExpContext>();
		}
		[System.Diagnostics.DebuggerNonUserCode] public ExpContext exp(int i) {
			return GetRuleContext<ExpContext>(i);
		}
		[System.Diagnostics.DebuggerNonUserCode] public OperatorComparisonContext operatorComparison() {
			return GetRuleContext<OperatorComparisonContext>(0);
		}
		public ExpCompareContext(ExpContext context) { CopyFrom(context); }
		[System.Diagnostics.DebuggerNonUserCode]
		public override TResult Accept<TResult>(IParseTreeVisitor<TResult> visitor) {
			ILuaVisitor<TResult> typedVisitor = visitor as ILuaVisitor<TResult>;
			if (typedVisitor != null) return typedVisitor.VisitExpCompare(this);
			else return visitor.VisitChildren(this);
		}
	}
	public partial class ExpConcatContext : ExpContext {
		[System.Diagnostics.DebuggerNonUserCode] public ExpContext[] exp() {
			return GetRuleContexts<ExpContext>();
		}
		[System.Diagnostics.DebuggerNonUserCode] public ExpContext exp(int i) {
			return GetRuleContext<ExpContext>(i);
		}
		[System.Diagnostics.DebuggerNonUserCode] public OperatorStrcatContext operatorStrcat() {
			return GetRuleContext<OperatorStrcatContext>(0);
		}
		public ExpConcatContext(ExpContext context) { CopyFrom(context); }
		[System.Diagnostics.DebuggerNonUserCode]
		public override TResult Accept<TResult>(IParseTreeVisitor<TResult> visitor) {
			ILuaVisitor<TResult> typedVisitor = visitor as ILuaVisitor<TResult>;
			if (typedVisitor != null) return typedVisitor.VisitExpConcat(this);
			else return visitor.VisitChildren(this);
		}
	}
	public partial class ExpNilContext : ExpContext {
		public ExpNilContext(ExpContext context) { CopyFrom(context); }
		[System.Diagnostics.DebuggerNonUserCode]
		public override TResult Accept<TResult>(IParseTreeVisitor<TResult> visitor) {
			ILuaVisitor<TResult> typedVisitor = visitor as ILuaVisitor<TResult>;
			if (typedVisitor != null) return typedVisitor.VisitExpNil(this);
			else return visitor.VisitChildren(this);
		}
	}
	public partial class ExpAddSubContext : ExpContext {
		[System.Diagnostics.DebuggerNonUserCode] public ExpContext[] exp() {
			return GetRuleContexts<ExpContext>();
		}
		[System.Diagnostics.DebuggerNonUserCode] public ExpContext exp(int i) {
			return GetRuleContext<ExpContext>(i);
		}
		[System.Diagnostics.DebuggerNonUserCode] public OperatorAddSubContext operatorAddSub() {
			return GetRuleContext<OperatorAddSubContext>(0);
		}
		public ExpAddSubContext(ExpContext context) { CopyFrom(context); }
		[System.Diagnostics.DebuggerNonUserCode]
		public override TResult Accept<TResult>(IParseTreeVisitor<TResult> visitor) {
			ILuaVisitor<TResult> typedVisitor = visitor as ILuaVisitor<TResult>;
			if (typedVisitor != null) return typedVisitor.VisitExpAddSub(this);
			else return visitor.VisitChildren(this);
		}
	}
	public partial class ExpVarArgContext : ExpContext {
		public ExpVarArgContext(ExpContext context) { CopyFrom(context); }
		[System.Diagnostics.DebuggerNonUserCode]
		public override TResult Accept<TResult>(IParseTreeVisitor<TResult> visitor) {
			ILuaVisitor<TResult> typedVisitor = visitor as ILuaVisitor<TResult>;
			if (typedVisitor != null) return typedVisitor.VisitExpVarArg(this);
			else return visitor.VisitChildren(this);
		}
	}
	public partial class ExpTableContext : ExpContext {
		[System.Diagnostics.DebuggerNonUserCode] public TableconstructorContext tableconstructor() {
			return GetRuleContext<TableconstructorContext>(0);
		}
		public ExpTableContext(ExpContext context) { CopyFrom(context); }
		[System.Diagnostics.DebuggerNonUserCode]
		public override TResult Accept<TResult>(IParseTreeVisitor<TResult> visitor) {
			ILuaVisitor<TResult> typedVisitor = visitor as ILuaVisitor<TResult>;
			if (typedVisitor != null) return typedVisitor.VisitExpTable(this);
			else return visitor.VisitChildren(this);
		}
	}

	[RuleVersion(0)]
	public ExpContext exp() {
		return exp(0);
	}

	private ExpContext exp(int _p) {
		ParserRuleContext _parentctx = Context;
		int _parentState = State;
		ExpContext _localctx = new ExpContext(Context, _parentState);
		ExpContext _prevctx = _localctx;
		int _startState = 24;
		EnterRecursionRule(_localctx, 24, RULE_exp, _p);
		try {
			int _alt;
			EnterOuterAlt(_localctx, 1);
			{
			State = 235;
			ErrorHandler.Sync(this);
			switch (TokenStream.LA(1)) {
			case T__22:
				{
				_localctx = new ExpNilContext(_localctx);
				Context = _localctx;
				_prevctx = _localctx;

				State = 223;
				Match(T__22);
				}
				break;
			case T__23:
				{
				_localctx = new ExpFalseContext(_localctx);
				Context = _localctx;
				_prevctx = _localctx;
				State = 224;
				Match(T__23);
				}
				break;
			case T__24:
				{
				_localctx = new ExpTrueContext(_localctx);
				Context = _localctx;
				_prevctx = _localctx;
				State = 225;
				Match(T__24);
				}
				break;
			case INT:
			case HEX:
			case FLOAT:
			case HEX_FLOAT:
				{
				_localctx = new ExpNumberContext(_localctx);
				Context = _localctx;
				_prevctx = _localctx;
				State = 226;
				number();
				}
				break;
			case NORMALSTRING:
			case CHARSTRING:
			case LONGSTRING:
				{
				_localctx = new ExpStringContext(_localctx);
				Context = _localctx;
				_prevctx = _localctx;
				State = 227;
				@string();
				}
				break;
			case T__25:
				{
				_localctx = new ExpVarArgContext(_localctx);
				Context = _localctx;
				_prevctx = _localctx;
				State = 228;
				Match(T__25);
				}
				break;
			case T__12:
				{
				_localctx = new ExpFunctionContext(_localctx);
				Context = _localctx;
				_prevctx = _localctx;
				State = 229;
				functiondef();
				}
				break;
			case T__26:
			case NAME:
				{
				_localctx = new ExpPrefixContext(_localctx);
				Context = _localctx;
				_prevctx = _localctx;
				State = 230;
				prefixexp();
				}
				break;
			case T__30:
				{
				_localctx = new ExpTableContext(_localctx);
				Context = _localctx;
				_prevctx = _localctx;
				State = 231;
				tableconstructor();
				}
				break;
			case T__42:
			case T__49:
			case T__52:
			case T__53:
				{
				_localctx = new ExpUnaryContext(_localctx);
				Context = _localctx;
				_prevctx = _localctx;
				State = 232;
				operatorUnary();
				State = 233;
				exp(8);
				}
				break;
			default:
				throw new NoViableAltException(this);
			}
			Context.Stop = TokenStream.LT(-1);
			State = 271;
			ErrorHandler.Sync(this);
			_alt = Interpreter.AdaptivePredict(TokenStream,16,Context);
			while ( _alt!=2 && _alt!=global::Antlr4.Runtime.Atn.ATN.INVALID_ALT_NUMBER ) {
				if ( _alt==1 ) {
					if ( ParseListeners!=null )
						TriggerExitRuleEvent();
					_prevctx = _localctx;
					{
					State = 269;
					ErrorHandler.Sync(this);
					switch ( Interpreter.AdaptivePredict(TokenStream,15,Context) ) {
					case 1:
						{
						_localctx = new ExpPowContext(new ExpContext(_parentctx, _parentState));
						PushNewRecursionContext(_localctx, _startState, RULE_exp);
						State = 237;
						if (!(Precpred(Context, 9))) throw new FailedPredicateException(this, "Precpred(Context, 9)");
						State = 238;
						operatorPower();
						State = 239;
						exp(9);
						}
						break;
					case 2:
						{
						_localctx = new ExpMulDivModContext(new ExpContext(_parentctx, _parentState));
						PushNewRecursionContext(_localctx, _startState, RULE_exp);
						State = 241;
						if (!(Precpred(Context, 7))) throw new FailedPredicateException(this, "Precpred(Context, 7)");
						State = 242;
						operatorMulDivMod();
						State = 243;
						exp(8);
						}
						break;
					case 3:
						{
						_localctx = new ExpAddSubContext(new ExpContext(_parentctx, _parentState));
						PushNewRecursionContext(_localctx, _startState, RULE_exp);
						State = 245;
						if (!(Precpred(Context, 6))) throw new FailedPredicateException(this, "Precpred(Context, 6)");
						State = 246;
						operatorAddSub();
						State = 247;
						exp(7);
						}
						break;
					case 4:
						{
						_localctx = new ExpConcatContext(new ExpContext(_parentctx, _parentState));
						PushNewRecursionContext(_localctx, _startState, RULE_exp);
						State = 249;
						if (!(Precpred(Context, 5))) throw new FailedPredicateException(this, "Precpred(Context, 5)");
						State = 250;
						operatorStrcat();
						State = 251;
						exp(5);
						}
						break;
					case 5:
						{
						_localctx = new ExpCompareContext(new ExpContext(_parentctx, _parentState));
						PushNewRecursionContext(_localctx, _startState, RULE_exp);
						State = 253;
						if (!(Precpred(Context, 4))) throw new FailedPredicateException(this, "Precpred(Context, 4)");
						State = 254;
						operatorComparison();
						State = 255;
						exp(5);
						}
						break;
					case 6:
						{
						_localctx = new ExpAndContext(new ExpContext(_parentctx, _parentState));
						PushNewRecursionContext(_localctx, _startState, RULE_exp);
						State = 257;
						if (!(Precpred(Context, 3))) throw new FailedPredicateException(this, "Precpred(Context, 3)");
						State = 258;
						operatorAnd();
						State = 259;
						exp(4);
						}
						break;
					case 7:
						{
						_localctx = new ExpOrContext(new ExpContext(_parentctx, _parentState));
						PushNewRecursionContext(_localctx, _startState, RULE_exp);
						State = 261;
						if (!(Precpred(Context, 2))) throw new FailedPredicateException(this, "Precpred(Context, 2)");
						State = 262;
						operatorOr();
						State = 263;
						exp(3);
						}
						break;
					case 8:
						{
						_localctx = new ExpBitwiseContext(new ExpContext(_parentctx, _parentState));
						PushNewRecursionContext(_localctx, _startState, RULE_exp);
						State = 265;
						if (!(Precpred(Context, 1))) throw new FailedPredicateException(this, "Precpred(Context, 1)");
						State = 266;
						operatorBitwise();
						State = 267;
						exp(2);
						}
						break;
					}
					} 
				}
				State = 273;
				ErrorHandler.Sync(this);
				_alt = Interpreter.AdaptivePredict(TokenStream,16,Context);
			}
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			ErrorHandler.ReportError(this, re);
			ErrorHandler.Recover(this, re);
		}
		finally {
			UnrollRecursionContexts(_parentctx);
		}
		return _localctx;
	}

	public partial class PrefixexpContext : ParserRuleContext {
		[System.Diagnostics.DebuggerNonUserCode] public VarOrExpContext varOrExp() {
			return GetRuleContext<VarOrExpContext>(0);
		}
		[System.Diagnostics.DebuggerNonUserCode] public NameAndArgsContext[] nameAndArgs() {
			return GetRuleContexts<NameAndArgsContext>();
		}
		[System.Diagnostics.DebuggerNonUserCode] public NameAndArgsContext nameAndArgs(int i) {
			return GetRuleContext<NameAndArgsContext>(i);
		}
		public PrefixexpContext(ParserRuleContext parent, int invokingState)
			: base(parent, invokingState)
		{
		}
		public override int RuleIndex { get { return RULE_prefixexp; } }
		[System.Diagnostics.DebuggerNonUserCode]
		public override TResult Accept<TResult>(IParseTreeVisitor<TResult> visitor) {
			ILuaVisitor<TResult> typedVisitor = visitor as ILuaVisitor<TResult>;
			if (typedVisitor != null) return typedVisitor.VisitPrefixexp(this);
			else return visitor.VisitChildren(this);
		}
	}

	[RuleVersion(0)]
	public PrefixexpContext prefixexp() {
		PrefixexpContext _localctx = new PrefixexpContext(Context, State);
		EnterRule(_localctx, 26, RULE_prefixexp);
		try {
			int _alt;
			EnterOuterAlt(_localctx, 1);
			{
			State = 274;
			varOrExp();
			State = 278;
			ErrorHandler.Sync(this);
			_alt = Interpreter.AdaptivePredict(TokenStream,17,Context);
			while ( _alt!=2 && _alt!=global::Antlr4.Runtime.Atn.ATN.INVALID_ALT_NUMBER ) {
				if ( _alt==1 ) {
					{
					{
					State = 275;
					nameAndArgs();
					}
					} 
				}
				State = 280;
				ErrorHandler.Sync(this);
				_alt = Interpreter.AdaptivePredict(TokenStream,17,Context);
			}
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			ErrorHandler.ReportError(this, re);
			ErrorHandler.Recover(this, re);
		}
		finally {
			ExitRule();
		}
		return _localctx;
	}

	public partial class FunctioncallContext : ParserRuleContext {
		[System.Diagnostics.DebuggerNonUserCode] public VarOrExpContext varOrExp() {
			return GetRuleContext<VarOrExpContext>(0);
		}
		[System.Diagnostics.DebuggerNonUserCode] public NameAndArgsContext[] nameAndArgs() {
			return GetRuleContexts<NameAndArgsContext>();
		}
		[System.Diagnostics.DebuggerNonUserCode] public NameAndArgsContext nameAndArgs(int i) {
			return GetRuleContext<NameAndArgsContext>(i);
		}
		public FunctioncallContext(ParserRuleContext parent, int invokingState)
			: base(parent, invokingState)
		{
		}
		public override int RuleIndex { get { return RULE_functioncall; } }
		[System.Diagnostics.DebuggerNonUserCode]
		public override TResult Accept<TResult>(IParseTreeVisitor<TResult> visitor) {
			ILuaVisitor<TResult> typedVisitor = visitor as ILuaVisitor<TResult>;
			if (typedVisitor != null) return typedVisitor.VisitFunctioncall(this);
			else return visitor.VisitChildren(this);
		}
	}

	[RuleVersion(0)]
	public FunctioncallContext functioncall() {
		FunctioncallContext _localctx = new FunctioncallContext(Context, State);
		EnterRule(_localctx, 28, RULE_functioncall);
		try {
			int _alt;
			EnterOuterAlt(_localctx, 1);
			{
			State = 281;
			varOrExp();
			State = 283;
			ErrorHandler.Sync(this);
			_alt = 1;
			do {
				switch (_alt) {
				case 1:
					{
					{
					State = 282;
					nameAndArgs();
					}
					}
					break;
				default:
					throw new NoViableAltException(this);
				}
				State = 285;
				ErrorHandler.Sync(this);
				_alt = Interpreter.AdaptivePredict(TokenStream,18,Context);
			} while ( _alt!=2 && _alt!=global::Antlr4.Runtime.Atn.ATN.INVALID_ALT_NUMBER );
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			ErrorHandler.ReportError(this, re);
			ErrorHandler.Recover(this, re);
		}
		finally {
			ExitRule();
		}
		return _localctx;
	}

	public partial class VarOrExpContext : ParserRuleContext {
		[System.Diagnostics.DebuggerNonUserCode] public VarContext var() {
			return GetRuleContext<VarContext>(0);
		}
		[System.Diagnostics.DebuggerNonUserCode] public ExpContext exp() {
			return GetRuleContext<ExpContext>(0);
		}
		public VarOrExpContext(ParserRuleContext parent, int invokingState)
			: base(parent, invokingState)
		{
		}
		public override int RuleIndex { get { return RULE_varOrExp; } }
		[System.Diagnostics.DebuggerNonUserCode]
		public override TResult Accept<TResult>(IParseTreeVisitor<TResult> visitor) {
			ILuaVisitor<TResult> typedVisitor = visitor as ILuaVisitor<TResult>;
			if (typedVisitor != null) return typedVisitor.VisitVarOrExp(this);
			else return visitor.VisitChildren(this);
		}
	}

	[RuleVersion(0)]
	public VarOrExpContext varOrExp() {
		VarOrExpContext _localctx = new VarOrExpContext(Context, State);
		EnterRule(_localctx, 30, RULE_varOrExp);
		try {
			State = 292;
			ErrorHandler.Sync(this);
			switch ( Interpreter.AdaptivePredict(TokenStream,19,Context) ) {
			case 1:
				EnterOuterAlt(_localctx, 1);
				{
				State = 287;
				var();
				}
				break;
			case 2:
				EnterOuterAlt(_localctx, 2);
				{
				State = 288;
				Match(T__26);
				State = 289;
				exp(0);
				State = 290;
				Match(T__27);
				}
				break;
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			ErrorHandler.ReportError(this, re);
			ErrorHandler.Recover(this, re);
		}
		finally {
			ExitRule();
		}
		return _localctx;
	}

	public partial class VarContext : ParserRuleContext {
		[System.Diagnostics.DebuggerNonUserCode] public ITerminalNode NAME() { return GetToken(LuaParser.NAME, 0); }
		[System.Diagnostics.DebuggerNonUserCode] public ExpContext exp() {
			return GetRuleContext<ExpContext>(0);
		}
		[System.Diagnostics.DebuggerNonUserCode] public VarSuffixContext[] varSuffix() {
			return GetRuleContexts<VarSuffixContext>();
		}
		[System.Diagnostics.DebuggerNonUserCode] public VarSuffixContext varSuffix(int i) {
			return GetRuleContext<VarSuffixContext>(i);
		}
		public VarContext(ParserRuleContext parent, int invokingState)
			: base(parent, invokingState)
		{
		}
		public override int RuleIndex { get { return RULE_var; } }
		[System.Diagnostics.DebuggerNonUserCode]
		public override TResult Accept<TResult>(IParseTreeVisitor<TResult> visitor) {
			ILuaVisitor<TResult> typedVisitor = visitor as ILuaVisitor<TResult>;
			if (typedVisitor != null) return typedVisitor.VisitVar(this);
			else return visitor.VisitChildren(this);
		}
	}

	[RuleVersion(0)]
	public VarContext var() {
		VarContext _localctx = new VarContext(Context, State);
		EnterRule(_localctx, 32, RULE_var);
		try {
			int _alt;
			EnterOuterAlt(_localctx, 1);
			{
			State = 300;
			ErrorHandler.Sync(this);
			switch (TokenStream.LA(1)) {
			case NAME:
				{
				State = 294;
				Match(NAME);
				}
				break;
			case T__26:
				{
				State = 295;
				Match(T__26);
				State = 296;
				exp(0);
				State = 297;
				Match(T__27);
				State = 298;
				varSuffix();
				}
				break;
			default:
				throw new NoViableAltException(this);
			}
			State = 305;
			ErrorHandler.Sync(this);
			_alt = Interpreter.AdaptivePredict(TokenStream,21,Context);
			while ( _alt!=2 && _alt!=global::Antlr4.Runtime.Atn.ATN.INVALID_ALT_NUMBER ) {
				if ( _alt==1 ) {
					{
					{
					State = 302;
					varSuffix();
					}
					} 
				}
				State = 307;
				ErrorHandler.Sync(this);
				_alt = Interpreter.AdaptivePredict(TokenStream,21,Context);
			}
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			ErrorHandler.ReportError(this, re);
			ErrorHandler.Recover(this, re);
		}
		finally {
			ExitRule();
		}
		return _localctx;
	}

	public partial class VarSuffixContext : ParserRuleContext {
		[System.Diagnostics.DebuggerNonUserCode] public ExpContext exp() {
			return GetRuleContext<ExpContext>(0);
		}
		[System.Diagnostics.DebuggerNonUserCode] public ITerminalNode NAME() { return GetToken(LuaParser.NAME, 0); }
		public VarSuffixContext(ParserRuleContext parent, int invokingState)
			: base(parent, invokingState)
		{
		}
		public override int RuleIndex { get { return RULE_varSuffix; } }
		[System.Diagnostics.DebuggerNonUserCode]
		public override TResult Accept<TResult>(IParseTreeVisitor<TResult> visitor) {
			ILuaVisitor<TResult> typedVisitor = visitor as ILuaVisitor<TResult>;
			if (typedVisitor != null) return typedVisitor.VisitVarSuffix(this);
			else return visitor.VisitChildren(this);
		}
	}

	[RuleVersion(0)]
	public VarSuffixContext varSuffix() {
		VarSuffixContext _localctx = new VarSuffixContext(Context, State);
		EnterRule(_localctx, 34, RULE_varSuffix);
		try {
			State = 314;
			ErrorHandler.Sync(this);
			switch (TokenStream.LA(1)) {
			case T__28:
				EnterOuterAlt(_localctx, 1);
				{
				State = 308;
				Match(T__28);
				State = 309;
				exp(0);
				State = 310;
				Match(T__29);
				}
				break;
			case T__20:
				EnterOuterAlt(_localctx, 2);
				{
				State = 312;
				Match(T__20);
				State = 313;
				Match(NAME);
				}
				break;
			default:
				throw new NoViableAltException(this);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			ErrorHandler.ReportError(this, re);
			ErrorHandler.Recover(this, re);
		}
		finally {
			ExitRule();
		}
		return _localctx;
	}

	public partial class NameAndArgsContext : ParserRuleContext {
		[System.Diagnostics.DebuggerNonUserCode] public ArgsContext args() {
			return GetRuleContext<ArgsContext>(0);
		}
		[System.Diagnostics.DebuggerNonUserCode] public ITerminalNode NAME() { return GetToken(LuaParser.NAME, 0); }
		public NameAndArgsContext(ParserRuleContext parent, int invokingState)
			: base(parent, invokingState)
		{
		}
		public override int RuleIndex { get { return RULE_nameAndArgs; } }
		[System.Diagnostics.DebuggerNonUserCode]
		public override TResult Accept<TResult>(IParseTreeVisitor<TResult> visitor) {
			ILuaVisitor<TResult> typedVisitor = visitor as ILuaVisitor<TResult>;
			if (typedVisitor != null) return typedVisitor.VisitNameAndArgs(this);
			else return visitor.VisitChildren(this);
		}
	}

	[RuleVersion(0)]
	public NameAndArgsContext nameAndArgs() {
		NameAndArgsContext _localctx = new NameAndArgsContext(Context, State);
		EnterRule(_localctx, 36, RULE_nameAndArgs);
		int _la;
		try {
			EnterOuterAlt(_localctx, 1);
			{
			State = 318;
			ErrorHandler.Sync(this);
			_la = TokenStream.LA(1);
			if (_la==T__21) {
				{
				State = 316;
				Match(T__21);
				State = 317;
				Match(NAME);
				}
			}

			State = 320;
			args();
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			ErrorHandler.ReportError(this, re);
			ErrorHandler.Recover(this, re);
		}
		finally {
			ExitRule();
		}
		return _localctx;
	}

	public partial class ArgsContext : ParserRuleContext {
		[System.Diagnostics.DebuggerNonUserCode] public ExplistContext explist() {
			return GetRuleContext<ExplistContext>(0);
		}
		[System.Diagnostics.DebuggerNonUserCode] public TableconstructorContext tableconstructor() {
			return GetRuleContext<TableconstructorContext>(0);
		}
		[System.Diagnostics.DebuggerNonUserCode] public StringContext @string() {
			return GetRuleContext<StringContext>(0);
		}
		public ArgsContext(ParserRuleContext parent, int invokingState)
			: base(parent, invokingState)
		{
		}
		public override int RuleIndex { get { return RULE_args; } }
		[System.Diagnostics.DebuggerNonUserCode]
		public override TResult Accept<TResult>(IParseTreeVisitor<TResult> visitor) {
			ILuaVisitor<TResult> typedVisitor = visitor as ILuaVisitor<TResult>;
			if (typedVisitor != null) return typedVisitor.VisitArgs(this);
			else return visitor.VisitChildren(this);
		}
	}

	[RuleVersion(0)]
	public ArgsContext args() {
		ArgsContext _localctx = new ArgsContext(Context, State);
		EnterRule(_localctx, 38, RULE_args);
		int _la;
		try {
			State = 329;
			ErrorHandler.Sync(this);
			switch (TokenStream.LA(1)) {
			case T__26:
				EnterOuterAlt(_localctx, 1);
				{
				State = 322;
				Match(T__26);
				State = 324;
				ErrorHandler.Sync(this);
				_la = TokenStream.LA(1);
				if ((((_la) & ~0x3f) == 0 && ((1L << _la) & -43901297866301440L) != 0)) {
					{
					State = 323;
					explist();
					}
				}

				State = 326;
				Match(T__27);
				}
				break;
			case T__30:
				EnterOuterAlt(_localctx, 2);
				{
				State = 327;
				tableconstructor();
				}
				break;
			case NORMALSTRING:
			case CHARSTRING:
			case LONGSTRING:
				EnterOuterAlt(_localctx, 3);
				{
				State = 328;
				@string();
				}
				break;
			default:
				throw new NoViableAltException(this);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			ErrorHandler.ReportError(this, re);
			ErrorHandler.Recover(this, re);
		}
		finally {
			ExitRule();
		}
		return _localctx;
	}

	public partial class FunctiondefContext : ParserRuleContext {
		[System.Diagnostics.DebuggerNonUserCode] public FuncbodyContext funcbody() {
			return GetRuleContext<FuncbodyContext>(0);
		}
		public FunctiondefContext(ParserRuleContext parent, int invokingState)
			: base(parent, invokingState)
		{
		}
		public override int RuleIndex { get { return RULE_functiondef; } }
		[System.Diagnostics.DebuggerNonUserCode]
		public override TResult Accept<TResult>(IParseTreeVisitor<TResult> visitor) {
			ILuaVisitor<TResult> typedVisitor = visitor as ILuaVisitor<TResult>;
			if (typedVisitor != null) return typedVisitor.VisitFunctiondef(this);
			else return visitor.VisitChildren(this);
		}
	}

	[RuleVersion(0)]
	public FunctiondefContext functiondef() {
		FunctiondefContext _localctx = new FunctiondefContext(Context, State);
		EnterRule(_localctx, 40, RULE_functiondef);
		try {
			EnterOuterAlt(_localctx, 1);
			{
			State = 331;
			Match(T__12);
			State = 332;
			funcbody();
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			ErrorHandler.ReportError(this, re);
			ErrorHandler.Recover(this, re);
		}
		finally {
			ExitRule();
		}
		return _localctx;
	}

	public partial class FuncbodyContext : ParserRuleContext {
		[System.Diagnostics.DebuggerNonUserCode] public BlockContext block() {
			return GetRuleContext<BlockContext>(0);
		}
		[System.Diagnostics.DebuggerNonUserCode] public ParlistContext parlist() {
			return GetRuleContext<ParlistContext>(0);
		}
		public FuncbodyContext(ParserRuleContext parent, int invokingState)
			: base(parent, invokingState)
		{
		}
		public override int RuleIndex { get { return RULE_funcbody; } }
		[System.Diagnostics.DebuggerNonUserCode]
		public override TResult Accept<TResult>(IParseTreeVisitor<TResult> visitor) {
			ILuaVisitor<TResult> typedVisitor = visitor as ILuaVisitor<TResult>;
			if (typedVisitor != null) return typedVisitor.VisitFuncbody(this);
			else return visitor.VisitChildren(this);
		}
	}

	[RuleVersion(0)]
	public FuncbodyContext funcbody() {
		FuncbodyContext _localctx = new FuncbodyContext(Context, State);
		EnterRule(_localctx, 42, RULE_funcbody);
		int _la;
		try {
			EnterOuterAlt(_localctx, 1);
			{
			State = 334;
			Match(T__26);
			State = 336;
			ErrorHandler.Sync(this);
			_la = TokenStream.LA(1);
			if (_la==T__25 || _la==NAME) {
				{
				State = 335;
				parlist();
				}
			}

			State = 338;
			Match(T__27);
			State = 339;
			block();
			State = 340;
			Match(T__5);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			ErrorHandler.ReportError(this, re);
			ErrorHandler.Recover(this, re);
		}
		finally {
			ExitRule();
		}
		return _localctx;
	}

	public partial class ParlistContext : ParserRuleContext {
		[System.Diagnostics.DebuggerNonUserCode] public NamelistContext namelist() {
			return GetRuleContext<NamelistContext>(0);
		}
		public ParlistContext(ParserRuleContext parent, int invokingState)
			: base(parent, invokingState)
		{
		}
		public override int RuleIndex { get { return RULE_parlist; } }
		[System.Diagnostics.DebuggerNonUserCode]
		public override TResult Accept<TResult>(IParseTreeVisitor<TResult> visitor) {
			ILuaVisitor<TResult> typedVisitor = visitor as ILuaVisitor<TResult>;
			if (typedVisitor != null) return typedVisitor.VisitParlist(this);
			else return visitor.VisitChildren(this);
		}
	}

	[RuleVersion(0)]
	public ParlistContext parlist() {
		ParlistContext _localctx = new ParlistContext(Context, State);
		EnterRule(_localctx, 44, RULE_parlist);
		int _la;
		try {
			State = 348;
			ErrorHandler.Sync(this);
			switch (TokenStream.LA(1)) {
			case NAME:
				EnterOuterAlt(_localctx, 1);
				{
				State = 342;
				namelist();
				State = 345;
				ErrorHandler.Sync(this);
				_la = TokenStream.LA(1);
				if (_la==T__10) {
					{
					State = 343;
					Match(T__10);
					State = 344;
					Match(T__25);
					}
				}

				}
				break;
			case T__25:
				EnterOuterAlt(_localctx, 2);
				{
				State = 347;
				Match(T__25);
				}
				break;
			default:
				throw new NoViableAltException(this);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			ErrorHandler.ReportError(this, re);
			ErrorHandler.Recover(this, re);
		}
		finally {
			ExitRule();
		}
		return _localctx;
	}

	public partial class TableconstructorContext : ParserRuleContext {
		[System.Diagnostics.DebuggerNonUserCode] public FieldlistContext fieldlist() {
			return GetRuleContext<FieldlistContext>(0);
		}
		public TableconstructorContext(ParserRuleContext parent, int invokingState)
			: base(parent, invokingState)
		{
		}
		public override int RuleIndex { get { return RULE_tableconstructor; } }
		[System.Diagnostics.DebuggerNonUserCode]
		public override TResult Accept<TResult>(IParseTreeVisitor<TResult> visitor) {
			ILuaVisitor<TResult> typedVisitor = visitor as ILuaVisitor<TResult>;
			if (typedVisitor != null) return typedVisitor.VisitTableconstructor(this);
			else return visitor.VisitChildren(this);
		}
	}

	[RuleVersion(0)]
	public TableconstructorContext tableconstructor() {
		TableconstructorContext _localctx = new TableconstructorContext(Context, State);
		EnterRule(_localctx, 46, RULE_tableconstructor);
		int _la;
		try {
			EnterOuterAlt(_localctx, 1);
			{
			State = 350;
			Match(T__30);
			State = 352;
			ErrorHandler.Sync(this);
			_la = TokenStream.LA(1);
			if ((((_la) & ~0x3f) == 0 && ((1L << _la) & -43901297329430528L) != 0)) {
				{
				State = 351;
				fieldlist();
				}
			}

			State = 354;
			Match(T__31);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			ErrorHandler.ReportError(this, re);
			ErrorHandler.Recover(this, re);
		}
		finally {
			ExitRule();
		}
		return _localctx;
	}

	public partial class FieldlistContext : ParserRuleContext {
		[System.Diagnostics.DebuggerNonUserCode] public FieldContext[] field() {
			return GetRuleContexts<FieldContext>();
		}
		[System.Diagnostics.DebuggerNonUserCode] public FieldContext field(int i) {
			return GetRuleContext<FieldContext>(i);
		}
		[System.Diagnostics.DebuggerNonUserCode] public FieldsepContext[] fieldsep() {
			return GetRuleContexts<FieldsepContext>();
		}
		[System.Diagnostics.DebuggerNonUserCode] public FieldsepContext fieldsep(int i) {
			return GetRuleContext<FieldsepContext>(i);
		}
		public FieldlistContext(ParserRuleContext parent, int invokingState)
			: base(parent, invokingState)
		{
		}
		public override int RuleIndex { get { return RULE_fieldlist; } }
		[System.Diagnostics.DebuggerNonUserCode]
		public override TResult Accept<TResult>(IParseTreeVisitor<TResult> visitor) {
			ILuaVisitor<TResult> typedVisitor = visitor as ILuaVisitor<TResult>;
			if (typedVisitor != null) return typedVisitor.VisitFieldlist(this);
			else return visitor.VisitChildren(this);
		}
	}

	[RuleVersion(0)]
	public FieldlistContext fieldlist() {
		FieldlistContext _localctx = new FieldlistContext(Context, State);
		EnterRule(_localctx, 48, RULE_fieldlist);
		int _la;
		try {
			int _alt;
			EnterOuterAlt(_localctx, 1);
			{
			State = 356;
			field();
			State = 362;
			ErrorHandler.Sync(this);
			_alt = Interpreter.AdaptivePredict(TokenStream,30,Context);
			while ( _alt!=2 && _alt!=global::Antlr4.Runtime.Atn.ATN.INVALID_ALT_NUMBER ) {
				if ( _alt==1 ) {
					{
					{
					State = 357;
					fieldsep();
					State = 358;
					field();
					}
					} 
				}
				State = 364;
				ErrorHandler.Sync(this);
				_alt = Interpreter.AdaptivePredict(TokenStream,30,Context);
			}
			State = 366;
			ErrorHandler.Sync(this);
			_la = TokenStream.LA(1);
			if (_la==T__0 || _la==T__10) {
				{
				State = 365;
				fieldsep();
				}
			}

			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			ErrorHandler.ReportError(this, re);
			ErrorHandler.Recover(this, re);
		}
		finally {
			ExitRule();
		}
		return _localctx;
	}

	public partial class FieldContext : ParserRuleContext {
		[System.Diagnostics.DebuggerNonUserCode] public ExpContext[] exp() {
			return GetRuleContexts<ExpContext>();
		}
		[System.Diagnostics.DebuggerNonUserCode] public ExpContext exp(int i) {
			return GetRuleContext<ExpContext>(i);
		}
		[System.Diagnostics.DebuggerNonUserCode] public ITerminalNode NAME() { return GetToken(LuaParser.NAME, 0); }
		public FieldContext(ParserRuleContext parent, int invokingState)
			: base(parent, invokingState)
		{
		}
		public override int RuleIndex { get { return RULE_field; } }
		[System.Diagnostics.DebuggerNonUserCode]
		public override TResult Accept<TResult>(IParseTreeVisitor<TResult> visitor) {
			ILuaVisitor<TResult> typedVisitor = visitor as ILuaVisitor<TResult>;
			if (typedVisitor != null) return typedVisitor.VisitField(this);
			else return visitor.VisitChildren(this);
		}
	}

	[RuleVersion(0)]
	public FieldContext field() {
		FieldContext _localctx = new FieldContext(Context, State);
		EnterRule(_localctx, 50, RULE_field);
		try {
			State = 378;
			ErrorHandler.Sync(this);
			switch ( Interpreter.AdaptivePredict(TokenStream,32,Context) ) {
			case 1:
				EnterOuterAlt(_localctx, 1);
				{
				State = 368;
				Match(T__28);
				State = 369;
				exp(0);
				State = 370;
				Match(T__29);
				State = 371;
				Match(T__1);
				State = 372;
				exp(0);
				}
				break;
			case 2:
				EnterOuterAlt(_localctx, 2);
				{
				State = 374;
				Match(NAME);
				State = 375;
				Match(T__1);
				State = 376;
				exp(0);
				}
				break;
			case 3:
				EnterOuterAlt(_localctx, 3);
				{
				State = 377;
				exp(0);
				}
				break;
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			ErrorHandler.ReportError(this, re);
			ErrorHandler.Recover(this, re);
		}
		finally {
			ExitRule();
		}
		return _localctx;
	}

	public partial class FieldsepContext : ParserRuleContext {
		public FieldsepContext(ParserRuleContext parent, int invokingState)
			: base(parent, invokingState)
		{
		}
		public override int RuleIndex { get { return RULE_fieldsep; } }
		[System.Diagnostics.DebuggerNonUserCode]
		public override TResult Accept<TResult>(IParseTreeVisitor<TResult> visitor) {
			ILuaVisitor<TResult> typedVisitor = visitor as ILuaVisitor<TResult>;
			if (typedVisitor != null) return typedVisitor.VisitFieldsep(this);
			else return visitor.VisitChildren(this);
		}
	}

	[RuleVersion(0)]
	public FieldsepContext fieldsep() {
		FieldsepContext _localctx = new FieldsepContext(Context, State);
		EnterRule(_localctx, 52, RULE_fieldsep);
		int _la;
		try {
			EnterOuterAlt(_localctx, 1);
			{
			State = 380;
			_la = TokenStream.LA(1);
			if ( !(_la==T__0 || _la==T__10) ) {
			ErrorHandler.RecoverInline(this);
			}
			else {
				ErrorHandler.ReportMatch(this);
			    Consume();
			}
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			ErrorHandler.ReportError(this, re);
			ErrorHandler.Recover(this, re);
		}
		finally {
			ExitRule();
		}
		return _localctx;
	}

	public partial class OperatorOrContext : ParserRuleContext {
		public OperatorOrContext(ParserRuleContext parent, int invokingState)
			: base(parent, invokingState)
		{
		}
		public override int RuleIndex { get { return RULE_operatorOr; } }
		[System.Diagnostics.DebuggerNonUserCode]
		public override TResult Accept<TResult>(IParseTreeVisitor<TResult> visitor) {
			ILuaVisitor<TResult> typedVisitor = visitor as ILuaVisitor<TResult>;
			if (typedVisitor != null) return typedVisitor.VisitOperatorOr(this);
			else return visitor.VisitChildren(this);
		}
	}

	[RuleVersion(0)]
	public OperatorOrContext operatorOr() {
		OperatorOrContext _localctx = new OperatorOrContext(Context, State);
		EnterRule(_localctx, 54, RULE_operatorOr);
		try {
			EnterOuterAlt(_localctx, 1);
			{
			State = 382;
			Match(T__32);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			ErrorHandler.ReportError(this, re);
			ErrorHandler.Recover(this, re);
		}
		finally {
			ExitRule();
		}
		return _localctx;
	}

	public partial class OperatorAndContext : ParserRuleContext {
		public OperatorAndContext(ParserRuleContext parent, int invokingState)
			: base(parent, invokingState)
		{
		}
		public override int RuleIndex { get { return RULE_operatorAnd; } }
		[System.Diagnostics.DebuggerNonUserCode]
		public override TResult Accept<TResult>(IParseTreeVisitor<TResult> visitor) {
			ILuaVisitor<TResult> typedVisitor = visitor as ILuaVisitor<TResult>;
			if (typedVisitor != null) return typedVisitor.VisitOperatorAnd(this);
			else return visitor.VisitChildren(this);
		}
	}

	[RuleVersion(0)]
	public OperatorAndContext operatorAnd() {
		OperatorAndContext _localctx = new OperatorAndContext(Context, State);
		EnterRule(_localctx, 56, RULE_operatorAnd);
		try {
			EnterOuterAlt(_localctx, 1);
			{
			State = 384;
			Match(T__33);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			ErrorHandler.ReportError(this, re);
			ErrorHandler.Recover(this, re);
		}
		finally {
			ExitRule();
		}
		return _localctx;
	}

	public partial class OperatorComparisonContext : ParserRuleContext {
		public OperatorComparisonContext(ParserRuleContext parent, int invokingState)
			: base(parent, invokingState)
		{
		}
		public override int RuleIndex { get { return RULE_operatorComparison; } }
		[System.Diagnostics.DebuggerNonUserCode]
		public override TResult Accept<TResult>(IParseTreeVisitor<TResult> visitor) {
			ILuaVisitor<TResult> typedVisitor = visitor as ILuaVisitor<TResult>;
			if (typedVisitor != null) return typedVisitor.VisitOperatorComparison(this);
			else return visitor.VisitChildren(this);
		}
	}

	[RuleVersion(0)]
	public OperatorComparisonContext operatorComparison() {
		OperatorComparisonContext _localctx = new OperatorComparisonContext(Context, State);
		EnterRule(_localctx, 58, RULE_operatorComparison);
		int _la;
		try {
			EnterOuterAlt(_localctx, 1);
			{
			State = 386;
			_la = TokenStream.LA(1);
			if ( !((((_la) & ~0x3f) == 0 && ((1L << _la) & 2164663517184L) != 0)) ) {
			ErrorHandler.RecoverInline(this);
			}
			else {
				ErrorHandler.ReportMatch(this);
			    Consume();
			}
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			ErrorHandler.ReportError(this, re);
			ErrorHandler.Recover(this, re);
		}
		finally {
			ExitRule();
		}
		return _localctx;
	}

	public partial class OperatorStrcatContext : ParserRuleContext {
		public OperatorStrcatContext(ParserRuleContext parent, int invokingState)
			: base(parent, invokingState)
		{
		}
		public override int RuleIndex { get { return RULE_operatorStrcat; } }
		[System.Diagnostics.DebuggerNonUserCode]
		public override TResult Accept<TResult>(IParseTreeVisitor<TResult> visitor) {
			ILuaVisitor<TResult> typedVisitor = visitor as ILuaVisitor<TResult>;
			if (typedVisitor != null) return typedVisitor.VisitOperatorStrcat(this);
			else return visitor.VisitChildren(this);
		}
	}

	[RuleVersion(0)]
	public OperatorStrcatContext operatorStrcat() {
		OperatorStrcatContext _localctx = new OperatorStrcatContext(Context, State);
		EnterRule(_localctx, 60, RULE_operatorStrcat);
		try {
			EnterOuterAlt(_localctx, 1);
			{
			State = 388;
			Match(T__40);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			ErrorHandler.ReportError(this, re);
			ErrorHandler.Recover(this, re);
		}
		finally {
			ExitRule();
		}
		return _localctx;
	}

	public partial class OperatorAddSubContext : ParserRuleContext {
		public OperatorAddSubContext(ParserRuleContext parent, int invokingState)
			: base(parent, invokingState)
		{
		}
		public override int RuleIndex { get { return RULE_operatorAddSub; } }
		[System.Diagnostics.DebuggerNonUserCode]
		public override TResult Accept<TResult>(IParseTreeVisitor<TResult> visitor) {
			ILuaVisitor<TResult> typedVisitor = visitor as ILuaVisitor<TResult>;
			if (typedVisitor != null) return typedVisitor.VisitOperatorAddSub(this);
			else return visitor.VisitChildren(this);
		}
	}

	[RuleVersion(0)]
	public OperatorAddSubContext operatorAddSub() {
		OperatorAddSubContext _localctx = new OperatorAddSubContext(Context, State);
		EnterRule(_localctx, 62, RULE_operatorAddSub);
		int _la;
		try {
			EnterOuterAlt(_localctx, 1);
			{
			State = 390;
			_la = TokenStream.LA(1);
			if ( !(_la==T__41 || _la==T__42) ) {
			ErrorHandler.RecoverInline(this);
			}
			else {
				ErrorHandler.ReportMatch(this);
			    Consume();
			}
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			ErrorHandler.ReportError(this, re);
			ErrorHandler.Recover(this, re);
		}
		finally {
			ExitRule();
		}
		return _localctx;
	}

	public partial class OperatorMulDivModContext : ParserRuleContext {
		public OperatorMulDivModContext(ParserRuleContext parent, int invokingState)
			: base(parent, invokingState)
		{
		}
		public override int RuleIndex { get { return RULE_operatorMulDivMod; } }
		[System.Diagnostics.DebuggerNonUserCode]
		public override TResult Accept<TResult>(IParseTreeVisitor<TResult> visitor) {
			ILuaVisitor<TResult> typedVisitor = visitor as ILuaVisitor<TResult>;
			if (typedVisitor != null) return typedVisitor.VisitOperatorMulDivMod(this);
			else return visitor.VisitChildren(this);
		}
	}

	[RuleVersion(0)]
	public OperatorMulDivModContext operatorMulDivMod() {
		OperatorMulDivModContext _localctx = new OperatorMulDivModContext(Context, State);
		EnterRule(_localctx, 64, RULE_operatorMulDivMod);
		int _la;
		try {
			EnterOuterAlt(_localctx, 1);
			{
			State = 392;
			_la = TokenStream.LA(1);
			if ( !((((_la) & ~0x3f) == 0 && ((1L << _la) & 263882790666240L) != 0)) ) {
			ErrorHandler.RecoverInline(this);
			}
			else {
				ErrorHandler.ReportMatch(this);
			    Consume();
			}
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			ErrorHandler.ReportError(this, re);
			ErrorHandler.Recover(this, re);
		}
		finally {
			ExitRule();
		}
		return _localctx;
	}

	public partial class OperatorBitwiseContext : ParserRuleContext {
		public OperatorBitwiseContext(ParserRuleContext parent, int invokingState)
			: base(parent, invokingState)
		{
		}
		public override int RuleIndex { get { return RULE_operatorBitwise; } }
		[System.Diagnostics.DebuggerNonUserCode]
		public override TResult Accept<TResult>(IParseTreeVisitor<TResult> visitor) {
			ILuaVisitor<TResult> typedVisitor = visitor as ILuaVisitor<TResult>;
			if (typedVisitor != null) return typedVisitor.VisitOperatorBitwise(this);
			else return visitor.VisitChildren(this);
		}
	}

	[RuleVersion(0)]
	public OperatorBitwiseContext operatorBitwise() {
		OperatorBitwiseContext _localctx = new OperatorBitwiseContext(Context, State);
		EnterRule(_localctx, 66, RULE_operatorBitwise);
		int _la;
		try {
			EnterOuterAlt(_localctx, 1);
			{
			State = 394;
			_la = TokenStream.LA(1);
			if ( !((((_la) & ~0x3f) == 0 && ((1L << _la) & 8725724278030336L) != 0)) ) {
			ErrorHandler.RecoverInline(this);
			}
			else {
				ErrorHandler.ReportMatch(this);
			    Consume();
			}
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			ErrorHandler.ReportError(this, re);
			ErrorHandler.Recover(this, re);
		}
		finally {
			ExitRule();
		}
		return _localctx;
	}

	public partial class OperatorUnaryContext : ParserRuleContext {
		public OperatorUnaryContext(ParserRuleContext parent, int invokingState)
			: base(parent, invokingState)
		{
		}
		public override int RuleIndex { get { return RULE_operatorUnary; } }
		[System.Diagnostics.DebuggerNonUserCode]
		public override TResult Accept<TResult>(IParseTreeVisitor<TResult> visitor) {
			ILuaVisitor<TResult> typedVisitor = visitor as ILuaVisitor<TResult>;
			if (typedVisitor != null) return typedVisitor.VisitOperatorUnary(this);
			else return visitor.VisitChildren(this);
		}
	}

	[RuleVersion(0)]
	public OperatorUnaryContext operatorUnary() {
		OperatorUnaryContext _localctx = new OperatorUnaryContext(Context, State);
		EnterRule(_localctx, 68, RULE_operatorUnary);
		int _la;
		try {
			EnterOuterAlt(_localctx, 1);
			{
			State = 396;
			_la = TokenStream.LA(1);
			if ( !((((_la) & ~0x3f) == 0 && ((1L << _la) & 28156293764087808L) != 0)) ) {
			ErrorHandler.RecoverInline(this);
			}
			else {
				ErrorHandler.ReportMatch(this);
			    Consume();
			}
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			ErrorHandler.ReportError(this, re);
			ErrorHandler.Recover(this, re);
		}
		finally {
			ExitRule();
		}
		return _localctx;
	}

	public partial class OperatorPowerContext : ParserRuleContext {
		public OperatorPowerContext(ParserRuleContext parent, int invokingState)
			: base(parent, invokingState)
		{
		}
		public override int RuleIndex { get { return RULE_operatorPower; } }
		[System.Diagnostics.DebuggerNonUserCode]
		public override TResult Accept<TResult>(IParseTreeVisitor<TResult> visitor) {
			ILuaVisitor<TResult> typedVisitor = visitor as ILuaVisitor<TResult>;
			if (typedVisitor != null) return typedVisitor.VisitOperatorPower(this);
			else return visitor.VisitChildren(this);
		}
	}

	[RuleVersion(0)]
	public OperatorPowerContext operatorPower() {
		OperatorPowerContext _localctx = new OperatorPowerContext(Context, State);
		EnterRule(_localctx, 70, RULE_operatorPower);
		try {
			EnterOuterAlt(_localctx, 1);
			{
			State = 398;
			Match(T__54);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			ErrorHandler.ReportError(this, re);
			ErrorHandler.Recover(this, re);
		}
		finally {
			ExitRule();
		}
		return _localctx;
	}

	public partial class NumberContext : ParserRuleContext {
		[System.Diagnostics.DebuggerNonUserCode] public ITerminalNode INT() { return GetToken(LuaParser.INT, 0); }
		[System.Diagnostics.DebuggerNonUserCode] public ITerminalNode HEX() { return GetToken(LuaParser.HEX, 0); }
		[System.Diagnostics.DebuggerNonUserCode] public ITerminalNode FLOAT() { return GetToken(LuaParser.FLOAT, 0); }
		[System.Diagnostics.DebuggerNonUserCode] public ITerminalNode HEX_FLOAT() { return GetToken(LuaParser.HEX_FLOAT, 0); }
		public NumberContext(ParserRuleContext parent, int invokingState)
			: base(parent, invokingState)
		{
		}
		public override int RuleIndex { get { return RULE_number; } }
		[System.Diagnostics.DebuggerNonUserCode]
		public override TResult Accept<TResult>(IParseTreeVisitor<TResult> visitor) {
			ILuaVisitor<TResult> typedVisitor = visitor as ILuaVisitor<TResult>;
			if (typedVisitor != null) return typedVisitor.VisitNumber(this);
			else return visitor.VisitChildren(this);
		}
	}

	[RuleVersion(0)]
	public NumberContext number() {
		NumberContext _localctx = new NumberContext(Context, State);
		EnterRule(_localctx, 72, RULE_number);
		int _la;
		try {
			EnterOuterAlt(_localctx, 1);
			{
			State = 400;
			_la = TokenStream.LA(1);
			if ( !((((_la) & ~0x3f) == 0 && ((1L << _la) & -1152921504606846976L) != 0)) ) {
			ErrorHandler.RecoverInline(this);
			}
			else {
				ErrorHandler.ReportMatch(this);
			    Consume();
			}
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			ErrorHandler.ReportError(this, re);
			ErrorHandler.Recover(this, re);
		}
		finally {
			ExitRule();
		}
		return _localctx;
	}

	public partial class StringContext : ParserRuleContext {
		[System.Diagnostics.DebuggerNonUserCode] public ITerminalNode NORMALSTRING() { return GetToken(LuaParser.NORMALSTRING, 0); }
		[System.Diagnostics.DebuggerNonUserCode] public ITerminalNode CHARSTRING() { return GetToken(LuaParser.CHARSTRING, 0); }
		[System.Diagnostics.DebuggerNonUserCode] public ITerminalNode LONGSTRING() { return GetToken(LuaParser.LONGSTRING, 0); }
		public StringContext(ParserRuleContext parent, int invokingState)
			: base(parent, invokingState)
		{
		}
		public override int RuleIndex { get { return RULE_string; } }
		[System.Diagnostics.DebuggerNonUserCode]
		public override TResult Accept<TResult>(IParseTreeVisitor<TResult> visitor) {
			ILuaVisitor<TResult> typedVisitor = visitor as ILuaVisitor<TResult>;
			if (typedVisitor != null) return typedVisitor.VisitString(this);
			else return visitor.VisitChildren(this);
		}
	}

	[RuleVersion(0)]
	public StringContext @string() {
		StringContext _localctx = new StringContext(Context, State);
		EnterRule(_localctx, 74, RULE_string);
		int _la;
		try {
			EnterOuterAlt(_localctx, 1);
			{
			State = 402;
			_la = TokenStream.LA(1);
			if ( !((((_la) & ~0x3f) == 0 && ((1L << _la) & 1008806316530991104L) != 0)) ) {
			ErrorHandler.RecoverInline(this);
			}
			else {
				ErrorHandler.ReportMatch(this);
			    Consume();
			}
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			ErrorHandler.ReportError(this, re);
			ErrorHandler.Recover(this, re);
		}
		finally {
			ExitRule();
		}
		return _localctx;
	}

	public override bool Sempred(RuleContext _localctx, int ruleIndex, int predIndex) {
		switch (ruleIndex) {
		case 12: return exp_sempred((ExpContext)_localctx, predIndex);
		}
		return true;
	}
	private bool exp_sempred(ExpContext _localctx, int predIndex) {
		switch (predIndex) {
		case 0: return Precpred(Context, 9);
		case 1: return Precpred(Context, 7);
		case 2: return Precpred(Context, 6);
		case 3: return Precpred(Context, 5);
		case 4: return Precpred(Context, 4);
		case 5: return Precpred(Context, 3);
		case 6: return Precpred(Context, 2);
		case 7: return Precpred(Context, 1);
		}
		return true;
	}

	private static int[] _serializedATN = {
		4,1,67,405,2,0,7,0,2,1,7,1,2,2,7,2,2,3,7,3,2,4,7,4,2,5,7,5,2,6,7,6,2,7,
		7,7,2,8,7,8,2,9,7,9,2,10,7,10,2,11,7,11,2,12,7,12,2,13,7,13,2,14,7,14,
		2,15,7,15,2,16,7,16,2,17,7,17,2,18,7,18,2,19,7,19,2,20,7,20,2,21,7,21,
		2,22,7,22,2,23,7,23,2,24,7,24,2,25,7,25,2,26,7,26,2,27,7,27,2,28,7,28,
		2,29,7,29,2,30,7,30,2,31,7,31,2,32,7,32,2,33,7,33,2,34,7,34,2,35,7,35,
		2,36,7,36,2,37,7,37,1,0,1,0,1,0,1,1,5,1,81,8,1,10,1,12,1,84,9,1,1,1,3,
		1,87,8,1,1,2,1,2,1,2,1,2,1,2,1,2,1,2,1,2,1,2,1,2,1,2,1,2,1,2,1,2,1,2,1,
		2,1,2,1,2,1,2,1,2,1,2,1,2,1,2,1,2,1,2,1,2,1,2,1,2,1,2,1,2,1,2,1,2,1,2,
		1,2,1,2,1,2,1,2,1,2,3,2,127,8,2,1,2,1,2,1,2,1,2,1,2,1,2,1,2,1,2,1,2,1,
		2,1,2,1,2,1,2,1,2,1,2,1,2,1,2,1,2,1,2,1,2,1,2,1,2,1,2,1,2,3,2,153,8,2,
		3,2,155,8,2,1,3,1,3,1,3,1,3,1,3,1,4,1,4,1,4,1,4,1,4,5,4,167,8,4,10,4,12,
		4,170,9,4,1,5,1,5,3,5,174,8,5,1,6,1,6,3,6,178,8,6,1,6,3,6,181,8,6,1,7,
		1,7,1,7,1,7,1,8,1,8,1,8,5,8,190,8,8,10,8,12,8,193,9,8,1,8,1,8,3,8,197,
		8,8,1,9,1,9,1,9,5,9,202,8,9,10,9,12,9,205,9,9,1,10,1,10,1,10,5,10,210,
		8,10,10,10,12,10,213,9,10,1,11,1,11,1,11,5,11,218,8,11,10,11,12,11,221,
		9,11,1,12,1,12,1,12,1,12,1,12,1,12,1,12,1,12,1,12,1,12,1,12,1,12,1,12,
		3,12,236,8,12,1,12,1,12,1,12,1,12,1,12,1,12,1,12,1,12,1,12,1,12,1,12,1,
		12,1,12,1,12,1,12,1,12,1,12,1,12,1,12,1,12,1,12,1,12,1,12,1,12,1,12,1,
		12,1,12,1,12,1,12,1,12,1,12,1,12,5,12,270,8,12,10,12,12,12,273,9,12,1,
		13,1,13,5,13,277,8,13,10,13,12,13,280,9,13,1,14,1,14,4,14,284,8,14,11,
		14,12,14,285,1,15,1,15,1,15,1,15,1,15,3,15,293,8,15,1,16,1,16,1,16,1,16,
		1,16,1,16,3,16,301,8,16,1,16,5,16,304,8,16,10,16,12,16,307,9,16,1,17,1,
		17,1,17,1,17,1,17,1,17,3,17,315,8,17,1,18,1,18,3,18,319,8,18,1,18,1,18,
		1,19,1,19,3,19,325,8,19,1,19,1,19,1,19,3,19,330,8,19,1,20,1,20,1,20,1,
		21,1,21,3,21,337,8,21,1,21,1,21,1,21,1,21,1,22,1,22,1,22,3,22,346,8,22,
		1,22,3,22,349,8,22,1,23,1,23,3,23,353,8,23,1,23,1,23,1,24,1,24,1,24,1,
		24,5,24,361,8,24,10,24,12,24,364,9,24,1,24,3,24,367,8,24,1,25,1,25,1,25,
		1,25,1,25,1,25,1,25,1,25,1,25,1,25,3,25,379,8,25,1,26,1,26,1,27,1,27,1,
		28,1,28,1,29,1,29,1,30,1,30,1,31,1,31,1,32,1,32,1,33,1,33,1,34,1,34,1,
		35,1,35,1,36,1,36,1,37,1,37,1,37,0,1,24,38,0,2,4,6,8,10,12,14,16,18,20,
		22,24,26,28,30,32,34,36,38,40,42,44,46,48,50,52,54,56,58,60,62,64,66,68,
		70,72,74,0,8,2,0,1,1,11,11,1,0,35,40,1,0,42,43,1,0,44,47,1,0,48,52,3,0,
		43,43,50,50,53,54,1,0,60,63,1,0,57,59,428,0,76,1,0,0,0,2,82,1,0,0,0,4,
		154,1,0,0,0,6,156,1,0,0,0,8,168,1,0,0,0,10,173,1,0,0,0,12,175,1,0,0,0,
		14,182,1,0,0,0,16,186,1,0,0,0,18,198,1,0,0,0,20,206,1,0,0,0,22,214,1,0,
		0,0,24,235,1,0,0,0,26,274,1,0,0,0,28,281,1,0,0,0,30,292,1,0,0,0,32,300,
		1,0,0,0,34,314,1,0,0,0,36,318,1,0,0,0,38,329,1,0,0,0,40,331,1,0,0,0,42,
		334,1,0,0,0,44,348,1,0,0,0,46,350,1,0,0,0,48,356,1,0,0,0,50,378,1,0,0,
		0,52,380,1,0,0,0,54,382,1,0,0,0,56,384,1,0,0,0,58,386,1,0,0,0,60,388,1,
		0,0,0,62,390,1,0,0,0,64,392,1,0,0,0,66,394,1,0,0,0,68,396,1,0,0,0,70,398,
		1,0,0,0,72,400,1,0,0,0,74,402,1,0,0,0,76,77,3,2,1,0,77,78,5,0,0,1,78,1,
		1,0,0,0,79,81,3,4,2,0,80,79,1,0,0,0,81,84,1,0,0,0,82,80,1,0,0,0,82,83,
		1,0,0,0,83,86,1,0,0,0,84,82,1,0,0,0,85,87,3,12,6,0,86,85,1,0,0,0,86,87,
		1,0,0,0,87,3,1,0,0,0,88,155,5,1,0,0,89,90,3,18,9,0,90,91,5,2,0,0,91,92,
		3,22,11,0,92,155,1,0,0,0,93,155,3,28,14,0,94,155,3,14,7,0,95,155,5,3,0,
		0,96,97,5,4,0,0,97,155,5,56,0,0,98,99,5,5,0,0,99,100,3,2,1,0,100,101,5,
		6,0,0,101,155,1,0,0,0,102,103,5,7,0,0,103,104,3,24,12,0,104,105,5,5,0,
		0,105,106,3,2,1,0,106,107,5,6,0,0,107,155,1,0,0,0,108,109,5,8,0,0,109,
		110,3,2,1,0,110,111,5,9,0,0,111,112,3,24,12,0,112,155,1,0,0,0,113,114,
		3,6,3,0,114,115,3,8,4,0,115,116,3,10,5,0,116,117,5,6,0,0,117,155,1,0,0,
		0,118,119,5,10,0,0,119,120,5,56,0,0,120,121,5,2,0,0,121,122,3,24,12,0,
		122,123,5,11,0,0,123,126,3,24,12,0,124,125,5,11,0,0,125,127,3,24,12,0,
		126,124,1,0,0,0,126,127,1,0,0,0,127,128,1,0,0,0,128,129,5,5,0,0,129,130,
		3,2,1,0,130,131,5,6,0,0,131,155,1,0,0,0,132,133,5,10,0,0,133,134,3,20,
		10,0,134,135,5,12,0,0,135,136,3,22,11,0,136,137,5,5,0,0,137,138,3,2,1,
		0,138,139,5,6,0,0,139,155,1,0,0,0,140,141,5,13,0,0,141,142,3,16,8,0,142,
		143,3,42,21,0,143,155,1,0,0,0,144,145,5,14,0,0,145,146,5,13,0,0,146,147,
		5,56,0,0,147,155,3,42,21,0,148,149,5,14,0,0,149,152,3,20,10,0,150,151,
		5,2,0,0,151,153,3,22,11,0,152,150,1,0,0,0,152,153,1,0,0,0,153,155,1,0,
		0,0,154,88,1,0,0,0,154,89,1,0,0,0,154,93,1,0,0,0,154,94,1,0,0,0,154,95,
		1,0,0,0,154,96,1,0,0,0,154,98,1,0,0,0,154,102,1,0,0,0,154,108,1,0,0,0,
		154,113,1,0,0,0,154,118,1,0,0,0,154,132,1,0,0,0,154,140,1,0,0,0,154,144,
		1,0,0,0,154,148,1,0,0,0,155,5,1,0,0,0,156,157,5,15,0,0,157,158,3,24,12,
		0,158,159,5,16,0,0,159,160,3,2,1,0,160,7,1,0,0,0,161,162,5,17,0,0,162,
		163,3,24,12,0,163,164,5,16,0,0,164,165,3,2,1,0,165,167,1,0,0,0,166,161,
		1,0,0,0,167,170,1,0,0,0,168,166,1,0,0,0,168,169,1,0,0,0,169,9,1,0,0,0,
		170,168,1,0,0,0,171,172,5,18,0,0,172,174,3,2,1,0,173,171,1,0,0,0,173,174,
		1,0,0,0,174,11,1,0,0,0,175,177,5,19,0,0,176,178,3,22,11,0,177,176,1,0,
		0,0,177,178,1,0,0,0,178,180,1,0,0,0,179,181,5,1,0,0,180,179,1,0,0,0,180,
		181,1,0,0,0,181,13,1,0,0,0,182,183,5,20,0,0,183,184,5,56,0,0,184,185,5,
		20,0,0,185,15,1,0,0,0,186,191,5,56,0,0,187,188,5,21,0,0,188,190,5,56,0,
		0,189,187,1,0,0,0,190,193,1,0,0,0,191,189,1,0,0,0,191,192,1,0,0,0,192,
		196,1,0,0,0,193,191,1,0,0,0,194,195,5,22,0,0,195,197,5,56,0,0,196,194,
		1,0,0,0,196,197,1,0,0,0,197,17,1,0,0,0,198,203,3,32,16,0,199,200,5,11,
		0,0,200,202,3,32,16,0,201,199,1,0,0,0,202,205,1,0,0,0,203,201,1,0,0,0,
		203,204,1,0,0,0,204,19,1,0,0,0,205,203,1,0,0,0,206,211,5,56,0,0,207,208,
		5,11,0,0,208,210,5,56,0,0,209,207,1,0,0,0,210,213,1,0,0,0,211,209,1,0,
		0,0,211,212,1,0,0,0,212,21,1,0,0,0,213,211,1,0,0,0,214,219,3,24,12,0,215,
		216,5,11,0,0,216,218,3,24,12,0,217,215,1,0,0,0,218,221,1,0,0,0,219,217,
		1,0,0,0,219,220,1,0,0,0,220,23,1,0,0,0,221,219,1,0,0,0,222,223,6,12,-1,
		0,223,236,5,23,0,0,224,236,5,24,0,0,225,236,5,25,0,0,226,236,3,72,36,0,
		227,236,3,74,37,0,228,236,5,26,0,0,229,236,3,40,20,0,230,236,3,26,13,0,
		231,236,3,46,23,0,232,233,3,68,34,0,233,234,3,24,12,8,234,236,1,0,0,0,
		235,222,1,0,0,0,235,224,1,0,0,0,235,225,1,0,0,0,235,226,1,0,0,0,235,227,
		1,0,0,0,235,228,1,0,0,0,235,229,1,0,0,0,235,230,1,0,0,0,235,231,1,0,0,
		0,235,232,1,0,0,0,236,271,1,0,0,0,237,238,10,9,0,0,238,239,3,70,35,0,239,
		240,3,24,12,9,240,270,1,0,0,0,241,242,10,7,0,0,242,243,3,64,32,0,243,244,
		3,24,12,8,244,270,1,0,0,0,245,246,10,6,0,0,246,247,3,62,31,0,247,248,3,
		24,12,7,248,270,1,0,0,0,249,250,10,5,0,0,250,251,3,60,30,0,251,252,3,24,
		12,5,252,270,1,0,0,0,253,254,10,4,0,0,254,255,3,58,29,0,255,256,3,24,12,
		5,256,270,1,0,0,0,257,258,10,3,0,0,258,259,3,56,28,0,259,260,3,24,12,4,
		260,270,1,0,0,0,261,262,10,2,0,0,262,263,3,54,27,0,263,264,3,24,12,3,264,
		270,1,0,0,0,265,266,10,1,0,0,266,267,3,66,33,0,267,268,3,24,12,2,268,270,
		1,0,0,0,269,237,1,0,0,0,269,241,1,0,0,0,269,245,1,0,0,0,269,249,1,0,0,
		0,269,253,1,0,0,0,269,257,1,0,0,0,269,261,1,0,0,0,269,265,1,0,0,0,270,
		273,1,0,0,0,271,269,1,0,0,0,271,272,1,0,0,0,272,25,1,0,0,0,273,271,1,0,
		0,0,274,278,3,30,15,0,275,277,3,36,18,0,276,275,1,0,0,0,277,280,1,0,0,
		0,278,276,1,0,0,0,278,279,1,0,0,0,279,27,1,0,0,0,280,278,1,0,0,0,281,283,
		3,30,15,0,282,284,3,36,18,0,283,282,1,0,0,0,284,285,1,0,0,0,285,283,1,
		0,0,0,285,286,1,0,0,0,286,29,1,0,0,0,287,293,3,32,16,0,288,289,5,27,0,
		0,289,290,3,24,12,0,290,291,5,28,0,0,291,293,1,0,0,0,292,287,1,0,0,0,292,
		288,1,0,0,0,293,31,1,0,0,0,294,301,5,56,0,0,295,296,5,27,0,0,296,297,3,
		24,12,0,297,298,5,28,0,0,298,299,3,34,17,0,299,301,1,0,0,0,300,294,1,0,
		0,0,300,295,1,0,0,0,301,305,1,0,0,0,302,304,3,34,17,0,303,302,1,0,0,0,
		304,307,1,0,0,0,305,303,1,0,0,0,305,306,1,0,0,0,306,33,1,0,0,0,307,305,
		1,0,0,0,308,309,5,29,0,0,309,310,3,24,12,0,310,311,5,30,0,0,311,315,1,
		0,0,0,312,313,5,21,0,0,313,315,5,56,0,0,314,308,1,0,0,0,314,312,1,0,0,
		0,315,35,1,0,0,0,316,317,5,22,0,0,317,319,5,56,0,0,318,316,1,0,0,0,318,
		319,1,0,0,0,319,320,1,0,0,0,320,321,3,38,19,0,321,37,1,0,0,0,322,324,5,
		27,0,0,323,325,3,22,11,0,324,323,1,0,0,0,324,325,1,0,0,0,325,326,1,0,0,
		0,326,330,5,28,0,0,327,330,3,46,23,0,328,330,3,74,37,0,329,322,1,0,0,0,
		329,327,1,0,0,0,329,328,1,0,0,0,330,39,1,0,0,0,331,332,5,13,0,0,332,333,
		3,42,21,0,333,41,1,0,0,0,334,336,5,27,0,0,335,337,3,44,22,0,336,335,1,
		0,0,0,336,337,1,0,0,0,337,338,1,0,0,0,338,339,5,28,0,0,339,340,3,2,1,0,
		340,341,5,6,0,0,341,43,1,0,0,0,342,345,3,20,10,0,343,344,5,11,0,0,344,
		346,5,26,0,0,345,343,1,0,0,0,345,346,1,0,0,0,346,349,1,0,0,0,347,349,5,
		26,0,0,348,342,1,0,0,0,348,347,1,0,0,0,349,45,1,0,0,0,350,352,5,31,0,0,
		351,353,3,48,24,0,352,351,1,0,0,0,352,353,1,0,0,0,353,354,1,0,0,0,354,
		355,5,32,0,0,355,47,1,0,0,0,356,362,3,50,25,0,357,358,3,52,26,0,358,359,
		3,50,25,0,359,361,1,0,0,0,360,357,1,0,0,0,361,364,1,0,0,0,362,360,1,0,
		0,0,362,363,1,0,0,0,363,366,1,0,0,0,364,362,1,0,0,0,365,367,3,52,26,0,
		366,365,1,0,0,0,366,367,1,0,0,0,367,49,1,0,0,0,368,369,5,29,0,0,369,370,
		3,24,12,0,370,371,5,30,0,0,371,372,5,2,0,0,372,373,3,24,12,0,373,379,1,
		0,0,0,374,375,5,56,0,0,375,376,5,2,0,0,376,379,3,24,12,0,377,379,3,24,
		12,0,378,368,1,0,0,0,378,374,1,0,0,0,378,377,1,0,0,0,379,51,1,0,0,0,380,
		381,7,0,0,0,381,53,1,0,0,0,382,383,5,33,0,0,383,55,1,0,0,0,384,385,5,34,
		0,0,385,57,1,0,0,0,386,387,7,1,0,0,387,59,1,0,0,0,388,389,5,41,0,0,389,
		61,1,0,0,0,390,391,7,2,0,0,391,63,1,0,0,0,392,393,7,3,0,0,393,65,1,0,0,
		0,394,395,7,4,0,0,395,67,1,0,0,0,396,397,7,5,0,0,397,69,1,0,0,0,398,399,
		5,55,0,0,399,71,1,0,0,0,400,401,7,6,0,0,401,73,1,0,0,0,402,403,7,7,0,0,
		403,75,1,0,0,0,33,82,86,126,152,154,168,173,177,180,191,196,203,211,219,
		235,269,271,278,285,292,300,305,314,318,324,329,336,345,348,352,362,366,
		378
	};

	public static readonly ATN _ATN =
		new ATNDeserializer().Deserialize(_serializedATN);


}
