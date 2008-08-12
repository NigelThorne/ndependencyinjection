namespace QuickGraph.Algorithms.AllShortestPath.Reducers
{
    using QuickGraph.Concepts;
    using QuickGraph.Concepts.Collections;
    using System;

    public class FloydWarshallShortestPathDistanceReducer : IFloydWarshallDistanceReducer
    {
        public void ReducePathDistance(IVertexDistanceMatrix distances, IVertex source, IVertex target, IVertex intermediate)
        {
            distances.SetDistance(source, target, distances.Distance(source, intermediate) + distances.Distance(intermediate, target));
        }
    }
}

