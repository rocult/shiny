using MoonsecDeobfuscator.Ast;
using MoonsecDeobfuscator.Ast.Expressions;
using MoonsecDeobfuscator.Ast.Literals;
using MoonsecDeobfuscator.Ast.Statements;
using MoonsecDeobfuscator.Deobfuscation.Bytecode;

namespace MoonsecDeobfuscator.Deobfuscation.Walkers;

public class Analyzer(Context ctx) : AstWalker
{
    private int _funcCounter;

    public override void Visit(LocalFunction node)
    {
        switch (_funcCounter++)
        {
            case 8:
                GetProtoFormat(node);
                break;
            case 10:
                IdentifyWrapProto(node);
                break;
            case 11:
                IdentifyWrapper(node);
                break;
        }

        base.Visit(node);
    }

    public override void Visit(Call node)
    {
        if (node.Arguments is [Name, StringLiteral @string])
            ctx.BytecodeString = @string.Value.Trim('"');

        base.Visit(node);
    }

    public override void Visit(BinaryExpression node)
    {
        if (node is { Operator: BinaryOperator.Add, Right: Call })
        {
            ctx.KeyExpressions.Add(node);
            return;
        }

        base.Visit(node);
    }

    public override void Visit(LocalDeclare node)
    {
        if (node is { Names: [Name name], Values: [Expression expression] })
        {
            var info = GetVariable(name.Value);

            if (info is not { AssignmentCount: 0 })
                return;

            if (expression is NumberLiteral number)
            {
                switch ((int) number.Value)
                {
                    case 1:
                        Identify(name.Value, "OP_ENUM");
                        break;
                    case 2:
                        Identify(name.Value, "OP_A");
                        break;
                    case 3:
                        Identify(name.Value, "OP_B");
                        break;
                    case 4:
                        Identify(name.Value, "OP_C");
                        break;
                }
            }
            else if (expression is BinaryExpression { Left: Name { Value: "unpack" } })
            {
                Identify(name.Value, "unpack");
            }
        }

        base.Visit(node);
    }

    private void GetProtoFormat(LocalFunction node)
    {
        foreach (var stat in node.Body.Statements)
        {
            if (stat is Assign)
            {
                ctx.ProtoFormat.Add(ProtoStep.NumParams);
            }
            else if (stat is NumericFor numericFor)
            {
                switch (numericFor.Body.Statements.Count)
                {
                    case 1:
                        ctx.ProtoFormat.Add(ProtoStep.Functions);
                        break;
                    case 2:
                        ctx.ProtoFormat.Add(ProtoStep.Instructions);
                        break;
                    case 4:
                        GetConstantFormat(numericFor);
                        ctx.ProtoFormat.Add(ProtoStep.Constants);
                        break;
                }
            }
        }
    }

    private void GetConstantFormat(NumericFor node)
    {
        var values = node.Body.DescendantNodes()
            .Where(it => it is BinaryExpression { Operator: BinaryOperator.Equals })
            .Select(it => (BinaryExpression) it)
            .Select(it => (byte) ((NumberLiteral) it.Right).Value)
            .ToList();

        ctx.ConstantFormat[values[0]] = ProtoStep.BooleanConstant;
        ctx.ConstantFormat[values[1]] = ProtoStep.NumberConstant;
        ctx.ConstantFormat[values[2]] = ProtoStep.StringConstant;
    }

    private void IdentifyWrapProto(LocalFunction node)
    {
        var names = node.Parameters.Names;

        Identify(node.Name.Value, "wrap_proto");
        Identify(names[0].Value, "proto");
        Identify(names[1].Value, "upv");
        Identify(names[2].Value, "env");
    }

    private void IdentifyWrapper(LocalFunction node)
    {
        var names = node.Body.ChildNodes()
            .OfType<LocalDeclare>()
            .SelectMany(decl => decl.Names)
            .ToList();

        Identify(names[0].Value, "insts");
        Identify(names[1].Value, "protos");
        Identify(names[2].Value, "params");
        Identify(names[4].Value, "_R");
        Identify(names[5].Value, "pc");
        Identify(names[6].Value, "top");
        Identify(names[7].Value, "vararg");
        Identify(names[8].Value, "args");
        Identify(names[9].Value, "pcount");
        Identify(names[10].Value, "lupv");
        Identify(names[11].Value, "stk");
        Identify(names[13].Value, "varargz");
        Identify(names[14].Value, "inst");
        Identify(names[15].Value, "enum");

        ctx.Wrapper = node;
        ctx.VmTree = (If) node.DescendantNodes()
            .First(n => n is If { IfClause.Condition: BinaryExpression { Left: Name name } }
                        && name.Value == names[15].Value);
    }

    private void Identify(string name, string newName)
    {
        ctx.IdentifiedNames[name] = newName;
    }
}