namespace QuickGraph.Algorithms.AllShortestPath.Reducers
{
    using QuickGraph.Concepts;
    using QuickGraph.Concepts.Collections;
    using System;

    public class FloydWarshallMaxMinDistanceReducer : IFloydWarshallDistanceReducer
    {
        public void ReducePathDistance(IVertexDistanceMatrix distances, IVertex source, IVertex target, IVertex intermediate)
        {
            distances.SetDistance(source, target, Math.Max(distances.Distance(source, target), Math.Min(distances.Distance(source, intermediate), distances.Distance(intermediate, target))));
        }
    }
}

