namespace QuickGraph.Algorithms.ShortestPath
{
    using QuickGraph.Collections;
    using QuickGraph.Concepts;
    using QuickGraph.Concepts.Collections;
    using QuickGraph.Concepts.Traversals;
    using System;
    using System.Runtime.CompilerServices;

    public class BellmanFordShortestPathAlgorithm
    {
        private VertexColorDictionary m_Colors;
        private VertexDoubleDictionary m_Distances;
        private VertexVertexDictionary m_Predecessors;
        private IVertexAndEdgeListGraph m_VisitedGraph;
        private EdgeDoubleDictionary m_Weights;

        public event EdgeEventHandler EdgeMinimized;

        public event EdgeEventHandler EdgeNotMinimized;

        public event EdgeEventHandler EdgeNotRelaxed;

        public event EdgeEventHandler EdgeRelaxed;

        public event EdgeEventHandler ExamineEdge;

        public event VertexEventHandler InitializeVertex;

        public BellmanFordShortestPathAlgorithm(IVertexAndEdgeListGraph g, EdgeDoubleDictionary weights)
        {
            if (weights == null)
            {
                throw new ArgumentNullException("Weights");
            }
            this.m_VisitedGraph = g;
            this.m_Colors = new VertexColorDictionary();
            this.m_Weights = weights;
            this.m_Distances = new VertexDoubleDictionary();
            this.m_Predecessors = new VertexVertexDictionary();
        }

        internal double Combine(double d, double w)
        {
            return (d + w);
        }

        internal bool Compare(double a, double b)
        {
            return (a < b);
        }

        public bool Compute()
        {
            int num = this.VisitedGraph.get_VerticesCount();
            for (int i = 0; i < num; i++)
            {
                bool flag = false;
                IEdgeEnumerator enumerator = this.VisitedGraph.get_Edges().GetEnumerator();
                while (enumerator.MoveNext())
                {
                    IEdge e = enumerator.get_Current();
                    this.OnExamineEdge(e);
                    if (this.Relax(e))
                    {
                        flag = true;
                        this.OnEdgeRelaxed(e);
                    }
                    else
                    {
                        this.OnEdgeNotRelaxed(e);
                    }
                }
                if (!flag)
                {
                    break;
                }
            }
            IEdgeEnumerator enumerator2 = this.VisitedGraph.get_Edges().GetEnumerator();
            while (enumerator2.MoveNext())
            {
                IEdge edge2 = enumerator2.get_Current();
                if (this.Compare(this.Combine(this.Distances.get_Item(edge2.get_Source()), this.Weights.get_Item(edge2)), this.Distances.get_Item(edge2.get_Target())))
                {
                    this.OnEdgeMinimized(edge2);
                    return false;
                }
                this.OnEdgeNotMinimized(edge2);
            }
            return true;
        }

        public bool Compute(IVertex s)
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
                this.Predecessors.set_Item(v, v);
                this.Distances.set_Item(v, double.PositiveInfinity);
                this.OnInitializeVertex(v);
            }
            return this.Compute();
        }

        protected void OnEdgeMinimized(IEdge e)
        {
            if (this.EdgeMinimized != null)
            {
                this.EdgeMinimized.Invoke(this, new EdgeEventArgs(e));
            }
        }

        protected void OnEdgeNotMinimized(IEdge e)
        {
            if (this.EdgeNotMinimized != null)
            {
                this.EdgeNotMinimized.Invoke(this, new EdgeEventArgs(e));
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

        protected void OnInitializeVertex(IVertex v)
        {
            if (this.InitializeVertex != null)
            {
                this.InitializeVertex.Invoke(this, new VertexEventArgs(v));
            }
        }

        internal bool Relax(IEdge e)
        {
            double d = this.m_Distances.get_Item(e.get_Source());
            double b = this.m_Distances.get_Item(e.get_Target());
            double w = this.m_Weights.get_Item(e);
            if (this.Compare(this.Combine(d, w), b))
            {
                this.Distances.set_Item(e.get_Target(), this.Combine(d, w));
                this.Predecessors.set_Item(e.get_Target(), e.get_Source());
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

        public IVertexAndEdgeListGraph VisitedGraph
        {
            get
            {
                return this.m_VisitedGraph;
            }
        }

        public EdgeDoubleDictionary Weights
        {
            get
            {
                return this.m_Weights;
            }
        }
    }
}

