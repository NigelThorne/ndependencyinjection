namespace QuickGraph.Algorithms.Layout
{
    using QuickGraph.Concepts.Algorithms;
    using QuickGraph.Concepts.Collections;
    using QuickGraph.Concepts.Traversals;
    using System;
    using System.Drawing;
    using System.Runtime.CompilerServices;

    public interface ILayoutAlgorithm : IAlgorithm
    {
        event EventHandler PostCompute;

        event EventHandler PreCompute;

        void Compute();
        void RequestComputationAbortion();
        void UpdateEdgeLength();

        float EdgeLength { get; set; }

        Size LayoutSize { get; set; }

        IVertexPointFDictionary Positions { get; }

        IVertexAndEdgeListGraph VisitedGraph { get; }
    }
}

