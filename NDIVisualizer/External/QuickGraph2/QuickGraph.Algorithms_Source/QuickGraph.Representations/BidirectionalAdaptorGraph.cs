namespace QuickGraph.Representations
{
    using QuickGraph.Collections;
    using QuickGraph.Concepts;
    using QuickGraph.Concepts.Collections;
    using QuickGraph.Concepts.Traversals;
    using QuickGraph.Predicates;
    using System;

    public class BidirectionalAdaptorGraph : IBidirectionalVertexAndEdgeListGraph, IBidirectionalVertexListGraph, IBidirectionalGraph, IVertexAndEdgeListGraph, IVertexListGraph, IEdgeListAndIncidenceGraph, IEdgeListGraph, IIncidenceGraph, IAdjacencyGraph, IImplicitGraph, IGraph
    {
        private IVertexAndEdgeListGraph graph;

        public BidirectionalAdaptorGraph(IVertexAndEdgeListGraph g)
        {
            if (g == null)
            {
                throw new ArgumentNullException("grah");
            }
            this.graph = g;
        }

        public bool AdjacentEdgesEmpty(IVertex v)
        {
            return (this.OutEdgesEmpty(v) && this.InEdgesEmpty(v));
        }

        public IVertexEnumerable AdjacentVertices(IVertex v)
        {
            return new TargetVertexEnumerable(this.OutEdges(v));
        }

        public bool ContainsEdge(IEdge e)
        {
            return this.graph.ContainsEdge(e);
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
            return this.Graph.ContainsEdge(u, v);
        }

        public bool ContainsVertex(IVertex v)
        {
            if (v == null)
            {
                throw new ArgumentNullException("v");
            }
            return this.Graph.ContainsVertex(v);
        }

        public int Degree(IVertex v)
        {
            return (this.OutDegree(v) + this.InDegree(v));
        }

        public int InDegree(IVertex v)
        {
            IEdgeEnumerator enumerator = this.InEdges(v).GetEnumerator();
            int num = 0;
            while (enumerator.MoveNext())
            {
                num++;
            }
            return num;
        }

        public IEdgeEnumerable InEdges(IVertex v)
        {
            return new FilteredEdgeEnumerable(this.Graph.get_Edges(), new IsInEdgePredicate(v));
        }

        public bool InEdgesEmpty(IVertex v)
        {
            return (this.InDegree(v) == 0);
        }

        public int OutDegree(IVertex v)
        {
            return this.Graph.OutDegree(v);
        }

        public IEdgeEnumerable OutEdges(IVertex v)
        {
            return this.Graph.OutEdges(v);
        }

        public bool OutEdgesEmpty(IVertex v)
        {
            return this.Graph.OutEdgesEmpty(v);
        }

        public bool AllowParallelEdges
        {
            get
            {
                return this.Graph.get_AllowParallelEdges();
            }
        }

        public IEdgeEnumerable Edges
        {
            get
            {
                return this.graph.get_Edges();
            }
        }

        public int EdgesCount
        {
            get
            {
                return this.graph.get_EdgesCount();
            }
        }

        public bool EdgesEmpty
        {
            get
            {
                return this.graph.get_EdgesEmpty();
            }
        }

        public IVertexAndEdgeListGraph Graph
        {
            get
            {
                return this.graph;
            }
        }

        public bool IsDirected
        {
            get
            {
                return this.Graph.get_IsDirected();
            }
        }

        public IVertexEnumerable Vertices
        {
            get
            {
                return this.graph.get_Vertices();
            }
        }

        public int VerticesCount
        {
            get
            {
                return this.graph.get_VerticesCount();
            }
        }

        public bool VerticesEmpty
        {
            get
            {
                return this.graph.get_VerticesEmpty();
            }
        }
    }
}

