using System.Text;
using MoonsecDeobfuscator.Ast.Expressions;
using MoonsecDeobfuscator.Ast.Literals;
using MoonsecDeobfuscator.Ast.Statements;

namespace MoonsecDeobfuscator.Ast;

public class PrettyPrinter : AstWalker
{
    private readonly StringBuilder _builder = new();
    private int _depth;

    public static void Print(Node node)
    {
        var visitor = new PrettyPrinter();
        node.Accept(visitor);
        Console.WriteLine(visitor._builder);
    }

    public static string AsString(Node node)
    {
        var visitor = new PrettyPrinter();
        node.Accept(visitor);
        return visitor._builder.ToString();
    }

    private void Indent()
    {
        for (var i = 0; i < _depth * 4; i++)
            _builder.Append(' ');
    }

    public override void Visit(Name node)
    {
        _builder.Append(node.Value);
    }

    public override void Visit(ElementAccess node)
    {
        Visit(node.Table);
        _builder.Append('[');
        Visit(node.Key);
        _builder.Append(']');
    }

    public override void Visit(MemberAccess node)
    {
        Visit(node.Table);
        _builder.Append($".{node.Key.Value}");
    }

    public override void Visit(BinaryExpression node)
    {
        _builder.Append('(');
        Visit(node.Left);
        _builder.Append(OperatorToString(node.Operator));
        Visit(node.Right);
        _builder.Append(')');
    }

    public override void Visit(UnaryExpression node)
    {
        _builder.Append(OperatorToString(node.Operator));
        Visit(node.Operand);
    }

    public override void Visit(Call node)
    {
        var needsPar = node.Function is not Variable;

        if (needsPar)
            _builder.Append('(');

        Visit(node.Function);

        if (needsPar)
            _builder.Append(')');

        _builder.Append('(');
        VisitList(node.Arguments);
        _builder.Append(')');
    }

    public override void Visit(MethodCall node)
    {
        var needsPar = node.Function is not Variable;

        if (needsPar)
            _builder.Append('(');

        Visit(node.Function);

        if (needsPar)
            _builder.Append(')');

        _builder.Append($":{node.MethodName.Value}");
        _builder.Append('(');
        VisitList(node.Arguments);
        _builder.Append(')');
    }

    public override void Visit(Table node)
    {
        var entries = node.Entries;

        if (entries.Count == 0)
        {
            _builder.Append("{}");
        }
        else
        {
            _builder.AppendLine("{");
            _depth++;

            for (var i = 0; i < entries.Count; i++)
            {
                Indent();
                Visit(entries[i]);

                if (i + 1 < entries.Count)
                    _builder.Append(", ");

                _builder.AppendLine();
            }

            _depth--;
            Indent();
            _builder.Append('}');
        }
    }

    public override void Visit(Table.Entry node)
    {
        var key = node.Key;

        if (key != null)
        {
            if (key is Name name)
            {
                _builder.Append($"{name.Value} = ");
            }
            else
            {
                _builder.Append('[');
                Visit(key);
                _builder.Append("] = ");
            }
        }

        Visit(node.Value);
    }

    public override void Visit(AnonymousFunction node)
    {
        _builder.Append("function");
        Visit(node.Parameters);
        _depth++;
        Visit(node.Body);
        _depth--;
        Indent();
        _builder.Append("end");
    }

    public override void Visit(VarArg node)
    {
        _builder.Append("...");
    }

    public override void Visit(ExpressionStatement node)
    {
        Visit(node.Expression);
    }

    public override void Visit(While node)
    {
        _builder.Append("while ");
        Visit(node.Condition);
        _builder.AppendLine(" do");
        _depth++;
        Visit(node.Body);
        _depth--;
        Indent();
        _builder.Append("end");
    }

    public override void Visit(LocalDeclare node)
    {
        _builder.Append("local ");
        VisitList(node.Names);

        if (node.Values.Count > 0)
        {
            _builder.Append(" = ");
            VisitList(node.Values);
        }
    }

    public override void Visit(Assign node)
    {
        VisitList(node.Variables);
        _builder.Append(" = ");
        VisitList(node.Values);
    }

    public override void Visit(GenericFor node)
    {
        _builder.Append("for ");
        VisitList(node.Names);
        _builder.Append(" in ");
        VisitList(node.Expressions);
        _builder.AppendLine(" do");
        _depth++;
        Visit(node.Body);
        _depth--;
        Indent();
        _builder.Append("end");
    }

