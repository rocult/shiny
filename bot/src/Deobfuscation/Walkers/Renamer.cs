using MoonsecDeobfuscator.Ast;
using MoonsecDeobfuscator.Ast.Expressions;
using MoonsecDeobfuscator.Ast.Statements;

namespace MoonsecDeobfuscator.Deobfuscation.Walkers;

public class Renamer : AstWalker
{
    private readonly Dictionary<string, string> _names;
    private readonly Stack<Scope> _scopes = [];
    private int _counter;

    public Renamer(Dictionary<string, string> names)
    {
        _names = names;
        PushScope();
    }

    public Renamer() : this([])
    {
    }

    private Scope PushScope()
    {
        var scope = new Scope();
        _scopes.Push(scope);
        return scope;
    }

    private void PopScope()
    {
        _scopes.Pop();
    }

    private void RenameAndDefine(Scope scope, Name name)
    {
        var newName = _names.TryGetValue(name.Value, out var n) ? n : $"_{_counter++}";
        scope.Define(name.Value, newName);
        name.Value = newName;
    }

    public override void Visit(LocalFunction node)
    {
        RenameAndDefine(_scopes.Peek(), node.Name);
        var scope = PushScope();

        foreach (var name in node.Parameters.Names)
            RenameAndDefine(scope, name);

        Visit(node.Body);
        PopScope();
    }

    public override void Visit(Function node)
    {
        RenameAndDefine(_scopes.Peek(), node.Name);
        var scope = PushScope();

        foreach (var name in node.Parameters.Names)
            RenameAndDefine(scope, name);

        Visit(node.Body);
        PopScope();
    }

    public override void Visit(AnonymousFunction node)
    {
        var scope = PushScope();

        foreach (var name in node.Parameters.Names)
            RenameAndDefine(scope, name);

        Visit(node.Body);
        PopScope();
    }

    public override void Visit(GenericFor node)
    {
        var scope = PushScope();

        foreach (var name in node.Names)
            RenameAndDefine(scope, name);

        Visit(node.Body);
        PopScope();
    }

    public override void Visit(NumericFor node)
    {
        Visit(node.InitialValue);
        Visit(node.FinalValue);

        if (node.Step != null)
            Visit(node.Step);

        RenameAndDefine(PushScope(), node.IteratorName);
        Visit(node.Body);
        PopScope();
    }

    public override void Visit(Repeat node)
    {
        PushScope();
        Visit(node.Body);
        Visit(node.Condition);
        PopScope();
    }

    public override void Visit(While node)
    {
        Visit(node.Condition);
        PushScope();
        Visit(node.Body);
        PopScope();
    }

    public override void Visit(Do node)
    {
        PushScope();
        Visit(node.Body);
        PopScope();
    }

    public override void Visit(If node)
    {
        Visit(node.IfClause);
        VisitList(node.ElseIfClauses);

        var elseBody = node.ElseBody;

        if (elseBody != null)
        {
            PushScope();
            Visit(elseBody);
            PopScope();
        }
    }

    public override void Visit(If.Clause node)
    {
        Visit(node.Condition);
        PushScope();
        Visit(node.Body);
        PopScope();
    }

    public override void Visit(LocalDeclare node)
    {
        VisitList(node.Values);
        var scope = _scopes.Peek();

        foreach (var name in node.Names)
            RenameAndDefine(scope, name);
    }

    public override void Visit(Assign node)
    {
        VisitList(node.Values);
        VisitList(node.Variables);
    }

    public override void Visit(Name node)
    {
        var value = node.Value;

        foreach (var scope in _scopes)
        {
            if (!scope.IsDefined(value))
                continue;

            node.Value = scope.GetNewName(value);
            break;
        }
    }

    private class Scope
    {
        private readonly Dictionary<string, Stack<string>> _variables = [];

        public void Define(string name, string newName)
        {
            if (!_variables.ContainsKey(name))
                _variables[name] = [];

            _variables[name].Push(newName);
        }

        public bool IsDefined(string name) => _variables.ContainsKey(name);

        public string GetNewName(string name) => _variables[name].Peek();
    }
}