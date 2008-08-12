namespace QuickGraph.Algorithms.Layout
{
    using QuickGraph.Concepts.Collections;
    using System;

    public interface IPotential
    {
        void Compute(IVertexPointFDictionary potentials);

        IIteratedLayoutAlgorithm Algorithm { get; set; }
    }
}

