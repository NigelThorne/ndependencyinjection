namespace QuickGraph.Algorithms.Search
{
    using QuickGraph.Collections;
    using QuickGraph.Concepts;
    using QuickGraph.Concepts.Algorithms;
    using QuickGraph.Concepts.Collections;
    using QuickGraph.Concepts.Traversals;
    using QuickGraph.Concepts.Visitors;
    using System;
    using System.Collections;
    using System.Runtime.CompilerServices;

    public class UndirectedDepthFirstSearchAlgorithm : IVertexColorizerAlgorithm, IPredecessorRecorderAlgorithm, ITimeStamperAlgorithm, IAlgorithm
    {
        private VertexColorDictionary colors;
        private EdgeColorDictionary edgeColors;
        private IBidirectionalVertexAndEdgeListGraph visitedGraph;

        public event EdgeEventHandler BackEdge;

        public event VertexEventHandler DiscoverVertex;

        public event EdgeEventHandler ExamineEdge;

        public event VertexEventHandler FinishVertex;

        public event VertexEventHandler InitializeVertex;

        public event VertexEventHandler StartVertex;

        public event EdgeEventHandler TreeEdge;

        public UndirectedDepthFirstSearchAlgorithm(IBidirectionalVertexAndEdgeListGraph g)
        {
            if (g == null)
            {
                throw new ArgumentNullException("g");
            }
            this.visitedGraph = g;
            this.edgeColors = new EdgeColorDictionary();
            this.colors = new VertexColorDictionary();
        }

        public void Compute()
        {
            this.Compute(null);
        }

        public void Compute(IVertex s)
        {
            IVertexEnumerator enumerator = this.VisitedGraph.get_Vertices().GetEnumerator();
            while (enumerator.MoveNext())
            {
                IVertex v = enumerator.get_Current();
                this.Colors.set_Item(v, 0);
                this.OnInitializeVertex(v);
            }
            IEdgeEnumerator enumerator2 = this.VisitedGraph.get_Edges().GetEnumerator();
            while (enumerator2.MoveNext())
            {
                IEdge edge = enumerator2.get_Current();
                this.EdgeColors.set_Item(edge, 0);
            }
            if (s != null)
            {
                this.OnStartVertex(s);
                this.Visit(s);
            }
            IVertexEnumerator enumerator3 = this.VisitedGraph.get_Vertices().GetEnumerator();
            while (enumerator3.MoveNext())
            {
                IVertex vertex2 = enumerator3.get_Current();
                if (this.Colors.get_Item(vertex2) == null)
                {
                    this.OnStartVertex(vertex2);
                    this.Visit(vertex2);
                }
            }
        }

        protected void OnBackEdge(IEdge e)
        {
            if (this.BackEdge != null)
            {
                this.BackEdge.Invoke(this, new EdgeEventArgs(e));
            }
        }

        protected void OnDiscoverVertex(IVertex v)
        {
            if (this.DiscoverVertex != null)
            {
                this.DiscoverVertex.Invoke(this, new VertexEventArgs(v));
            }
        }

        protected void OnExamineEdge(IEdge e)
        {
            if (this.ExamineEdge != null)
            {
                this.ExamineEdge.Invoke(this, new EdgeEventArgs(e));
            }
        }

        protected void OnFinishVertex(IVertex v)
        {
            if (this.FinishVertex != null)
            {
                this.FinishVertex.Invoke(this, new VertexEventArgs(v));
            }
        }

        protected void OnInitializeVertex(IVertex v)
        {
            if (this.InitializeVertex != null)
            {
                this.InitializeVertex.Invoke(this, new VertexEventArgs(v));
            }
        }

        protected void OnStartVertex(IVertex v)
        {
            if (this.StartVertex != null)
            {
                this.StartVertex.Invoke(this, new VertexEventArgs(v));
            }
        }

        protected void OnTreeEdge(IEdge e)
        {
            if (this.TreeEdge != null)
            {
                this.TreeEdge.Invoke(this, new EdgeEventArgs(e));
            }
        }

        object IAlgorithm.get_VisitedGraph()
        {
            return this.VisitedGraph;
        }

        IDictionary IVertexColorizerAlgorithm.get_Colors()
        {
            return this.Colors;
        }

        public void RegisterPredecessorRecorderHandlers(IPredecessorRecorderVisitor vis)
        {
            if (vis == null)
            {
                throw new ArgumentNullException("visitor");
            }
            this.TreeEdge = (EdgeEventHandler) Delegate.Combine(this.TreeEdge, new EdgeEventHandler(vis, (IntPtr) vis.TreeEdge));
            this.FinishVertex = (VertexEventHandler) Delegate.Combine(this.FinishVertex, new VertexEventHandler(vis, (IntPtr) vis.FinishVertex));
        }

        public void RegisterTimeStamperHandlers(ITimeStamperVisitor vis)
        {
            if (vis == null)
            {
                throw new ArgumentNullException("visitor");
            }
            this.DiscoverVertex = (VertexEventHandler) Delegate.Combine(this.DiscoverVertex, new VertexEventHandler(vis, (IntPtr) vis.DiscoverVertex));
            this.FinishVertex = (VertexEventHandler) Delegate.Combine(this.FinishVertex, new VertexEventHandler(vis, (IntPtr) vis.FinishVertex));
        }

        public void RegisterVertexColorizerHandlers(IVertexColorizerVisitor vis)
        {
            if (vis == null)
            {
                throw new ArgumentNullException("visitor");
            }
            this.InitializeVertex = (VertexEventHandler) Delegate.Combine(this.InitializeVertex, new VertexEventHandler(vis, (IntPtr) vis.InitializeVertex));
            this.DiscoverVertex = (VertexEventHandler) Delegate.Combine(this.DiscoverVertex, new VertexEventHandler(vis, (IntPtr) vis.DiscoverVertex));
            this.FinishVertex = (VertexEventHandler) Delegate.Combine(this.FinishVertex, new VertexEventHandler(vis, (IntPtr) vis.FinishVertex));
        }

        public void Visit(IVertex u)
        {
            VertexEventArgs args = new VertexEventArgs(u);
            this.Colors.set_Item(u, 2);
            this.OnDiscoverVertex(u);
            IEdgeEnumerable outEdges = this.VisitedGraph.OutEdges(u);
            this.VisitEdges(outEdges, true);
            IEdgeEnumerable enumerable2 = this.VisitedGraph.InEdges(u);
            this.VisitEdges(enumerable2, false);
            this.Colors.set_Item(u, 1);
            this.OnFinishVertex(u);
        }

        private void VisitEdges(IEdgeEnumerable outEdges, bool forward)
        {
            IVertex u = null;
            IEdgeEnumerator enumerator = outEdges.GetEnumerator();
            while (enumerator.MoveNext())
            {
                IEdge e = enumerator.get_Current();
                this.OnExamineEdge(e);
                if (forward)
                {
                    u = e.get_Target();
                }
                else
                {
                    u = e.get_Source();
                }
                GraphColor color = this.Colors.get_Item(u);
                GraphColor color2 = this.EdgeColors.get_Item(e);
                this.EdgeColors.set_Item(e, 1);
                if (color == null)
                {
                    this.OnTreeEdge(e);
                    this.Visit(u);
                }
                else if ((color == 2) && (color2 == null))
                {
                    this.OnBackEdge(e);
                }
            }
        }

        public VertexColorDictionary Colors
        {
            get
            {
                return this.colors;
            }
        }

        public EdgeColorDictionary EdgeColors
        {
            get
            {
                return this.edgeColors;
            }
        }

        public IBidirectionalVertexAndEdgeListGraph VisitedGraph
        {
            get
            {
                return this.visitedGraph;
            }
        }
    }
}

