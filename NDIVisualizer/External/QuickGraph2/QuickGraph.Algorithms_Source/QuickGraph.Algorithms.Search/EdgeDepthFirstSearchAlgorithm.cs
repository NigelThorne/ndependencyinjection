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

    public class EdgeDepthFirstSearchAlgorithm : ITreeEdgeBuilderAlgorithm, IEdgeColorizerAlgorithm, IEdgePredecessorRecorderAlgorithm, IAlgorithm
    {
        private EdgeColorDictionary edgeColors;
        private int maxDepth;
        private IEdgeListAndIncidenceGraph visitedGraph;

        public event EdgeEventHandler BackEdge;

        public event EdgeEdgeEventHandler DiscoverTreeEdge;

        public event EdgeEventHandler FinishEdge;

        public event EdgeEventHandler ForwardOrCrossEdge;

        public event EdgeEventHandler InitializeEdge;

        public event EdgeEventHandler StartEdge;

        public event VertexEventHandler StartVertex;

        public event EdgeEventHandler TreeEdge;

        public EdgeDepthFirstSearchAlgorithm(IEdgeListAndIncidenceGraph g) : this(g, new EdgeColorDictionary())
        {
        }

        public EdgeDepthFirstSearchAlgorithm(IEdgeListAndIncidenceGraph g, EdgeColorDictionary colors)
        {
            this.maxDepth = 0x7fffffff;
            if (g == null)
            {
                throw new ArgumentNullException("g");
            }
            if (colors == null)
            {
                throw new ArgumentNullException("Colors");
            }
            this.visitedGraph = g;
            this.edgeColors = colors;
        }

        public void Compute()
        {
            this.Compute(Traversal.FirstEdge(this.VisitedGraph).get_Source());
        }

        public void Compute(IVertex v)
        {
            if (v == null)
            {
                throw new ArgumentNullException("entry point");
            }
            this.Initialize();
            this.OnStartVertex(v);
            IEdgeEnumerator enumerator = this.VisitedGraph.OutEdges(v).GetEnumerator();
            while (enumerator.MoveNext())
            {
                IEdge e = enumerator.get_Current();
                if (this.EdgeColors.get_Item(e) == null)
                {
                    this.OnStartEdge(e);
                    this.Visit(e, 0);
                }
            }
            IEdgeEnumerator enumerator2 = this.VisitedGraph.get_Edges().GetEnumerator();
            while (enumerator2.MoveNext())
            {
                IEdge edge2 = enumerator2.get_Current();
                if (this.EdgeColors.get_Item(edge2) == null)
                {
                    this.OnStartEdge(edge2);
                    this.Visit(edge2, 0);
                }
            }
        }

        public void Initialize()
        {
            IEdgeEnumerator enumerator = this.VisitedGraph.get_Edges().GetEnumerator();
            while (enumerator.MoveNext())
            {
                IEdge e = enumerator.get_Current();
                this.EdgeColors.set_Item(e, 0);
                this.OnInitializeEdge(e);
            }
        }

        protected void OnBackEdge(IEdge e)
        {
            if (this.BackEdge != null)
            {
                this.BackEdge.Invoke(this, new EdgeEventArgs(e));
            }
        }

        public void OnDiscoverTreeEdge(IEdge se, IEdge e)
        {
            if (this.DiscoverTreeEdge != null)
            {
                this.DiscoverTreeEdge.Invoke(this, new EdgeEdgeEventArgs(se, e));
            }
        }

        protected void OnFinishEdge(IEdge e)
        {
            if (this.FinishEdge != null)
            {
                this.FinishEdge.Invoke(this, new EdgeEventArgs(e));
            }
        }

        protected void OnForwardOrCrossEdge(IEdge e)
        {
            if (this.ForwardOrCrossEdge != null)
            {
                this.ForwardOrCrossEdge.Invoke(this, new EdgeEventArgs(e));
            }
        }

        protected void OnInitializeEdge(IEdge e)
        {
            if (this.InitializeEdge != null)
            {
                this.InitializeEdge.Invoke(this, new EdgeEventArgs(e));
            }
        }

        protected void OnStartEdge(IEdge e)
        {
            if (this.StartEdge != null)
            {
                this.StartEdge.Invoke(this, new EdgeEventArgs(e));
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

        IDictionary IEdgeColorizerAlgorithm.get_EdgeColors()
        {
            return this.EdgeColors;
        }

        public void RegisterEdgeColorizerHandlers(IEdgeColorizerVisitor vis)
        {
            if (vis == null)
            {
                throw new ArgumentNullException("vis");
            }
            this.InitializeEdge = (EdgeEventHandler) Delegate.Combine(this.InitializeEdge, new EdgeEventHandler(vis, (IntPtr) vis.InitializeEdge));
            this.TreeEdge = (EdgeEventHandler) Delegate.Combine(this.TreeEdge, new EdgeEventHandler(vis, (IntPtr) vis.TreeEdge));
            this.FinishEdge = (EdgeEventHandler) Delegate.Combine(this.FinishEdge, new EdgeEventHandler(vis, (IntPtr) vis.FinishEdge));
        }

        public void RegisterEdgePredecessorRecorderHandlers(IEdgePredecessorRecorderVisitor vis)
        {
            this.InitializeEdge = (EdgeEventHandler) Delegate.Combine(this.InitializeEdge, new EdgeEventHandler(vis, (IntPtr) vis.InitializeEdge));
            this.DiscoverTreeEdge = (EdgeEdgeEventHandler) Delegate.Combine(this.DiscoverTreeEdge, new EdgeEdgeEventHandler(vis, (IntPtr) vis.DiscoverTreeEdge));
            this.FinishEdge = (EdgeEventHandler) Delegate.Combine(this.FinishEdge, new EdgeEventHandler(vis, (IntPtr) vis.FinishEdge));
        }

        public void RegisterTreeEdgeBuilderHandlers(ITreeEdgeBuilderVisitor vis)
        {
            if (vis == null)
            {
                throw new ArgumentNullException("visitor");
            }
            this.TreeEdge = (EdgeEventHandler) Delegate.Combine(this.TreeEdge, new EdgeEventHandler(vis, (IntPtr) vis.TreeEdge));
        }

        public void Visit(IEdge se, int depth)
        {
            if (depth <= this.maxDepth)
            {
                if (se == null)
                {
                    throw new ArgumentNullException("se");
                }
                this.EdgeColors.set_Item(se, 2);
                this.OnTreeEdge(se);
                IEdgeEnumerator enumerator = this.VisitedGraph.OutEdges(se.get_Target()).GetEnumerator();
                while (enumerator.MoveNext())
                {
                    IEdge e = enumerator.get_Current();
                    if (this.EdgeColors.get_Item(e) == null)
                    {
                        this.OnDiscoverTreeEdge(se, e);
                        this.Visit(e, depth + 1);
                    }
                    else
                    {
                        if (this.EdgeColors.get_Item(e) == 2)
                        {
                            this.OnBackEdge(e);
                            continue;
                        }
                        this.OnForwardOrCrossEdge(e);
                    }
                }
                this.EdgeColors.set_Item(se, 1);
                this.OnFinishEdge(se);
            }
        }

        public EdgeColorDictionary EdgeColors
        {
            get
            {
                return this.edgeColors;
            }
        }

        public int MaxDepth
        {
            get
            {
                return this.maxDepth;
            }
            set
            {
                this.maxDepth = value;
            }
        }

        public IEdgeListAndIncidenceGraph VisitedGraph
        {
            get
            {
                return this.visitedGraph;
            }
        }
    }
}

