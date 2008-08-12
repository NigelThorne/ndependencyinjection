namespace QuickGraph.Algorithms.Layout
{
    using QuickGraph.Concepts.Algorithms;
    using System;
    using System.Runtime.CompilerServices;

    public interface IIteratedLayoutAlgorithm : ILayoutAlgorithm, IAlgorithm
    {
        event EventHandler PostIteration;

        event EventHandler PreIteration;

        void Iterate();

        int CurrentIteration { get; }

        object SyncRoot { get; }
    }
}

