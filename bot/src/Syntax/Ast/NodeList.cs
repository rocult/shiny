namespace MoonsecDeobfuscator.Ast;

public class NodeList<TNode> : List<TNode> where TNode : Node
{
    public NodeList()
    {
    }

    public NodeList(int capacity) : base(capacity)
    {
    }

    public NodeList(IEnumerable<TNode> collection) : base(collection)
    {
    }

    public NodeList<TNode> Clone()
    {
        var newList = new NodeList<TNode>(Count);

        foreach (var element in this)
            newList.Add((TNode) element.Clone());

        return newList;
    }
}