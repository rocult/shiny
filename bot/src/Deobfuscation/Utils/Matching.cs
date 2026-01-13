using MoonsecDeobfuscator.Ast.Expressions;
using MoonsecDeobfuscator.Ast.Literals;
using MoonsecDeobfuscator.Ast.Statements;

namespace MoonsecDeobfuscator.Deobfuscation.Utils;

public static class Matching
{
    public static bool IsEnumComparison(Expression node) =>
        node is BinaryExpression { Left: Name { Value: "enum" } };

    public static bool IsInstAssign(Statement node) => node is Assign
    {
        Variables: [Name { Value: "inst" }],
        Values: [ElementAccess { Table: Name { Value: "insts" } }]
    };

    public static bool IsPcIncrement(Statement node) => node is Assign
    {
        Variables: [Name { Value: "pc" }],
        Values:
        [
            BinaryExpression
            {
                Operator: BinaryOperator.Add,
                Left: Name { Value: "pc" } or NumberLiteral,
                Right: Name { Value: "pc" } or NumberLiteral
            }
        ]
    };
}