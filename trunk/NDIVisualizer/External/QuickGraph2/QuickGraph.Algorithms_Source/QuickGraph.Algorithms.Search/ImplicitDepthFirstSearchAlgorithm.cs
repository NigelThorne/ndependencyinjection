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

    public class ImplicitDepthFirstSearchAlgorithm : IPredecessorRecorderAlgorithm, ITimeStamperAlgorithm, ITreeEdgeBuilderAlgorithm, IAlgorithm
    {
        private VertexColorDictionary colors = new VertexColorDictionary();
        private int maxDepth = 0x7fffffff;
        private IIncidenceGraph visitedGraph;

        public event EdgeEventHandler BackEdge;

        public event VertexEventHandler DiscoverVertex;

        public event EdgeEventHandler ExamineEdge;

        public event VertexEventHandler FinishVertex;

        public event EdgeEventHandler ForwardOrCrossEdge;

        public event VertexEventHandler StartVertex;

        public event EdgeEventHandler TreeEdge;

        public ImplicitDepthFirstSearchAlgorithm(IIncidenceGraph visitedGraph)
        {
            if (visitedGraph == null)
            {
                throw new ArgumentNullException("visitedGraph");
            }
            this.visitedGraph = visitedGraph;
        }

        public void Compute(IVertex startVertex)
        {
            if (startVertex == null)
            {
                throw new ArgumentNullException("startVertex");
            }
            this.Initialize();
            this.Visit(startVertex, 0);
        }

        public virtual void Initialize()
        {
            this.Colors.Clear();
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

        public virtual void Visit(IVertex u, int depth)
        {
            if (depth <= this.MaxDepth)
            {
                this.Colors.set_Item(u, 2);
                this.OnDiscoverVertex(u);
                IVertex vertex = null;
                IEdgeEnumerator enumerator = this.VisitedGraph.OutEdges(u).GetEnumerator();
                while (enumerator.MoveNext())
                {
                    IEdge e = enumerator.get_Current();
                    this.OnExamineEdge(e);
                    vertex = e.get_Target();
                    if (!this.Colors.Contains(vertex))
                    {
                        this.OnTreeEdge(e);
                        this.Visit(vertex, depth + 1);
                    }
                    else
                    {
                        if (this.Colors.get_Item(vertex) == 2)
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

        public IIncidenceGraph VisitedGraph
        {
            get
            {
                return this.visitedGraph;
            }
        }
    }
}

