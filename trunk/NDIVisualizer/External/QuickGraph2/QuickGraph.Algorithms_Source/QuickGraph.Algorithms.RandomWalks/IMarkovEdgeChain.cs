namespace QuickGraph.Algorithms.RandomWalks
{
    using QuickGraph.Concepts;
    using QuickGraph.Concepts.Traversals;

    public interface IMarkovEdgeChain
    {
        IEdge Successor(IImplicitGraph g, IVertex u);
    }
}

