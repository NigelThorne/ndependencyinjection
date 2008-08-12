namespace QuickGraph.Representations
{
    using QuickGraph.Concepts;
    using QuickGraph.Concepts.Collections;
    using QuickGraph.Concepts.Traversals;
    using System;

    public class EdgeList : IEdgeListGraph, IGraph
    {
        private bool allowParallelEdges;
        private IEdgeCollection edges;
        private bool isDirected;

        public EdgeList(IEdgeCollection edges, bool isDirected, bool allowParallelEdges)
        {
            if (edges == null)
            {
                throw new ArgumentNullException("edge collection");
            }
            this.edges = edges;
            this.isDirected = isDirected;
            this.allowParallelEdges = allowParallelEdges;
        }

        public bool ContainsEdge(IEdge e)
        {
            if (e == null)
            {
                throw new ArgumentNullException("e");
            }
            IEdgeEnumerator enumerator = this.Edges.GetEnumerator();
            while (enumerator.MoveNext())
            {
                if (enumerator.get_Current() == e)
                {
                    return true;
                }
            }
            return false;
        }

        IEdgeEnumerable IEdgeListGraph.get_Edges()
        {
            return this.Edges;
        }

        public bool AllowParallelEdges
        {
            get
            {
                return this.allowParallelEdges;
            }
        }

        public IEdgeCollection Edges
        {
            get
            {
                return this.edges;
            }
        }

        public int EdgesCount
        {
            get
            {
                return this.edges.Count;
            }
        }

        public bool EdgesEmpty
        {
            get
            {
                return (this.edges.Count == 0);
            }
        }

        public bool IsDirected
        {
            get
            {
                return this.isDirected;
            }
        }
    }
}

