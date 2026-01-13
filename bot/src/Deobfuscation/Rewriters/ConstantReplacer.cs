using MoonsecDeobfuscator.Ast;
using MoonsecDeobfuscator.Ast.Expressions;
using MoonsecDeobfuscator.Ast.Literals;

namespace MoonsecDeobfuscator.Deobfuscation.Rewriters;

public class ConstantReplacer(Dictionary<string, string[]> constants) : AstRewriter
{
    public override Node Visit(MemberAccess node) =>
        constants.TryGetValue(node.Key.Value, out var values) ? CreateReplacement(values) : node;

    private static Expression CreateReplacement(string[] values)
    {
        if (int.TryParse(values[0], out var result))
            return new NumberLiteral(result);

        Expression name = new Name(values[0]);
        return values
            .Skip(1)
            .Aggregate(name, (acc, str) => new MemberAccess(acc, new Name(str)));
    }
}