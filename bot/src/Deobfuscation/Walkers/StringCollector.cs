using MoonsecDeobfuscator.Ast;
using MoonsecDeobfuscator.Ast.Expressions;
using MoonsecDeobfuscator.Ast.Literals;

namespace MoonsecDeobfuscator.Deobfuscation.Walkers;

public class StringCollector : AstWalker
{
    private readonly List<(int?, string)> _strings = [];

    public override void Visit(Call node)
    {
        if (node.Arguments is [NumberLiteral number, StringLiteral @string])
            _strings.Add(((int) number.Value, @string.Value[1..^1]));

        base.Visit(node);
    }

    public override void Visit(StringLiteral node)
    {
        var value = node.Value;

        if (value.StartsWith("\"\\4\\8"))
            _strings.Add((null, value[1..^1]));
    }

    public static List<(int?, string)> Collect(Node node)
    {
        var walker = new StringCollector();
        walker.Visit(node);
        return walker._strings;
    }
}