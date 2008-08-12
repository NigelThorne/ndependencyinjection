namespace QuickGraph.Representations
{
    using QuickGraph.Collections;
    using QuickGraph.Concepts;
    using QuickGraph.Concepts.Collections;
    using QuickGraph.Concepts.Traversals;
    using System;

    public class ReversedBidirectionalGraph : IBidirectionalVertexAndEdgeListGraph, IBidirectionalVertexListGraph, IBidirectionalGraph, IVertexAndEdgeListGraph, IVertexListGraph, IEdgeListAndIncidenceGraph, IEdgeListGraph, IIncidenceGraph, IAdjacencyGraph, IImplicitGraph, IGraph
    {
        private IBidirectionalVertexAndEdgeListGraph wrapped;

        public ReversedBidirectionalGraph(IBidirectionalVertexAndEdgeListGraph g)
        {
            if (g == null)
            {
                throw new ArgumentNullException("g");
            }
            this.wrapped = g;
        }

        public bool AdjacentEdgesEmpty(IVertex v)
        {
            return this.ReversedGraph.AdjacentEdgesEmpty(v);
        }

        public IVertexEnumerable AdjacentVertices(IVertex v)
        {
            return new TargetVertexEnumerable(this.InEdges(v));
        }

        public bool ContainsEdge(IEdge e)
        {
            return this.wrapped.ContainsEdge(e);
        }

        public bool ContainsEdge(IVertex u, IVertex v)
        {
            if (u == null)
            {
                throw new ArgumentNullException("u");
            }
            if (v == null)
            {
                throw new ArgumentNullException("v");
            }
            return this.ReversedGraph.ContainsEdge(v, u);
        }

        public bool ContainsVertex(IVertex v)
        {
            return this.ReversedGraph.ContainsVertex(v);
        }

        public int Degree(IVertex v)
        {
            return this.ReversedGraph.Degree(v);
        }

        public int InDegree(IVertex v)
        {
            return this.ReversedGraph.OutDegree(v);
        }

        public IEdgeEnumerable InEdges(IVertex v)
        {
            return new ReversedEdgeEnumerable(this.ReversedGraph.OutEdges(v));
        }

        public bool InEdgesEmpty(IVertex v)
        {
            if (v == null)
            {
                throw new ArgumentNullException("v");
            }
            return this.ReversedGraph.OutEdgesEmpty(v);
        }

        public int OutDegree(IVertex v)
        {
            return this.ReversedGraph.InDegree(v);
        }

        public IEdgeEnumerable OutEdges(IVertex v)
        {
            return new ReversedEdgeEnumerable(this.ReversedGraph.InEdges(v));
        }

        public bool OutEdgesEmpty(IVertex v)
        {
            return this.ReversedGraph.InEdgesEmpty(v);
        }

        public bool AllowParallelEdges
        {
            get
            {
                return this.ReversedGraph.get_AllowParallelEdges();
            }
        }

        public IEdgeEnumerable Edges
        {
            get
            {
                return new ReversedEdgeEnumerable(this.wrapped.get_Edges());
            }
        }

        public int EdgesCount
        {
            get
            {
                return this.wrapped.get_EdgesCount();
            }
        }

        public bool EdgesEmpty
        {
            get
            {
                return this.wrapped.get_EdgesEmpty();
            }
        }

        public bool IsDirected
        {
            get
            {
                return this.ReversedGraph.get_IsDirected();
            }
        }

        public IBidirectionalVertexAndEdgeListGraph ReversedGraph
        {
            get
            {
                return this.wrapped;
            }
        }

        public IVertexEnumerable Vertices
        {
            get
            {
                return this.ReversedGraph.get_Vertices();
            }
        }

        public int VerticesCount
        {
            get
            {
                return this.ReversedGraph.get_VerticesCount();
            }
        }

        public bool VerticesEmpty
        {
            get
            {
                return this.ReversedGraph.get_VerticesEmpty();
            }
        }
    }
}

