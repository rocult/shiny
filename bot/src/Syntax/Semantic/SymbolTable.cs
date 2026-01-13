namespace MoonsecDeobfuscator.Syntax.Semantic;

public class SymbolTable
{
    private readonly Dictionary<string, VariableInfo> _variables = [];

    public void Define(string name, VariableInfo variable)
    {
        _variables[name] = variable;
    }

    public bool IsDefined(string name) => _variables.ContainsKey(name);

    public VariableInfo? GetVariable(string name) => _variables.GetValueOrDefault(name);
}