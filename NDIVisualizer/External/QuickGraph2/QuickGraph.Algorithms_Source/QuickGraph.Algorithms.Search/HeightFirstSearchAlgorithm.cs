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

    public class HeightFirstSearchAlgorithm : IPredecessorRecorderAlgorithm, ITimeStamperAlgorithm, IVertexColorizerAlgorithm, ITreeEdgeBuilderAlgorithm, IAlgorithm
    {
        private VertexColorDictionary colors;
        private int maxDepth;
        private IBidirectionalVertexListGraph visitedGraph;

        public event EdgeEventHandler BackEdge;

        public event VertexEventHandler DiscoverVertex;

        public event EdgeEventHandler ExamineEdge;

        public event VertexEventHandler FinishVertex;

        public event EdgeEventHandler ForwardOrCrossEdge;

        public event VertexEventHandler InitializeVertex;

        public event VertexEventHandler StartVertex;

        public event EdgeEventHandler TreeEdge;

        public HeightFirstSearchAlgorithm(IBidirectionalVertexListGraph g)
        {
            this.maxDepth = 0x7fffffff;
            if (g == null)
            {
                throw new ArgumentNullException("g");
            }
            this.visitedGraph = g;
            this.colors = new VertexColorDictionary();
        }

        public HeightFirstSearchAlgorithm(IBidirectionalVertexListGraph g, VertexColorDictionary colors)
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
            this.colors = colors;
        }

        public void Compute()
        {
            this.Compute(null);
        }

        public void Compute(IVertex s)
        {
            this.Initialize();
            if (s != null)
            {
                this.OnStartVertex(s);
                this.Visit(s, 0);
            }
            IVertexEnumerator enumerator = this.VisitedGraph.get_Vertices().GetEnumerator();
            while (enumerator.MoveNext())
            {
                IVertex v = enumerator.get_Current();
                if (this.Colors.get_Item(v) == null)
                {
                    this.OnStartVertex(v);
                    this.Visit(v, 0);
                }
            }
        }

        public void Initialize()
        {
            IVertexEnumerator enumerator = this.VisitedGraph.get_Vertices().GetEnumerator();
            while (enumerator.MoveNext())
            {
                IVertex v = enumerator.get_Current();
                this.Colors.set_Item(v, 0);
                this.OnInitializeVertex(v);
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

        protected void OnForwardOrCrossEdge(IEdge e)
        {
            if (this.ForwardOrCrossEdge != null)
            {
                this.ForwardOrCrossEdge.Invoke(this, new EdgeEventArgs(e));
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

        public void RegisterTreeEdgeBuilderHandlers(ITreeEdgeBuilderVisitor vis)
        {
            if (vis == null)
            {
                throw new ArgumentNullException("visitor");
            }
            this.TreeEdge = (EdgeEventHandler) Delegate.Combine(this.TreeEdge, new EdgeEventHandler(vis, (IntPtr) vis.TreeEdge));
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

        public void Visit(IVertex u, int depth)
        {
            if (depth <= this.maxDepth)
            {
                if (u == null)
                {
                    throw new ArgumentNullException("u");
                }
                this.Colors.set_Item(u, 2);
                this.OnDiscoverVertex(u);
                IVertex vertex = null;
                IEdgeEnumerator enumerator = this.VisitedGraph.InEdges(u).GetEnumerator();
                while (enumerator.MoveNext())
                {
                    IEdge e = enumerator.get_Current();
                    this.OnExamineEdge(e);
                    vertex = e.get_Source();
                    GraphColor color = this.Colors.get_Item(vertex);
                    if (color == null)
                    {
                        this.OnTreeEdge(e);
                        this.Visit(vertex, depth + 1);
                    }
                    else
                    {
                        if (color == 2)
                        {
                            this.OnBackEdge(e);
                            continue;
                        }
                        this.OnForwardOrCrossEdge(e);
                    }
                }
                this.Colors.set_Item(u, 1);
                this.OnFinishVertex(u);
            }
        }

        public VertexColorDictionary Colors
        {
            get
            {
                return this.colors;
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

        public IBidirectionalVertexListGraph VisitedGraph
        {
            get
            {
                return this.visitedGraph;
            }
        }
    }
}

