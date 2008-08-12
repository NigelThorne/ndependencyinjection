namespace QuickGraph.Representations
{
    using QuickGraph.Collections;
    using QuickGraph.Concepts;
    using QuickGraph.Concepts.Collections;
    using QuickGraph.Concepts.Modifications;
    using QuickGraph.Concepts.MutableTraversals;
    using QuickGraph.Concepts.Predicates;
    using QuickGraph.Concepts.Providers;
    using QuickGraph.Concepts.Traversals;
    using QuickGraph.Exceptions;
    using System;

    public class BidirectionalGraph : AdjacencyGraph, IFilteredBidirectionalGraph, IFilteredIncidenceGraph, IBidirectionalVertexAndEdgeListGraph, IBidirectionalVertexListGraph, IMutableBidirectionalVertexAndEdgeListGraph, IMutableBidirectionalGraph, IMutableIncidenceGraph, IBidirectionalGraph, IMutableVertexAndEdgeListGraph, IVertexAndEdgeListGraph, IVertexListGraph, IEdgeListAndIncidenceGraph, IEdgeListGraph, IIncidenceGraph, IAdjacencyGraph, IImplicitGraph, IVertexAndEdgeMutableGraph, IEdgeMutableGraph, IVertexMutableGraph, IMutableGraph, IGraph
    {
        private VertexEdgesDictionary m_VertexInEdges;

        public BidirectionalGraph(bool allowParallelEdges) : base(allowParallelEdges)
        {
            this.m_VertexInEdges = new VertexEdgesDictionary();
        }

        public BidirectionalGraph(IVertexProvider vertexProvider, IEdgeProvider edgeProvider, bool allowParallelEdges) : base(vertexProvider, edgeProvider, allowParallelEdges)
        {
            this.m_VertexInEdges = new VertexEdgesDictionary();
        }

        public override void AddEdge(IEdge e)
        {
            if (e == null)
            {
                throw new ArgumentNullException("edge");
            }
            if (!this.VertexInEdges.ContainsKey(e.get_Source()))
            {
                throw new VertexNotFoundException("Could not find source vertex");
            }
            if (!this.VertexInEdges.ContainsKey(e.get_Target()))
            {
                throw new VertexNotFoundException("Could not find target vertex");
            }
            base.AddEdge(e);
            this.VertexInEdges.get_Item(e.get_Target()).Add(e);
        }

        public override IEdge AddEdge(IVertex source, IVertex target)
        {
            if (!this.VertexInEdges.ContainsKey(source))
            {
                throw new VertexNotFoundException("Could not find source vertex");
            }
            if (!this.VertexInEdges.ContainsKey(target))
            {
                throw new VertexNotFoundException("Could not find target vertex");
            }
            IEdge edge = base.AddEdge(source, target);
            this.VertexInEdges.get_Item(target).Add(edge);
            return edge;
        }

        public override IVertex AddVertex()
        {
            IVertex vertex = base.AddVertex();
            this.VertexInEdges.Add(vertex, new EdgeCollection());
            return vertex;
        }

        public override void AddVertex(IVertex v)
        {
            base.AddVertex(v);
            this.VertexInEdges.Add(v, new EdgeCollection());
        }

        public bool AdjacentEdgesEmpty(IVertex v)
        {
            return (base.OutEdgesEmpty(v) && this.InEdgesEmpty(v));
        }

        public override void Clear()
        {
            base.Clear();
            this.VertexInEdges.Clear();
        }

        public override void ClearVertex(IVertex v)
        {
            if (v == null)
            {
                throw new ArgumentNullException("vertex");
            }
            base.ClearVertex(v);
            this.VertexInEdges.get_Item(v).Clear();
        }

        public int Degree(IVertex v)
        {
            if (v == null)
            {
                throw new ArgumentNullException("v");
            }
            return (this.VertexInEdges.get_Item(v).Count + base.VertexOutEdges.get_Item(v).Count);
        }

        public int InDegree(IVertex v)
        {
            if (v == null)
            {
                throw new ArgumentNullException("v");
            }
            return this.VertexInEdges.get_Item(v).Count;
        }

        public EdgeCollection InEdges(IVertex v)
        {
            if (v == null)
            {
                throw new ArgumentNullException("v");
            }
            return this.VertexInEdges.get_Item(v);
        }

        public bool InEdgesEmpty(IVertex v)
        {
            if (v == null)
            {
                throw new ArgumentNullException("v");
            }
            return (this.VertexInEdges.get_Item(v).Count == 0);
        }

        IEdgeEnumerable IBidirectionalGraph.InEdges(IVertex v)
        {
            return this.InEdges(v);
        }

        IEdgeEnumerable IFilteredBidirectionalGraph.SelectInEdges(IVertex v, IEdgePredicate ep)
        {
            return this.SelectInEdges(v, ep);
        }

        public override void RemoveEdge(IEdge e)
        {
            if (e == null)
            {
                throw new ArgumentNullException("edge");
            }
            base.RemoveEdge(e);
            EdgeCollection edges = this.VertexInEdges.get_Item(e.get_Target());
            if ((edges == null) || !edges.Contains(e))
            {
                throw new EdgeNotFoundException();
            }
            edges.Remove(e);
        }

        public override void RemoveEdge(IVertex u, IVertex v)
        {
            if (u == null)
            {
                throw new ArgumentNullException("source vertex");
            }
            if (v == null)
            {
                throw new ArgumentNullException("targetvertex");
            }
            EdgeCollection edges = base.VertexOutEdges.get_Item(u);
            EdgeCollection edges2 = this.VertexInEdges.get_Item(v);
            EdgeCollection edges3 = new EdgeCollection();
            EdgeCollection.Enumerator enumerator = edges.GetEnumerator();
            while (enumerator.MoveNext())
            {
                IEdge edge = enumerator.get_Current();
                if (edge.get_Target() == v)
                {
                    edges3.Add(edge);
                }
            }
            EdgeCollection.Enumerator enumerator2 = edges2.GetEnumerator();
            while (enumerator2.MoveNext())
            {
                IEdge edge2 = enumerator2.get_Current();
                if (edge2.get_Source() == u)
                {
                    edges3.Add(edge2);
                }
            }
            EdgeCollection.Enumerator enumerator3 = edges3.GetEnumerator();
            while (enumerator3.MoveNext())
            {
                IEdge e = enumerator3.get_Current();
                this.RemoveEdge(e);
            }
        }

        public void RemoveInEdgeIf(IVertex u, IEdgePredicate pred)
        {
            if (u == null)
            {
                throw new ArgumentNullException("vertex u");
            }
            if (pred == null)
            {
                throw new ArgumentNullException("predicate");
            }
            EdgeCollection edges = this.VertexInEdges.get_Item(u);
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

        public override void RemoveVertex(IVertex v)
        {
            if (v == null)
            {
                throw new ArgumentNullException("vertex");
            }
            base.RemoveVertex(v);
            this.VertexInEdges.Remove(v);
        }

        public FilteredEdgeEnumerable SelectInEdges(IVertex v, IEdgePredicate ep)
        {
            if (v == null)
            {
                throw new ArgumentNullException("vertex");
            }
            if (ep == null)
            {
                throw new ArgumentNullException("edge predicate");
            }
            return new FilteredEdgeEnumerable(this.InEdges(v), ep);
        }

        public IEdge SelectSingleInEdge(IVertex v, IEdgePredicate ep)
        {
            if (ep == null)
            {
                throw new ArgumentNullException("edge predicate");
            }
            FilteredEdgeEnumerable.Enumerator enumerator = this.SelectInEdges(v, ep).GetEnumerator();
            while (enumerator.MoveNext())
            {
                return enumerator.get_Current();
            }
            return null;
        }

        protected VertexEdgesDictionary VertexInEdges
        {
            get
            {
                return this.m_VertexInEdges;
            }
        }
    }
}

