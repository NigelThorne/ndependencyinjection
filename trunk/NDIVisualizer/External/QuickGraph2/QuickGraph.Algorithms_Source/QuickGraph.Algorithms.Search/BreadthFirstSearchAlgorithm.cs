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

    public class BreadthFirstSearchAlgorithm : IPredecessorRecorderAlgorithm, IDistanceRecorderAlgorithm, IVertexColorizerAlgorithm, ITreeEdgeBuilderAlgorithm, IAlgorithm
    {
        private VertexColorDictionary m_Colors;
        private VertexBuffer m_Q;
        private IVertexListGraph m_VisitedGraph;

        public event EdgeEventHandler BlackTarget;

        public event VertexEventHandler DiscoverVertex;

        public event EdgeEventHandler ExamineEdge;

        public event VertexEventHandler ExamineVertex;

        public event VertexEventHandler FinishVertex;

        public event EdgeEventHandler GrayTarget;

        public event VertexEventHandler InitializeVertex;

        public event EdgeEventHandler NonTreeEdge;

        public event EdgeEventHandler TreeEdge;

        public BreadthFirstSearchAlgorithm(IVertexListGraph g)
        {
            if (g == null)
            {
                throw new ArgumentNullException("g");
            }
            this.m_VisitedGraph = g;
            this.m_Colors = new VertexColorDictionary();
            this.m_Q = new VertexBuffer();
        }

        public BreadthFirstSearchAlgorithm(IVertexListGraph g, VertexBuffer q, VertexColorDictionary colors)
        {
            if (g == null)
            {
                throw new ArgumentNullException("g");
            }
            if (q == null)
            {
                throw new ArgumentNullException("Stack Q is null");
            }
            if (colors == null)
            {
                throw new ArgumentNullException("Colors");
            }
            this.m_VisitedGraph = g;
            this.m_Colors = colors;
            this.m_Q = q;
        }

        public void Compute(IVertex s)
        {
            if (s == null)
            {
                throw new ArgumentNullException("Start vertex is null");
            }
            IVertexEnumerator enumerator = this.VisitedGraph.get_Vertices().GetEnumerator();
            while (enumerator.MoveNext())
            {
                IVertex v = enumerator.get_Current();
                this.Colors.set_Item(v, 0);
                this.OnInitializeVertex(v);
            }
            this.Visit(s);
        }

        protected void OnBlackTarget(IEdge e)
        {
            if (this.BlackTarget != null)
            {
                this.BlackTarget.Invoke(this, new EdgeEventArgs(e));
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

        protected void OnExamineVertex(IVertex v)
        {
            if (this.ExamineVertex != null)
            {
                this.ExamineVertex.Invoke(this, new VertexEventArgs(v));
            }
        }

        protected void OnFinishVertex(IVertex v)
        {
            if (this.FinishVertex != null)
            {
                this.FinishVertex.Invoke(this, new VertexEventArgs(v));
            }
        }

        protected void OnGrayTarget(IEdge e)
        {
            if (this.GrayTarget != null)
            {
                this.GrayTarget.Invoke(this, new EdgeEventArgs(e));
            }
        }

        protected void OnInitializeVertex(IVertex v)
        {
            if (this.InitializeVertex != null)
            {
                this.InitializeVertex.Invoke(this, new VertexEventArgs(v));
            }
        }

        protected void OnNonTreeEdge(IEdge e)
        {
            if (this.NonTreeEdge != null)
            {
                this.NonTreeEdge.Invoke(this, new EdgeEventArgs(e));
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

        public void RegisterDistanceRecorderHandlers(IDistanceRecorderVisitor vis)
        {
            if (vis == null)
            {
                throw new ArgumentNullException("visitor");
            }
            this.InitializeVertex = (VertexEventHandler) Delegate.Combine(this.InitializeVertex, new VertexEventHandler(vis, (IntPtr) vis.InitializeVertex));
            this.DiscoverVertex = (VertexEventHandler) Delegate.Combine(this.DiscoverVertex, new VertexEventHandler(vis, (IntPtr) vis.DiscoverVertex));
            this.TreeEdge = (EdgeEventHandler) Delegate.Combine(this.TreeEdge, new EdgeEventHandler(vis, (IntPtr) vis.TreeEdge));
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

        public void Visit(IVertex s)
        {
            if (s == null)
            {
                throw new ArgumentNullException("Start vertex is null");
            }
            this.Colors.set_Item(s, 2);
            this.OnDiscoverVertex(s);
            this.m_Q.Push(s);
            while (this.m_Q.get_Count() != 0)
            {
                IVertex v = this.m_Q.Peek();
                this.m_Q.Pop();
                this.OnExamineVertex(v);
                IEdgeEnumerator enumerator = this.VisitedGraph.OutEdges(v).GetEnumerator();
                while (enumerator.MoveNext())
                {
                    IEdge e = enumerator.get_Current();
                    IVertex vertex2 = e.get_Target();
                    this.OnExamineEdge(e);
                    GraphColor color = this.Colors.get_Item(vertex2);
                    if (color == null)
                    {
                        this.OnTreeEdge(e);
                        this.Colors.set_Item(vertex2, 2);
                        this.OnDiscoverVertex(vertex2);
                        this.m_Q.Push(vertex2);
                    }
                    else
                    {
                        this.OnNonTreeEdge(e);
                        if (color == 2)
                        {
                            this.OnGrayTarget(e);
                            continue;
                        }
                        this.OnBlackTarget(e);
                    }
                }
                this.Colors.set_Item(v, 1);
                this.OnFinishVertex(v);
            }
        }

        public VertexColorDictionary Colors
        {
            get
            {
                return this.m_Colors;
            }
        }

        public IVertexListGraph VisitedGraph
        {
            get
            {
                return this.m_VisitedGraph;
            }
        }
    }
}

