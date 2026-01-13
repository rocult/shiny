using System.Text;
using MoonsecDeobfuscator.Ast;
using MoonsecDeobfuscator.Ast.Literals;
using MoonsecDeobfuscator.Ast.Statements;
using MoonsecDeobfuscator.Bytecode.Models;
using MoonsecDeobfuscator.Deobfuscation.Bytecode;
using MoonsecDeobfuscator.Deobfuscation.Rewriters;
using MoonsecDeobfuscator.Deobfuscation.Utils;
using MoonsecDeobfuscator.Deobfuscation.Walkers;
using NLua;
using Function = MoonsecDeobfuscator.Bytecode.Models.Function;

namespace MoonsecDeobfuscator.Deobfuscation;

public class Deobfuscator
{
    private Context _ctx = new();
    private Block _root = null!;

    public Function Deobfuscate(string code, bool antiTamper = false)
    {
        _root = Block.FromString(code);

        new Renamer().Walk(_root);
        new ConstantReplacer(DecodeConstants()).Rewrite(_root, Order.PreOrder);
        new ConstantFolder().Rewrite(_root, Order.PostOrder);
        new Analyzer(_ctx).Walk(_root, symbols: true);

        if (_root.Statements.Count > 3 && !antiTamper)
            return Deobfuscate(HandleAntiTamper(), true);

        new ControlFlowSolver().Rewrite(_ctx.Wrapper, Order.PostOrder);
        new Renamer(_ctx.IdentifiedNames).Walk(_root);

        var function = DeserializeBytecode();
        var bytecodeDeobfuscator = new BytecodeDeobfuscator(function, _ctx);

        function = bytecodeDeobfuscator.Deobfuscate();

        return function;
    }

    private Dictionary<string, string[]> DecodeConstants()
    {
        var constants = StringCollector.Collect(_root);

        return constants
            .Select(it => it.Item1 != null
                ? StringDecoding.Decode(it.Item2, (int) it.Item1)
                : StringDecoding.DecodeEscape(it.Item2))
            .Select(StringDecoding.DecodeConstants)
            .SelectMany(map => map)
            .ToDictionary(entry => entry.Key, entry => entry.Value);
    }

    private string HandleAntiTamper()
    {
        var function = DeserializeBytecode();
        var instructions = function.Instructions
            .Where(instr => instr is { IsKA: false, IsKB: true, IsKC: false })
            .ToList();

        // why tf is this static
        var constant = function.Constants[instructions[^2].B - 1];
        var key = ((NumberConstant) constant).Value;

        var bytecodeString = ((StringLiteral) ((Assign) _root.Statements[2]).Values[0]).Value[1..^1];
        var scriptString = ((StringLiteral) ((Assign) _root.Statements[3]).Values[0]).Value[1..^1];

        _ctx = new Context
        {
            BytecodeString = bytecodeString
        };

        return Encoding.UTF8.GetString(StringDecoding.Decode(scriptString, (int) key));
    }

    private Function DeserializeBytecode()
    {
        SolveKeys();

        var bytecode = StringDecoding.Decode(_ctx.BytecodeString, _ctx.BytecodeKey);
        var deserializer = new Deserializer(bytecode, _ctx);

        return deserializer.ReadFunction();
    }

    private void SolveKeys()
    {
        using var L = new Lua(openLibs: false);

        if (_ctx.KeyExpressions.Count == 2)
        {
            var code = $"return {PrettyPrinter.AsString(_ctx.KeyExpressions.First())}";
            _ctx.ConstantKey = Convert.ToInt32(L.DoString(code)[0]);
        }

        {
            var code = $"return {PrettyPrinter.AsString(_ctx.KeyExpressions.Last())}";
            _ctx.BytecodeKey = Convert.ToInt32(L.DoString(code)[0]);
        }
    }
}