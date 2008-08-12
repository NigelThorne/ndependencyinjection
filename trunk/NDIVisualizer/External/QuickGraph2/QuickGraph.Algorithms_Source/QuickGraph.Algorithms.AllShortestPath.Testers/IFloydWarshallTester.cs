namespace QuickGraph.Algorithms.AllShortestPath.Testers
{
    using QuickGraph.Concepts;
    using System;

    public interface IFloydWarshallTester
    {
        bool Test(IVertex source, IVertex target, IVertex intermediate);
    }
}

