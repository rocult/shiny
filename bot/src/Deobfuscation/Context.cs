using MoonsecDeobfuscator.Ast.Expressions;
using MoonsecDeobfuscator.Ast.Statements;
using MoonsecDeobfuscator.Deobfuscation.Bytecode;

namespace MoonsecDeobfuscator.Deobfuscation;

public class Context
{
    public readonly Dictionary<string, string> IdentifiedNames = [];
    public readonly List<BinaryExpression> KeyExpressions = [];

    public readonly List<ProtoStep> ProtoFormat = [];
    public readonly Dictionary<int, ProtoStep> ConstantFormat = [];

    public LocalFunction Wrapper;
    public If VmTree;
    public string BytecodeString;

    public int BytecodeKey, ConstantKey;
}