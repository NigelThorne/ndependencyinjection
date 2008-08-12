namespace QuickGraph.Algorithms.TestGames
{
    using QuickGraph.Concepts;
    using System;

    public interface IStrategy : IBoundedReachabilityGamePlayer
    {
        void SetChooseEdge(IVertex v, int k, IEdge e);
    }
}

