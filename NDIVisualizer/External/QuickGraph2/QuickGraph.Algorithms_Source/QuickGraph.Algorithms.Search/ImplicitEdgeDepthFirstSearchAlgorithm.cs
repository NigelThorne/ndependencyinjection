namespace QuickGraph.Algorithms.Search
{
    using QuickGraph.Collections;
    using QuickGraph.Concepts;
    using QuickGraph.Concepts.Algorithms;
    using QuickGraph.Concepts.Collections;
    using QuickGraph.Concepts.Traversals;
    using QuickGraph.Concepts.Visitors;
    using System;
    using System.Runtime.CompilerServices;

    public class ImplicitEdgeDepthFirstSearchAlgorithm : ITreeEdgeBuilderAlgorithm, IAlgorithm
    {
        private EdgeColorDictionary edgeColors = new EdgeColorDictionary();
        private int maxDepth = 0x7fffffff;
        private IIncidenceGraph visitedGraph;

        public event EdgeEventHandler BackEdge;

        public event EdgeEdgeEventHandler DiscoverTreeEdge;

        public event EdgeEventHandler FinishEdge;

        public event EdgeEventHandler ForwardOrCrossEdge;

        public event EdgeEventHandler StartEdge;

        public event VertexEventHandler StartVertex;

        public event EdgeEventHandler TreeEdge;

        public ImplicitEdgeDepthFirstSearchAlgorithm(IIncidenceGraph visitedGraph)
        {
            if (visitedGraph == null)
            {
                throw new ArgumentNullException("visitedGraph");
            }
            this.visitedGraph = visitedGraph;
        }

        public void Compute(IVertex v)
        {
            if (v == null)
            {
                throw new ArgumentNullException("v");
            }
            this.Initialize();
            this.OnStartVertex(v);
            IEdgeEnumerator enumerator = this.VisitedGraph.OutEdges(v).GetEnumerator();
            while (enumerator.MoveNext())
            {
                IEdge e = enumerator.get_Current();
                if (!this.EdgeColors.Contains(e))
                {
                    this.OnStartEdge(e);
                    this.Visit(e, 0);
                }
            }
        }

        public virtual void Initialize()
        {
            this.EdgeColors.Clear();
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
                    if (!this.EdgeColors.Contains(e))
                    {
                        this.OnDiscoverTreeEdge(se, e);
                        this.Visit(e, depth + 1);
                    }
                    else
                    {
                        GraphColor color = this.EdgeColors.get_Item(e);
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

        public IIncidenceGraph VisitedGraph
        {
            get
            {
                return this.visitedGraph;
            }
        }
    }
}

