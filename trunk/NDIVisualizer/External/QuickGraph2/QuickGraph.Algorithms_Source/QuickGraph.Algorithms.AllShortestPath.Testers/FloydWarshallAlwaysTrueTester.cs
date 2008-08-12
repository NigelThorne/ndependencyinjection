namespace QuickGraph.Algorithms.AllShortestPath.Testers
{
    using QuickGraph.Concepts;
    using System;

    public class FloydWarshallAlwaysTrueTester : IFloydWarshallTester
    {
        public bool Test(IVertex source, IVertex target, IVertex intermediate)
        {
            return true;
        }
    }
}