    public override void Visit(NumericFor node)
    {
        _builder.Append($"for {node.IteratorName.Value} = ");
        Visit(node.InitialValue);
        _builder.Append(", ");
        Visit(node.FinalValue);

        if (node.Step != null)
        {
            _builder.Append(", ");
            Visit(node.Step);
        }

        _builder.AppendLine(" do");
        _depth++;
        Visit(node.Body);
        _depth--;
        Indent();
        _builder.Append("end");
    }

    public override void Visit(If node)
    {
        _builder.Append("if ");
        Visit(node.IfClause);

        foreach (var elseIf in node.ElseIfClauses)
        {
            Indent();
            _builder.Append("elseif ");
            Visit(elseIf);
        }

        if (node.ElseBody != null)
        {
            Indent();
            _builder.AppendLine("else");
            _depth++;
            Visit(node.ElseBody);
            _depth--;
        }

        Indent();
        _builder.Append("end");
    }

    public override void Visit(If.Clause node)
    {
        Visit(node.Condition);
        _builder.AppendLine(" then");
        _depth++;
        Visit(node.Body);
        _depth--;
    }

    public override void Visit(LocalFunction node)
    {
        _builder.Append($"local function {node.Name.Value}");
        Visit(node.Parameters);
        _depth++;
        Visit(node.Body);
        _depth--;
        Indent();
        _builder.Append("end");
    }

    public override void Visit(Function node)
    {
        _builder.Append($"function {node.Name.Value}");
        Visit(node.Parameters);
        _depth++;
        Visit(node.Body);
        _depth--;
        Indent();
        _builder.Append("end");
    }

    public override void Visit(Break node)
    {
        _builder.Append("break");
    }

    public override void Visit(Do node)
    {
        _builder.AppendLine("do");
        _depth++;
        Visit(node.Body);
        _depth--;
        Indent();
        _builder.Append("end");
    }

    public override void Visit(Repeat node)
    {
        _builder.AppendLine("repeat");
        _depth++;
        Visit(node.Body);
        _depth--;
        Indent();
        _builder.Append("until ");
        Visit(node.Condition);
    }

    public override void Visit(Return node)
    {
        _builder.Append("return");

        if (node.Values.Count > 0)
        {
            _builder.Append(' ');
            VisitList(node.Values);
        }
    }

    public override void Visit(ParameterList node)
    {
        _builder.Append('(');

        VisitList(node.Names);

        if (node.IsVarArg)
            _builder.Append(node.Names.Count == 0 ? "..." : ", ...");

        _builder.Append(')');
        _builder.AppendLine();
    }

    public override void Visit(StringLiteral node)
    {
        _builder.Append(node.Value);
    }

    public override void Visit(NumberLiteral node)
    {
        _builder.Append(node.Value);
    }

    public override void Visit(BooleanLiteral node)
    {
        _builder.Append(node.Value.ToString().ToLower());
    }

    public override void Visit(Nil node)
    {
        _builder.Append("nil");
    }

    public override void Visit(Block node)
    {
        foreach (var statement in node.Statements)
        {
            Indent();
            Visit(statement);
            _builder.AppendLine();
        }
    }

    public override void Visit(Goto node)
    {
        _builder.Append($"goto {node.Name.Value}");
    }

    public override void Visit(Label node)
    {
        _builder.Append($"::{node.Name.Value}::");
    }

    protected override void VisitList<TNode>(NodeList<TNode> list)
    {
        for (var i = 0; i < list.Count; i++)
        {
            Visit(list[i]);

            if (i + 1 < list.Count)
                _builder.Append(", ");
        }
    }

    private static string OperatorToString(BinaryOperator @operator) => @operator switch
    {
        BinaryOperator.Add => " + ",
        BinaryOperator.Sub => " - ",
        BinaryOperator.Mul => " * ",
        BinaryOperator.Div => " / ",
        BinaryOperator.Mod => " % ",
        BinaryOperator.Pow => " ^ ",
        BinaryOperator.Equals => " == ",
        BinaryOperator.NotEquals => " ~= ",
        BinaryOperator.LessThan => " < ",
        BinaryOperator.GreaterThan => " > ",
        BinaryOperator.LessThanOrEquals => " <= ",
        BinaryOperator.GreaterThanOrEquals => " >= ",
        BinaryOperator.And => " and ",
        BinaryOperator.Or => " or ",
        BinaryOperator.Concat => " .. ",
        _ => " ?? "
    };

    private static string OperatorToString(UnaryOperator @operator) => @operator switch
    {
        UnaryOperator.Not => "not ",
        UnaryOperator.Length => "#",
        UnaryOperator.Negate => "-",
        _ => "??"
    };
}