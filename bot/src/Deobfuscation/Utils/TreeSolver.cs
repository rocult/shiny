using MoonsecDeobfuscator.Ast;
using MoonsecDeobfuscator.Ast.Expressions;
using MoonsecDeobfuscator.Ast.Literals;
using MoonsecDeobfuscator.Ast.Statements;
using QuikGraph;

namespace MoonsecDeobfuscator.Deobfuscation.Utils;

// Using QuikGraph is probably not necessary, but I built it this way because I assumed I would need QuikGraph for a control flow graph of the bytecode.
public class TreeSolver(If tree, string stateName)
{
    private enum EdgeType
    {
        Then,
        Else
    }

    private class Range(int min, int max)
    {
        public int Min = min;
        public int Max = max;
    }

    private class Vertex
    {
        public BinaryOperator? Operator;
        public int? ConstantValue;

        public Range? Range;
        public Block? Body;
    }

    private readonly BidirectionalGraph<Vertex, TaggedEdge<Vertex, EdgeType>> _graph = new();

    public Dictionary<int, Block> Solve()
    {
        AddBranches(tree, null, EdgeType.Then);
        SetRanges();
        ReduceRanges();
        RemoveUnreachable();

        var result = new Dictionary<int, Block>();

        foreach (var vertex in _graph.Vertices.Where(IsTerminal))
        {
            var range = vertex.Range!;

            if (range.Min != range.Max)
                throw new Exception("Failed to reduce range!");

            result[range.Min] = vertex.Body!;
        }

        return result;
    }

    private void RemoveUnreachable()
    {
        var unreachable = _graph.Vertices
            .Where(v => IsTerminal(v) && v.Range is { Min: < 0, Max: < 0 })
            .ToHashSet();

        foreach (var vertex in unreachable)
            _graph.RemoveVertex(vertex);
    }

    private void ReduceRanges()
    {
        var vertices = _graph.Vertices.ToList();
        var root = vertices.First(v => _graph.InDegree(v) == 0);
        var leafs = vertices.Where(IsTerminal);

        foreach (var leaf in leafs)
        {
            var range = leaf.Range!;

            for (var i = range.Min; i <= range.Max; i++)
            {
                if (HasValidPath(leaf, root, i))
                {
                    range.Min = range.Max = i;
                    break;
                }

                if (i == range.Max)
                    range.Min = range.Max = -1;
            }
        }
    }

    private bool HasValidPath(Vertex current, Vertex root, int state)
    {
        if (current == root)
            return true;

        foreach (var edge in _graph.InEdges(current))
        {
            var source = edge.Source;

            if (IsValidPath(source, edge, state) && HasValidPath(source, root, state))
                return true;
        }

        return false;
    }

    private static bool IsValidPath(Vertex vertex, TaggedEdge<Vertex, EdgeType> edge, int state)
    {
        var constant = (int) vertex.ConstantValue!;
        var valid = vertex.Operator! switch
        {
            BinaryOperator.Equals => state == constant,
            BinaryOperator.NotEquals => state != constant,
            BinaryOperator.LessThanOrEquals => state <= constant,
            BinaryOperator.GreaterThanOrEquals => state >= constant,
            BinaryOperator.LessThan => state < constant,
            BinaryOperator.GreaterThan => state > constant,
            _ => throw new Exception("Invalid operator in IsValidPath!")
        };

        return edge.Tag == EdgeType.Then == valid;
    }

    private void SetRanges()
    {
        foreach (var node in _graph.Vertices)
        {
            if (IsTerminal(node))
                continue;

            var edges = _graph.OutEdges(node).ToList();

            if (edges.Count != 2)
                throw new Exception($"Expected 2 edges but got {edges.Count}");

            var thenEdge = edges.First(edge => edge.Tag == EdgeType.Then);
            var elseEdge = edges.First(edge => edge.Tag == EdgeType.Else);
            var ranges = ComputeRanges(node);

            var thenTarget = thenEdge.Target;
            var elseTarget = elseEdge.Target;

            if (IsTerminal(thenTarget))
                thenTarget.Range = ranges.Then;

            if (IsTerminal(elseTarget))
                elseTarget.Range = ranges.Else;
        }
    }

    private static (Range Then, Range Else) ComputeRanges(Vertex v)
    {
        var c = (int) v.ConstantValue!;

        return v.Operator! switch
        {
            BinaryOperator.Equals => (new Range(c, c), new Range(Math.Max(c - 1, 0), c + 1)),
            BinaryOperator.NotEquals => (new Range(Math.Max(c - 1, 0), c + 1), new Range(c, c)),
            BinaryOperator.LessThan => (new Range(Math.Max(c - 1, 0), Math.Max(c - 1, 0)), new Range(c, c)),
            BinaryOperator.GreaterThan => (new Range(c + 1, c + 1), new Range(c, c)),
            BinaryOperator.LessThanOrEquals => (new Range(c, c), new Range(c + 1, c + 1)),
            BinaryOperator.GreaterThanOrEquals => (new Range(c, c), new Range(Math.Max(c - 1, 0), Math.Max(c - 1, 0))),
            _ => throw new Exception("Invalid operator in ComputeRanges!")
        };
    }

    private void AddBranches(If branch, Vertex? source, EdgeType edgeType)
    {
        var condition = (BinaryExpression) branch.IfClause.Condition;
        var node = new Vertex
        {
            Operator = condition.Operator,
            ConstantValue = (int) ((NumberLiteral) condition.Right).Value
        };

        _graph.AddVertex(node);

        if (source != null)
            _graph.AddEdge(new TaggedEdge<Vertex, EdgeType>(source, node, edgeType));

        AddBranch(branch.IfClause.Body, node, EdgeType.Then);
        AddBranch(branch.ElseBody!, node, EdgeType.Else);
    }

    private void AddBranch(Block body, Vertex source, EdgeType edgeType)
    {
        if (TreeContinues(body))
        {
            AddBranches((If) body.Statements[0], source, edgeType);
        }
        else
        {
            var leaf = new Vertex
            {
                Body = body,
            };

            _graph.AddVertex(leaf);
            _graph.AddEdge(new TaggedEdge<Vertex, EdgeType>(source, leaf, edgeType));
        }
    }

    private bool IsTerminal(Vertex vertex) => _graph.OutDegree(vertex) == 0;

    private bool TreeContinues(Block node) =>
        node.Statements is [If { IfClause.Condition: BinaryExpression { Left: Name name } }] && name.Value == stateName;
}