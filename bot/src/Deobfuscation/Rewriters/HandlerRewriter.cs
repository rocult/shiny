using MoonsecDeobfuscator.Ast;
using MoonsecDeobfuscator.Ast.Expressions;
using MoonsecDeobfuscator.Ast.Statements;

namespace MoonsecDeobfuscator.Deobfuscation.Rewriters;

// Hyi vittu kehtaako tätä edes julkaista...?
public class HandlerRewriter : AstRewriter
{
    public override Node Visit(Call node)
    {
        var args = node.Arguments;

        // stk(_201, _200) => stk(inst[OP_A], inst[OP_B])
        if (node.Function is Name { Value: "stk" } && args is [Name name1, Name name2])
        {
            var newArg1 = ReplaceWithElementAccess(name1);
            var newArg2 = ReplaceWithElementAccess(name2);

            if (name1 != newArg1 || name2 != newArg2)
                return new Call(node.Function, [newArg1, newArg2]);
        }

        return node;
    }

    public override Node Visit(ElementAccess node)
    {
        var newTable = ReplaceWithName(node.Table);
        var newKey = ReplaceWithName(node.Key);

        /*
            local _186 = _184[_185] => local _186 = inst[OP_A]
        */
        if (node.Table != newTable || node.Key != newKey)
            return new ElementAccess(newTable, newKey);

        /*
            stk[_185] = _186 => stk[inst[OP_A]] = _186
        */
        newKey = ReplaceWithElementAccess(node.Key);

        if (node.Key != newKey)
            return new ElementAccess(newTable, newKey);

        return node;
    }


    public override Node Visit(Block node)
    {
        var unused = node.Statements
            .Where(stat => stat is LocalDeclare decl && ShouldRemoveDeclaration(decl))
            .ToHashSet();

        if (unused.Count > 0)
        {
            node.Statements.RemoveAll(stat => unused.Contains(stat));

            // make sure changes are detected
            return new Block(node.Statements);
        }

        return node;
    }

    public override Node Visit(Assign node)
    {
        /*
            _185 = OP_A => local _185 = OP_A
        */
        if (ShouldLocalizeAssignment(node))
        {
            var names = new NodeList<Name>(node.Variables.Select(name => (Name) name));
            return new LocalDeclare(names, node.Values);
        }

        /*
            stk[_186] = _185 => stk[_186] = stk[inst[OP_B]]
        */
        if (node is { Variables: [ElementAccess], Values: [Name name] })
        {
            var replacement = ReplaceWithElementAccess(name);

            if (replacement != name)
                return new Assign(node.Variables, [replacement]);
        }

        /*
             local _275 = (((stk[inst[OP_A]] == stk[inst[OP_C]]) and inst[OP_B]) or (1 + pc))
             pc = _275

             => pc = (((stk[inst[OP_A]] == stk[inst[OP_C]]) and inst[OP_B]) or (1 + pc))
        */
        if (node is { Variables: [Name], Values: [Name name1] })
        {
            var newValue = ReplaceWithBinaryExpression(name1);

            if (name1 != newValue)
                return new Assign(node.Variables, [newValue]);
        }

        return node;
    }


    public override Node Visit(BinaryExpression node)
    {
        var newLeft = ReplaceWithElementAccess(node.Left);
        var newRight = ReplaceWithElementAccess(node.Right);

        /*
             (((_273 == _274) and inst[OP_B]) or (1 + _269)) => (((stk[inst[OP_A]] == stk[inst[OP_C]]) and inst[OP_B]) or (1 + _269))
        */
        if (node.Left != newLeft || node.Right != newRight)
            return new BinaryExpression(node.Operator, newLeft, newRight);

        newLeft = ReplaceWithName(node.Left);
        newRight = ReplaceWithName(node.Right);

        /*
             (1 + _269) => (1 + pc)
        */
        if (node.Left != newLeft || node.Right != newRight)
            return new BinaryExpression(node.Operator, newLeft, newRight);

        return node;
    }

    private bool ShouldRemoveDeclaration(LocalDeclare node)
    {
        if (node.Values.Count == 0)
            return true;

        foreach (var name in node.Names)
        {
            var info = GetVariable(name.Value);

            if (info is { ReadCount: > 0 })
                return false;
        }

        return true;
    }

    private bool ShouldLocalizeAssignment(Assign node)
    {
        foreach (var variable in node.Variables)
        {
            if (variable is not Name name)
                return false;

            var value = name.Value;

            if (!value.StartsWith('_'))
                return false;

            if (GetVariable(value)!.DeclarationLocation != node)
                return false;
        }

        return true;
    }

    private Expression ReplaceWithName(Expression node)
    {
        if (node is not Name name || !name.Value.StartsWith('_'))
            return node;

        return GetVariable(name.Value) is { Value: Name } info
            ? info.Value.Clone()
            : node;
    }

    private Expression ReplaceWithElementAccess(Expression node)
    {
        if (node is not Name name || !name.Value.StartsWith('_'))
            return node;

        return GetVariable(name.Value) is { Value: ElementAccess { Table: Name, Key: Name }, ReadCount: 1 } info
            ? info.Value.Clone()
            : node;
    }

    private Expression ReplaceWithBinaryExpression(Expression node)
    {
        if (node is not Name name || !name.Value.StartsWith('_'))
            return node;

        return GetVariable(name.Value) is { Value: BinaryExpression, ReadCount: 1, AssignmentCount: 0 } info
            ? info.Value.Clone()
            : node;
    }
}