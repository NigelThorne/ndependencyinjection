namespace QuickGraph.Algorithms.Layout
{
    using QuickGraph.Concepts.Algorithms;
    using System;

    public interface IForceDirectedLayoutAlgorithm : IIteratedLayoutAlgorithm, ILayoutAlgorithm, IAlgorithm
    {
        IPotential Potential { get; set; }

        ILayoutAlgorithm PreLayoutAlgorithm { get; set; }
    }
}

