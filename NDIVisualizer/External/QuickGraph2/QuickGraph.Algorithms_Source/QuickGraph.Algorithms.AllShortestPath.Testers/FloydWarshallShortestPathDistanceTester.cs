namespace QuickGraph.Algorithms.AllShortestPath.Testers
{
    using QuickGraph.Concepts;
    using QuickGraph.Concepts.Collections;
    using System;

    public class FloydWarshallShortestPathDistanceTester : IFloydWarshallDistanceTester
    {
        public bool TestDistance(IVertexDistanceMatrix distances, IVertex source, IVertex target, IVertex intermediate)
        {
            return ((distances.Distance(source, intermediate) + distances.Distance(intermediate, target)) < distances.Distance(source, target));
        }
    }
}

