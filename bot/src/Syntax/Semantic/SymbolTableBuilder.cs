using MoonsecDeobfuscator.Ast;
using MoonsecDeobfuscator.Ast.Expressions;
using MoonsecDeobfuscator.Ast.Statements;

namespace MoonsecDeobfuscator.Syntax.Semantic;

public class SymbolTableBuilder : AstWalker
{
    private readonly SymbolTable _symbols = new();

    public static SymbolTable Build(Node node)
    {
        var visitor = new SymbolTableBuilder();
        visitor.Visit(node);
        return visitor._symbols;
    }

    public override void Visit(LocalDeclare node)
    {
        var names = node.Names;
        var values = node.Values;

        VisitList(values);

        for (var i = 0; i < names.Count; i++)
        {
            var name = names[i].Value;
            var expression = i < values.Count ? values[i] : values.LastOrDefault();

            _symbols.Define(name, new VariableInfo
            {
                Value = expression,
                DeclarationLocation = node
            });
        }
    }

    public override void Visit(Assign node)
    {
        var variables = node.Variables;
        var values = node.Values;

        VisitList(values);

        for (var i = 0; i < variables.Count; i++)
        {
            var variable = variables[i];

            if (variable is not Name name)
            {
                Visit(variable);
                continue;
            }

            var value = name.Value;
            var expression = i < values.Count ? values[i] : values.LastOrDefault();

            if (_symbols.IsDefined(value))
            {
                var info = _symbols.GetVariable(value)!;

                info.Value = null;
                info.AssignmentCount++;
            }
            else
            {
                _symbols.Define(value, new VariableInfo
                {
                    DeclarationLocation = node,
                    Value = expression
                });
            }
        }
    }

    public override void Visit(Name node)
    {
        var info = _symbols.GetVariable(node.Value);

        if (info != null)
            info.ReadCount++;
    }

    public override void Visit(ParameterList node)
    {
        foreach (var name in node.Names)
        {
            _symbols.Define(name.Value, new VariableInfo
            {
                DeclarationLocation = node
            });
        }
    }

    public override void Visit(Function node)
    {
        _symbols.Define(node.Name.Value, new VariableInfo
        {
            DeclarationLocation = node
        });

        Visit(node.Parameters);
        Visit(node.Body);
    }

    public override void Visit(LocalFunction node)
    {
        _symbols.Define(node.Name.Value, new VariableInfo
        {
            DeclarationLocation = node
        });

        Visit(node.Parameters);
        Visit(node.Body);
    }

    public override void Visit(NumericFor node)
    {
        Visit(node.InitialValue);
        Visit(node.FinalValue);

        if (node.Step != null)
            Visit(node.Step);

        _symbols.Define(node.IteratorName.Value, new VariableInfo
        {
            DeclarationLocation = node
        });

        Visit(node.Body);
    }

    public override void Visit(GenericFor node)
    {
        VisitList(node.Expressions);

        foreach (var name in node.Names)
        {
            _symbols.Define(name.Value, new VariableInfo
            {
                DeclarationLocation = node
            });
        }

        Visit(node.Body);
    }
}