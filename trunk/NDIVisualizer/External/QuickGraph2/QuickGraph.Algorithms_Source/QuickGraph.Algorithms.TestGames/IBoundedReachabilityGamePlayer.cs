namespace QuickGraph.Algorithms.TestGames
{
    using QuickGraph.Concepts;
    using System;

    public interface IBoundedReachabilityGamePlayer
    {
        IEdge ChooseEdge(IVertex state, int i);
    }
}

