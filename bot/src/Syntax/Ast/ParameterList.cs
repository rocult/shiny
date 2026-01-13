using System.Collections.Immutable;
using MoonsecDeobfuscator.Ast.Expressions;

namespace MoonsecDeobfuscator.Ast;

public class ParameterList(NodeList<Name> names, bool isVarArg = false) : Node
{
    public NodeList<Name> Names { get; set; } = names;
    public bool IsVarArg { get; set; } = isVarArg;

    public ParameterList() : this([])
    {
    }

    public override ImmutableList<Node> ChildNodes() => [..Names];

    public override ParameterList Clone() => new(Names.Clone(), IsVarArg);

    public override void Accept(AstWalker visitor)
    {
        visitor.Visit(this);
    }

    public override Node Accept(AstRewriter visitor) => visitor.Visit(this);
}