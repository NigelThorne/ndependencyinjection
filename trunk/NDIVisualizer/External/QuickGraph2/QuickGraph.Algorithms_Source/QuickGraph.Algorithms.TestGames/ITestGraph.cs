namespace QuickGraph.Algorithms.TestGames
{
    using QuickGraph.Concepts;
    using QuickGraph.Concepts.Collections;
    using QuickGraph.Concepts.Traversals;
    using System;

    public interface ITestGraph
    {
        bool ContainsChoicePoint(IVertex v);
        bool ContainsState(IVertex v);
        double Cost(IEdge e);
        double Prob(IEdge e);

        IVertexEnumerable ChoicePoints { get; }

        IBidirectionalVertexAndEdgeListGraph Graph { get; }

        IVertexEnumerable States { get; }
    }
}

