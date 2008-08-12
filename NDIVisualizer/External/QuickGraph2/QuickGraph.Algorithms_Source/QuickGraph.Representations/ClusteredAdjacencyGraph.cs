namespace QuickGraph.Representations
{
    using QuickGraph.Collections;
    using QuickGraph.Concepts;
    using QuickGraph.Concepts.Collections;
    using QuickGraph.Concepts.Modifications;
    using QuickGraph.Concepts.MutableTraversals;
    using QuickGraph.Concepts.Predicates;
    using QuickGraph.Concepts.Providers;
    using QuickGraph.Concepts.Serialization;
    using QuickGraph.Concepts.Traversals;
    using System;
    using System.Collections;

    public class ClusteredAdjacencyGraph : IFilteredVertexAndEdgeListGraph, IFilteredVertexListGraph, IFilteredEdgeListGraph, IFilteredIncidenceGraph, IMutableEdgeListGraph, IMutableIncidenceGraph, ISerializableVertexAndEdgeListGraph, ISerializableEdgeListGraph, ISerializableVertexListGraph, IMutableVertexAndEdgeListGraph, IVertexAndEdgeListGraph, IVertexListGraph, IEdgeListAndIncidenceGraph, IEdgeListGraph, IIncidenceGraph, IAdjacencyGraph, IImplicitGraph, IVertexAndEdgeMutableGraph, IEdgeMutableGraph, IVertexMutableGraph, IMutableGraph, IGraph, IClusteredGraph
    {
        private ArrayList clusters;
        private bool colapsed;
        private ClusteredAdjacencyGraph parent;
        private AdjacencyGraph wrapped;

        public ClusteredAdjacencyGraph(AdjacencyGraph wrapped)
        {
            if (wrapped == null)
            {
                throw new ArgumentNullException("parent");
            }
            this.parent = null;
            this.wrapped = wrapped;
            this.clusters = new ArrayList();
            this.colapsed = false;
        }

        public ClusteredAdjacencyGraph(ClusteredAdjacencyGraph parent)
        {
            if (parent == null)
            {
                throw new ArgumentNullException("parent");
            }
            this.parent = parent;
            this.wrapped = new AdjacencyGraph(this.Parent.VertexProvider, this.Parent.EdgeProvider, this.Parent.AllowParallelEdges);
            this.clusters = new ArrayList();
        }

        public ClusteredAdjacencyGraph AddCluster()
        {
            ClusteredAdjacencyGraph graph = new ClusteredAdjacencyGraph(this);
            this.clusters.Add(graph);
            return graph;
        }

        public virtual void AddEdge(IEdge e)
        {
            if (e == null)
            {
                throw new ArgumentNullException("e");
            }
            if ((this.Parent != null) && !this.Parent.ContainsEdge(e))
            {
                this.Parent.AddEdge(e);
            }
            this.Wrapped.AddEdge(e);
        }

        public virtual IEdge AddEdge(IVertex u, IVertex v)
        {
            if (u == null)
            {
                throw new ArgumentNullException("u");
            }
            if (v == null)
            {
                throw new ArgumentNullException("v");
            }
            if (!this.ContainsVertex(u))
            {
                throw new ArgumentException("graph does not contain u");
            }
            if (!this.ContainsVertex(v))
            {
                throw new ArgumentException("graph does not contain v");
            }
            if (this.Parent != null)
            {
                IEdge e = this.Parent.AddEdge(u, v);
                this.Wrapped.AddEdge(e);
                return e;
            }
            return this.Wrapped.AddEdge(u, v);
        }

        public virtual IVertex AddVertex()
        {
            if (this.Parent != null)
            {
                IVertex v = this.Parent.AddVertex();
                this.Wrapped.AddVertex(v);
                return v;
            }
            return this.Wrapped.AddVertex();
        }

        public virtual void AddVertex(IVertex v)
        {
            if (v == null)
            {
                throw new ArgumentNullException("v");
            }
            if ((this.Parent != null) && !this.Parent.ContainsVertex(v))
            {
                this.Parent.AddVertex(v);
            }
            this.Wrapped.AddVertex(v);
        }

        public IVertexEnumerable AdjacentVertices(IVertex v)
        {
            return new TargetVertexEnumerable(this.OutEdges(v));
        }

        public virtual void Clear()
        {
            this.wrapped.Clear();
            this.clusters.Clear();
        }

        public virtual void ClearVertex(IVertex u)
        {
            this.Parent.ClearVertex(u);
            this.Wrapped.ClearVertex(u);
        }

        public bool ContainsEdge(IEdge e)
        {
            return this.Wrapped.ContainsEdge(e);
        }

        public bool ContainsEdge(IVertex u, IVertex v)
        {
            return this.Wrapped.ContainsEdge(u, v);
        }

        public bool ContainsVertex(IVertex v)
        {
            return this.Wrapped.ContainsVertex(v);
        }

        public int OutDegree(IVertex v)
        {
            return this.Wrapped.OutDegree(v);
        }

        public IEdgeEnumerable OutEdges(IVertex v)
        {
            return this.Wrapped.OutEdges(v);
        }

        public bool OutEdgesEmpty(IVertex v)
        {
            return this.Wrapped.OutEdgesEmpty(v);
        }

        IClusteredGraph IClusteredGraph.AddCluster()
        {
            return this.AddCluster();
        }

        public void RemoveCluster(IClusteredGraph cluster)
        {
            if (cluster == null)
            {
                throw new ArgumentNullException("cluster");
            }
            this.clusters.Remove(cluster);
        }

        public virtual void RemoveEdge(IEdge e)
        {
            if (e == null)
            {
                throw new ArgumentNullException("e");
            }
            this.Wrapped.RemoveEdge(e);
            if (this.Parent != null)
            {
                this.Parent.RemoveEdge(e);
            }
        }

        public virtual void RemoveEdge(IVertex u, IVertex v)
        {
            if (u == null)
            {
                throw new ArgumentNullException("u");
            }
            if (v == null)
            {
                throw new ArgumentNullException("v");
            }
            this.Wrapped.RemoveEdge(u, v);
            if (this.Parent != null)
            {
                this.Parent.RemoveEdge(u, v);
            }
        }

        public virtual void RemoveEdgeIf(IEdgePredicate ep)
        {
            if (ep == null)
            {
                throw new ArgumentNullException("ep");
            }
            this.Wrapped.RemoveEdgeIf(ep);
            if (this.Parent != null)
            {
                this.Parent.RemoveEdgeIf(ep);
            }
        }

        public virtual void RemoveOutEdgeIf(IVertex v, IEdgePredicate ep)
        {
            if (v == null)
            {
                throw new ArgumentNullException("v");
            }
            if (ep == null)
            {
                throw new ArgumentNullException("ep");
            }
            this.Wrapped.RemoveOutEdgeIf(v, ep);
            if (this.Parent != null)
            {
                this.Parent.RemoveOutEdgeIf(v, ep);
            }
        }

        public virtual void RemoveVertex(IVertex u)
        {
            if (u == null)
            {
                throw new ArgumentNullException("u");
            }
            this.Wrapped.RemoveVertex(u);
            if (this.Parent != null)
            {
                this.Parent.RemoveVertex(u);
            }
        }

        public IEdgeEnumerable SelectEdges(IEdgePredicate ep)
        {
            return this.Wrapped.SelectEdges(ep);
        }

        public IEdgeEnumerable SelectOutEdges(IVertex v, IEdgePredicate ep)
        {
            return this.Wrapped.SelectOutEdges(v, ep);
        }

        public IEdge SelectSingleEdge(IEdgePredicate ep)
        {
            return this.Wrapped.SelectSingleEdge(ep);
        }

        public IEdge SelectSingleOutEdge(IVertex v, IEdgePredicate ep)
        {
            return this.Wrapped.SelectSingleOutEdge(v, ep);
        }

        public IVertex SelectSingleVertex(IVertexPredicate vp)
        {
            return this.Wrapped.SelectSingleVertex(vp);
        }

        public IVertexEnumerable SelectVertices(IVertexPredicate vp)
        {
            return this.Wrapped.SelectVertices(vp);
        }

        public bool AllowParallelEdges
        {
            get
            {
                return this.Wrapped.AllowParallelEdges;
            }
        }

        public IEnumerable Clusters
        {
            get
            {
                return this.clusters;
            }
        }

        public int ClustersCount
        {
            get
            {
                return this.clusters.Count;
            }
        }

        public bool Colapsed
        {
            get
            {
                return this.colapsed;
            }
            set
            {
                this.colapsed = value;
            }
        }

        public IEdgeProvider EdgeProvider
        {
            get
            {
                return this.Wrapped.EdgeProvider;
            }
        }

        public IEdgeEnumerable Edges
        {
            get
            {
                return this.Wrapped.Edges;
            }
        }

        public int EdgesCount
        {
            get
            {
                return this.Wrapped.EdgesCount;
            }
        }

        public bool EdgesEmpty
        {
            get
            {
                return this.Wrapped.EdgesEmpty;
            }
        }

        public bool IsDirected
        {
            get
            {
                return this.Wrapped.IsDirected;
            }
        }

        public ClusteredAdjacencyGraph Parent
        {
            get
            {
                return this.parent;
            }
        }

        public IVertexProvider VertexProvider
        {
            get
            {
                return this.Wrapped.VertexProvider;
            }
        }

        public IVertexEnumerable Vertices
        {
            get
            {
                return this.Wrapped.Vertices;
            }
        }

        public int VerticesCount
        {
            get
            {
                return this.Wrapped.VerticesCount;
            }
        }

        public bool VerticesEmpty
        {
            get
            {
                return this.Wrapped.VerticesEmpty;
            }
        }

        protected AdjacencyGraph Wrapped
        {
            get
            {
                return this.wrapped;
            }
        }
    }
}

