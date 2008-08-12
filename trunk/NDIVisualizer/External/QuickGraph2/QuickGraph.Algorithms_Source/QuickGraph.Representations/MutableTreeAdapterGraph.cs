namespace QuickGraph.Representations
{
    using QuickGraph.Algorithms;
    using QuickGraph.Collections;
    using QuickGraph.Concepts;
    using QuickGraph.Concepts.Modifications;
    using QuickGraph.Concepts.MutableTraversals;
    using System;

    public class MutableTreeAdapterGraph : TreeAdaptorGraph, IMutableTreeGraph, IMutableGraph, IGraph
    {
        private bool allowCycles;
        private IMutableBidirectionalVertexAndEdgeListGraph mutableWrapped;

        public MutableTreeAdapterGraph(IMutableBidirectionalVertexAndEdgeListGraph g, bool allowCycles) : base(g)
        {
            this.mutableWrapped = g;
            this.allowCycles = allowCycles;
        }

        public virtual IVertex AddChild(IVertex parent)
        {
            IVertex vertex = this.MutableWrapped.AddVertex();
            this.MutableWrapped.AddEdge(parent, vertex);
            if (!this.allowCycles)
            {
                QuickGraph.Algorithms.AlgoUtility.CheckAcyclic(this.MutableWrapped, parent);
            }
            return vertex;
        }

        public virtual void Clear()
        {
            this.MutableWrapped.Clear();
        }

        bool IGraph.get_AllowParallelEdges()
        {
            return false;
        }

        bool IGraph.get_IsDirected()
        {
            return true;
        }

        public virtual void RemoveTree(IVertex root)
        {
            VertexCollection.Enumerator enumerator = Representation.OutVertexTree(this.MutableWrapped, root, 0x7fffffff).GetEnumerator();
            while (enumerator.MoveNext())
            {
                IVertex vertex = enumerator.get_Current();
                this.MutableWrapped.ClearVertex(vertex);
                this.MutableWrapped.RemoveVertex(vertex);
            }
        }

        public bool AllowCycles
        {
            get
            {
                return this.allowCycles;
            }
        }

        protected IMutableBidirectionalVertexAndEdgeListGraph MutableWrapped
        {
            get
            {
                return this.mutableWrapped;
            }
        }
    }
}

