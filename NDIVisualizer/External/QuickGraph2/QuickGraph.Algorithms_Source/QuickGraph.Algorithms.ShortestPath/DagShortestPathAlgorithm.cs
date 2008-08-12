namespace QuickGraph.Algorithms.ShortestPath
{
    using QuickGraph.Algorithms;
    using QuickGraph.Collections;
    using QuickGraph.Concepts;
    using QuickGraph.Concepts.Algorithms;
    using QuickGraph.Concepts.Collections;
    using QuickGraph.Concepts.Traversals;
    using QuickGraph.Concepts.Visitors;
    using System;
    using System.Collections;
    using System.Runtime.CompilerServices;

    public class DagShortestPathAlgorithm : IVertexColorizerAlgorithm, IAlgorithm
    {
        private VertexColorDictionary m_Colors;
        private VertexDoubleDictionary m_Distances;
        private VertexVertexDictionary m_Predecessors;
        private IVertexListGraph m_VisitedGraph;
        private EdgeDoubleDictionary m_Weights;

        public event VertexEventHandler DiscoverVertex;

        public event EdgeEventHandler EdgeNotRelaxed;

        public event EdgeEventHandler EdgeRelaxed;

        public event EdgeEventHandler ExamineEdge;

        public event VertexEventHandler ExamineVertex;

        public event VertexEventHandler FinishVertex;

        public event VertexEventHandler InitializeVertex;

        public DagShortestPathAlgorithm(IVertexListGraph g, EdgeDoubleDictionary weights)
        {
            if (g == null)
            {
                throw new ArgumentNullException("g");
            }
            if (weights == null)
            {
                throw new ArgumentNullException("Weights");
            }
            this.m_VisitedGraph = g;
            this.m_Colors = new VertexColorDictionary();
            this.m_Distances = new VertexDoubleDictionary();
            this.m_Predecessors = new VertexVertexDictionary();
            this.m_Weights = weights;
        }

        internal double Combine(double d, double w)
        {
            return (d + w);
        }

        internal bool Compare(double a, double b)
        {
            return (a < b);
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
                IVertex vertex = enumerator.get_Current();
                this.Colors.set_Item(vertex, 0);
                this.Predecessors.set_Item(vertex, vertex);
                this.Distances.set_Item(vertex, double.PositiveInfinity);
            }
            this.Colors.set_Item(s, 2);
            this.Distances.set_Item(s, 0.0);
            this.ComputeNoInit(s);
        }

        internal void ComputeNoInit(IVertex s)
        {
            VertexCollection vertices = new VertexCollection();
            new TopologicalSortAlgorithm(this.VisitedGraph, vertices).Compute();
            this.OnDiscoverVertex(s);
            VertexCollection.Enumerator enumerator = vertices.GetEnumerator();
            while (enumerator.MoveNext())
            {
                IVertex v = enumerator.get_Current();
                this.OnExamineVertex(v);
                IEdgeEnumerator enumerator2 = this.VisitedGraph.OutEdges(v).GetEnumerator();
                while (enumerator2.MoveNext())
                {
                    IEdge e = enumerator2.get_Current();
                    this.OnDiscoverVertex(e.get_Target());
                    if (this.Relax(e))
                    {
                        this.OnEdgeRelaxed(e);
                    }
                    else
                    {
                        this.OnEdgeNotRelaxed(e);
                    }
                }
                this.OnFinishVertex(v);
            }
        }

        protected void OnDiscoverVertex(IVertex v)
        {
            if (this.DiscoverVertex != null)
            {
                this.DiscoverVertex.Invoke(this, new VertexEventArgs(v));
            }
        }

        protected void OnEdgeNotRelaxed(IEdge e)
        {
            if (this.EdgeNotRelaxed != null)
            {
                this.EdgeNotRelaxed.Invoke(this, new EdgeEventArgs(e));
            }
        }

        protected void OnEdgeRelaxed(IEdge e)
        {
            if (this.EdgeRelaxed != null)
            {
                this.EdgeRelaxed.Invoke(this, new EdgeEventArgs(e));
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

        protected void OnInitializeVertex(IVertex v)
        {
            if (this.InitializeVertex != null)
            {
                this.InitializeVertex.Invoke(this, new VertexEventArgs(v));
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

        public void RegisterVertexColorizerHandlers(IVertexColorizerVisitor vis)
        {
            this.InitializeVertex = (VertexEventHandler) Delegate.Combine(this.InitializeVertex, new VertexEventHandler(vis, (IntPtr) vis.InitializeVertex));
            this.DiscoverVertex = (VertexEventHandler) Delegate.Combine(this.DiscoverVertex, new VertexEventHandler(vis, (IntPtr) vis.DiscoverVertex));
            this.FinishVertex = (VertexEventHandler) Delegate.Combine(this.FinishVertex, new VertexEventHandler(vis, (IntPtr) vis.FinishVertex));
        }

        internal bool Relax(IEdge e)
        {
            double d = this.m_Distances.get_Item(e.get_Source());
            double b = this.m_Distances.get_Item(e.get_Target());
            double w = this.m_Weights.get_Item(e);
            if (this.Compare(this.Combine(d, w), b))
            {
                this.m_Distances.set_Item(e.get_Target(), this.Combine(d, w));
                this.m_Predecessors.set_Item(e.get_Target(), e.get_Source());
                return true;
            }
            return false;
        }

        public VertexColorDictionary Colors
        {
            get
            {
                return this.m_Colors;
            }
        }

        public VertexDoubleDictionary Distances
        {
            get
            {
                return this.m_Distances;
            }
        }

        public VertexVertexDictionary Predecessors
        {
            get
            {
                return this.m_Predecessors;
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

