using System.Collections.Immutable;

namespace MoonsecDeobfuscator.Ast;

public abstract class Node
{
    public virtual ImmutableList<Node> ChildNodes() => [];

    public IEnumerable<Node> DescendantNodes()
    {
        foreach (var child in ChildNodes())
        {
            yield return child;

            foreach (var descendant in child.DescendantNodes())
                yield return descendant;
        }
    }

    public abstract Node Clone();

    public abstract void Accept(AstWalker visitor);

    public abstract Node Accept(AstRewriter visitor);
}