namespace QuickGraph.Algorithms.AllShortestPath.Reducers
{
    using QuickGraph.Concepts;
    using QuickGraph.Concepts.Collections;
    using System;

    public interface IFloydWarshallDistanceReducer
    {
        void ReducePathDistance(IVertexDistanceMatrix distances, IVertex source, IVertex target, IVertex intermediate);
    }
}

