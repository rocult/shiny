using MoonsecDeobfuscator.Ast;
using MoonsecDeobfuscator.Ast.Expressions;

namespace MoonsecDeobfuscator.Syntax.Semantic;

public class VariableInfo
{
    public Node? DeclarationLocation;
    public Expression? Value;
    public int AssignmentCount, ReadCount;
}