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
    using QuickGraph.Exceptions;
    using QuickGraph.Predicates;
    using QuickGraph.Providers;
    using System;
    using System.Collections;

    public class AdjacencyGraph : IFilteredVertexAndEdgeListGraph, IFilteredVertexListGraph, IFilteredEdgeListGraph, IFilteredIncidenceGraph, IMutableEdgeListGraph, IMutableIncidenceGraph, ISerializableVertexAndEdgeListGraph, ISerializableEdgeListGraph, ISerializableVertexListGraph, IMutableVertexAndEdgeListGraph, IVertexAndEdgeListGraph, IEdgeListAndIncidenceGraph, IEdgeListGraph, IVertexAndEdgeMutableGraph, IEdgeMutableGraph, IVertexMutableGraph, IMutableGraph, IIndexedVertexListGraph, IVertexListGraph, IIndexedIncidenceGraph, IIncidenceGraph, IAdjacencyGraph, IImplicitGraph, IGraph
    {
        private bool allowParallelEdges;
        private IEdgeProvider edgeProvider;
        private VertexEdgesDictionary vertexOutEdges;
        private IVertexProvider vertexProvider;

        public AdjacencyGraph()
        {
            this.vertexProvider = new QuickGraph.Providers.VertexProvider();
            this.edgeProvider = new QuickGraph.Providers.EdgeProvider();
            this.allowParallelEdges = true;
            this.vertexOutEdges = new VertexEdgesDictionary();
        }

        public AdjacencyGraph(bool allowParallelEdges)
        {
            this.vertexProvider = new QuickGraph.Providers.VertexProvider();
            this.edgeProvider = new QuickGraph.Providers.EdgeProvider();
            this.allowParallelEdges = allowParallelEdges;
            this.vertexOutEdges = new VertexEdgesDictionary();
        }

        public AdjacencyGraph(IVertexProvider vertexProvider, IEdgeProvider edgeProvider, bool allowParallelEdges)
        {
            if (vertexProvider == null)
            {
                throw new ArgumentNullException("vertex provider");
            }
            if (edgeProvider == null)
            {
                throw new ArgumentNullException("edge provider");
            }
            this.vertexProvider = vertexProvider;
            this.edgeProvider = edgeProvider;
            this.allowParallelEdges = allowParallelEdges;
            this.vertexOutEdges = new VertexEdgesDictionary();
        }

        public virtual void AddEdge(IEdge e)
        {
            if (e == null)
            {
                throw new ArgumentNullException("vertex");
            }
            if (e.GetType() != this.EdgeProvider.get_EdgeType())
            {
                throw new ArgumentNullException("vertex type not valid");
            }
            if (!this.VertexOutEdges.ContainsKey(e.get_Source()))
            {
                throw new VertexNotFoundException("Could not find source vertex");
            }
            if (!this.VertexOutEdges.ContainsKey(e.get_Target()))
            {
                throw new VertexNotFoundException("Could not find target vertex");
            }
            if (!this.AllowParallelEdges && this.ContainsEdge(e.get_Source(), e.get_Target()))
            {
                throw new ArgumentException("graph does not allow duplicate edges");
            }
            this.EdgeProvider.UpdateEdge(e);
            this.VertexOutEdges.get_Item(e.get_Source()).Add(e);
        }

        public virtual IEdge AddEdge(IVertex source, IVertex target)
        {
            if (!this.VertexOutEdges.ContainsKey(source))
            {
                throw new VertexNotFoundException("Could not find source vertex");
            }
            if (!this.VertexOutEdges.ContainsKey(target))
            {
                throw new VertexNotFoundException("Could not find target vertex");
            }
            if (!this.AllowParallelEdges && this.ContainsEdge(source, target))
            {
                throw new Exception("Parallel edge not allowed");
            }
            IEdge edge = this.EdgeProvider.ProvideEdge(source, target);
            this.VertexOutEdges.get_Item(source).Add(edge);
            return edge;
        }

        public virtual IVertex AddVertex()
        {
            IVertex vertex = this.VertexProvider.ProvideVertex();
            this.VertexOutEdges.Add(vertex, new EdgeCollection());
            return vertex;
        }

        public virtual void AddVertex(IVertex v)
        {
            if (v == null)
            {
                throw new ArgumentNullException("vertex");
            }
            if (v.GetType() != this.VertexProvider.get_VertexType())
            {
                throw new ArgumentNullException("vertex type not valid");
            }
            if (this.VertexOutEdges.Contains(v))
            {
                throw new ArgumentException("vertex already in graph");
            }
            this.VertexProvider.UpdateVertex(v);
            this.VertexOutEdges.Add(v, new EdgeCollection());
        }

        public IVertexEnumerable AdjacentVertices(IVertex v)
        {
            return new TargetVertexEnumerable(this.OutEdges(v));
        }

        public virtual void Clear()
        {
            this.VertexOutEdges.Clear();
        }

        public virtual void ClearVertex(IVertex v)
        {
            if (v == null)
            {
                throw new ArgumentNullException("vertex");
            }
            this.RemoveEdgeIf(new IsAdjacentEdgePredicate(v));
            this.VertexOutEdges.get_Item(v).Clear();
        }

        public bool ContainsEdge(IEdge e)
        {
            IDictionaryEnumerator enumerator = this.VertexOutEdges.GetEnumerator();
            while (enumerator.MoveNext())
            {
                DictionaryEntry current = (DictionaryEntry) enumerator.Current;
                EdgeCollection edges = (EdgeCollection) current.Value;
                if (edges.Contains(e))
                {
                    return true;
                }
            }
            return false;
        }

        public bool ContainsEdge(IVertex u, IVertex v)
        {
            if (this.ContainsVertex(u))
            {
                if (!this.ContainsVertex(v))
                {
                    return false;
                }
                EdgeCollection.Enumerator enumerator = this.OutEdges(u).GetEnumerator();
                while (enumerator.MoveNext())
                {
                    if (enumerator.get_Current().get_Target() == v)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public bool ContainsVertex(IVertex v)
        {
            return this.VertexOutEdges.Contains(v);
        }

        public int OutDegree(IVertex v)
        {
            if (v == null)
            {
                throw new ArgumentNullException("v");
            }
            EdgeCollection edges = this.VertexOutEdges.get_Item(v);
            if (edges == null)
            {
                throw new VertexNotFoundException(v.ToString());
            }
            return edges.Count;
        }

        public EdgeCollection OutEdges(IVertex v)
        {
            if (v == null)
            {
                throw new ArgumentNullException("v");
            }
            EdgeCollection edges = this.VertexOutEdges.get_Item(v);
            if (edges == null)
            {
                throw new VertexNotFoundException(v.ToString());
            }
            return edges;
        }

        public bool OutEdgesEmpty(IVertex v)
        {
            if (v == null)
            {
                throw new ArgumentNullException("v");
            }
            return (this.VertexOutEdges.get_Item(v).Count == 0);
        }

        IEdgeEnumerable IEdgeListGraph.get_Edges()
        {
            return this.Edges;
        }

        IEdgeEnumerable IFilteredEdgeListGraph.SelectEdges(IEdgePredicate ep)
        {
            return this.SelectEdges(ep);
        }

        IEdgeEnumerable IFilteredIncidenceGraph.SelectOutEdges(IVertex v, IEdgePredicate ep)
        {
            return this.SelectOutEdges(v, ep);
        }

        IEdgeEnumerable IImplicitGraph.OutEdges(IVertex v)
        {
            return this.OutEdges(v);
        }

        IEdgeCollection IIndexedIncidenceGraph.OutEdges(IVertex v)
        {
            return this.OutEdges(v);
        }

        IVertexEnumerable IVertexListGraph.get_Vertices()
        {
            return this.Vertices;
        }

        public virtual void RemoveEdge(IEdge e)
        {
            if (e == null)
            {
                throw new ArgumentNullException("edge");
            }
            EdgeCollection edges = this.VertexOutEdges.get_Item(e.get_Source());
            if ((edges == null) || !edges.Contains(e))
            {
                throw new EdgeNotFoundException();
            }
            edges.Remove(e);
        }

        public virtual void RemoveEdge(IVertex u, IVertex v)
        {
            if (u == null)
            {
                throw new ArgumentNullException("source vertex");
            }
            if (v == null)
            {
                throw new ArgumentNullException("targetvertex");
            }
            EdgeCollection edges = this.VertexOutEdges.get_Item(u);
            if (edges == null)
            {
                throw new EdgeNotFoundException();
            }
            EdgeCollection edges2 = new EdgeCollection();
            EdgeCollection.Enumerator enumerator = edges.GetEnumerator();
            while (enumerator.MoveNext())
            {
                IEdge edge = enumerator.get_Current();
                if (edge.get_Target() == v)
                {
                    edges2.Add(edge);
                }
            }
            EdgeCollection.Enumerator enumerator2 = edges2.GetEnumerator();
            while (enumerator2.MoveNext())
            {
                IEdge edge2 = enumerator2.get_Current();
                edges.Remove(edge2);
            }
        }

        public virtual void RemoveEdgeIf(IEdgePredicate pred)
        {
            if (pred == null)
            {
                throw new ArgumentNullException("predicate");
            }
            EdgeCollection edges = new EdgeCollection();
            VertexEdgesEnumerator enumerator = this.Edges.GetEnumerator();
            while (enumerator.MoveNext())
            {
                IEdge edge = enumerator.get_Current();
                if (pred.Test(edge))
                {
                    edges.Add(edge);
                }
            }
            EdgeCollection.Enumerator enumerator2 = edges.GetEnumerator();
            while (enumerator2.MoveNext())
            {
                IEdge e = enumerator2.get_Current();
                this.RemoveEdge(e);
            }
        }

        public virtual void RemoveOutEdgeIf(IVertex u, IEdgePredicate pred)
        {
            if (u == null)
            {
                throw new ArgumentNullException("vertex u");
            }
            if (pred == null)
            {
                throw new ArgumentNullException("predicate");
            }
            EdgeCollection edges = this.VertexOutEdges.get_Item(u);
            EdgeCollection edges2 = new EdgeCollection();
            EdgeCollection.Enumerator enumerator = edges.GetEnumerator();
            while (enumerator.MoveNext())
            {
                IEdge edge = enumerator.get_Current();
                if (pred.Test(edge))
                {
                    edges2.Add(edge);
                }
            }
            EdgeCollection.Enumerator enumerator2 = edges2.GetEnumerator();
            while (enumerator2.MoveNext())
            {
                IEdge e = enumerator2.get_Current();
                this.RemoveEdge(e);
            }
        }

        public virtual void RemoveVertex(IVertex v)
        {
            if (v == null)
            {
                throw new ArgumentNullException("vertex");
            }
            if (!this.ContainsVertex(v))
            {
                throw new VertexNotFoundException("v");
            }
            this.ClearVertex(v);
            this.VertexOutEdges.Remove(v);
        }

        public FilteredEdgeEnumerable SelectEdges(IEdgePredicate ep)
        {
            if (ep == null)
            {
                throw new ArgumentNullException("edge predicate");
            }
            return new FilteredEdgeEnumerable(this.Edges, ep);
        }

        public FilteredEdgeEnumerable SelectOutEdges(IVertex v, IEdgePredicate ep)
        {
            if (v == null)
            {
                throw new ArgumentNullException("vertex");
            }
            if (ep == null)
            {
                throw new ArgumentNullException("edge predicate");
            }
            return new FilteredEdgeEnumerable(this.OutEdges(v), ep);
        }

        public IEdge SelectSingleEdge(IEdgePredicate ep)
        {
            if (ep == null)
            {
                throw new ArgumentNullException("edge predicate");
            }
            FilteredEdgeEnumerable.Enumerator enumerator = this.SelectEdges(ep).GetEnumerator();
            while (enumerator.MoveNext())
            {
                return enumerator.get_Current();
            }
            return null;
        }

        public IEdge SelectSingleOutEdge(IVertex v, IEdgePredicate ep)
        {
            if (ep == null)
            {
                throw new ArgumentNullException("edge predicate");
            }
            FilteredEdgeEnumerable.Enumerator enumerator = this.SelectOutEdges(v, ep).GetEnumerator();
            while (enumerator.MoveNext())
            {
                return enumerator.get_Current();
            }
            return null;
        }

        public IVertex SelectSingleVertex(IVertexPredicate vp)
        {
            if (vp == null)
            {
                throw new ArgumentNullException("vertex predicate");
            }
            IVertexEnumerator enumerator = this.SelectVertices(vp).GetEnumerator();
            while (enumerator.MoveNext())
            {
                return enumerator.get_Current();
            }
            return null;
        }

        public IVertexEnumerable SelectVertices(IVertexPredicate vp)
        {
            if (vp == null)
            {
                throw new ArgumentNullException("vertex predicate");
            }
            return new FilteredVertexEnumerable(this.Vertices, vp);
        }

        public bool AllowParallelEdges
        {
            get
            {
                return (this.IsDirected && this.allowParallelEdges);
            }
        }

        public IEdgeProvider EdgeProvider
        {
            get
            {
                return this.edgeProvider;
            }
        }

        public VertexEdgesEnumerable Edges
        {
            get
            {
                return new VertexEdgesEnumerable(this.VertexOutEdges);
            }
        }

        public int EdgesCount
        {
            get
            {
                int num = 0;
                IDictionaryEnumerator enumerator = this.VertexOutEdges.GetEnumerator();
                while (enumerator.MoveNext())
                {
                    DictionaryEntry current = (DictionaryEntry) enumerator.Current;
                    num += ((EdgeCollection) current.Value).Count;
                }
                return num;
            }
        }

        public bool EdgesEmpty
        {
            get
            {
                return (this.EdgesCount == 0);
            }
        }

        public bool IsDirected
        {
            get
            {
                return true;
            }
        }

        protected VertexEdgesDictionary VertexOutEdges
        {
            get
            {
                return this.vertexOutEdges;
            }
        }

        public IVertexProvider VertexProvider
        {
            get
            {
                return this.vertexProvider;
            }
        }

        public VertexEnumerable Vertices
        {
            get
            {
                return new VertexEnumerable(this.VertexOutEdges.get_Keys());
            }
        }

        public int VerticesCount
        {
            get
            {
                return this.VertexOutEdges.Count;
            }
        }

        public bool VerticesEmpty
        {
            get
            {
                return (this.VertexOutEdges.Count == 0);
            }
        }
    }
}

