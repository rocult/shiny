using System.Collections.Immutable;
using Antlr4.Runtime;
using MoonsecDeobfuscator.Ast.Statements;
using MoonsecDeobfuscator.Syntax;
using MoonsecDeobfuscator.Syntax.Parser;

namespace MoonsecDeobfuscator.Ast;

public class Block(NodeList<Statement> statements) : Node
{
    public NodeList<Statement> Statements { get; set; } = statements;

    public Block() : this([])
    {
    }

    public override ImmutableList<Node> ChildNodes() => [..Statements];

    public override Block Clone() => new(Statements.Clone());

    public override void Accept(AstWalker visitor)
    {
        visitor.Visit(this);
    }

    public override Node Accept(AstRewriter visitor) => visitor.Visit(this);

    public static Block FromString(string code)
    {
        var lexer = new LuaLexer(CharStreams.fromString(code));
        var tokens = new CommonTokenStream(lexer);

        return new CstToAstVisitor().VisitBlock(new LuaParser(tokens).block());
    }
}