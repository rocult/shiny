namespace MoonsecDeobfuscator.Ast;

public interface IRewriteDispatch
{
    void Rewrite(Node node, bool symbols, bool fixedPoint);
}