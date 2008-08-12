namespace QuickGraph.Algorithms.ShortestPath
{
    using QuickGraph.Algorithms.Search;
    using QuickGraph.Collections;
    using QuickGraph.Concepts;
    using QuickGraph.Concepts.Algorithms;
    using QuickGraph.Concepts.Collections;
    using QuickGraph.Concepts.Traversals;
    using QuickGraph.Concepts.Visitors;
    using System;
    using System.Collections;
    using System.Runtime.CompilerServices;

    public class DijkstraShortestPathAlgorithm : IVertexColorizerAlgorithm, IPredecessorRecorderAlgorithm, IDistanceRecorderAlgorithm, IAlgorithm
    {
        private VertexColorDictionary colors;
        private VertexDoubleDictionary distances;
        private PriorithizedVertexBuffer vertexQueue;
        private IVertexListGraph visitedGraph;
        private EdgeDoubleDictionary weights;

        public event VertexEventHandler DiscoverVertex;

        public event EdgeEventHandler EdgeNotRelaxed;

        public event EdgeEventHandler EdgeRelaxed;

        public event EdgeEventHandler ExamineEdge;

        public event VertexEventHandler ExamineVertex;

        public event VertexEventHandler FinishVertex;

        public event VertexEventHandler InitializeVertex;

        public DijkstraShortestPathAlgorithm(IVertexListGraph g, EdgeDoubleDictionary weights)
        {
            if (g == null)
            {
                throw new ArgumentNullException("g");
            }
            if (weights == null)
            {
                throw new ArgumentNullException("Weights");
            }
            this.visitedGraph = g;
            this.colors = new VertexColorDictionary();
            this.distances = new VertexDoubleDictionary();
            this.weights = weights;
            this.vertexQueue = null;
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
                this.colors.set_Item(vertex, 0);
                this.distances.set_Item(vertex, double.PositiveInfinity);
            }
            this.Colors.set_Item(s, 2);
            this.Distances.set_Item(s, 0.0);
            this.ComputeNoInit(s);
        }

        public void ComputeNoInit(IVertex s)
        {
            this.vertexQueue = new PriorithizedVertexBuffer(this.distances);
            BreadthFirstSearchAlgorithm algorithm = new BreadthFirstSearchAlgorithm(this.VisitedGraph, this.vertexQueue, this.Colors);
            algorithm.InitializeVertex += this.InitializeVertex;
            algorithm.DiscoverVertex += this.DiscoverVertex;
            algorithm.ExamineEdge += this.ExamineEdge;
            algorithm.ExamineVertex += this.ExamineVertex;
            algorithm.FinishVertex += this.FinishVertex;
            algorithm.TreeEdge += new EdgeEventHandler(this, (IntPtr) this.TreeEdge);
            algorithm.GrayTarget += new EdgeEventHandler(this, (IntPtr) this.GrayTarget);
            algorithm.Visit(s);
        }

        private void GrayTarget(object sender, EdgeEventArgs args)
        {
            if (this.Relax(args.get_Edge()))
            {
                this.vertexQueue.Update(args.get_Edge().get_Target());
                this.OnEdgeRelaxed(args.get_Edge());
            }
            else
            {
                this.OnEdgeNotRelaxed(args.get_Edge());
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
            this.InitializeVertex = (VertexEventHandler) Delegate.Combine(this.InitializeVertex, new VertexEventHandler(vis, (IntPtr) vis.InitializeVertex));
            this.DiscoverVertex = (VertexEventHandler) Delegate.Combine(this.DiscoverVertex, new VertexEventHandler(vis, (IntPtr) vis.DiscoverVertex));
            this.EdgeRelaxed = (EdgeEventHandler) Delegate.Combine(this.EdgeRelaxed, new EdgeEventHandler(vis, (IntPtr) vis.TreeEdge));
        }

        public void RegisterPredecessorRecorderHandlers(IPredecessorRecorderVisitor vis)
        {
            this.EdgeRelaxed = (EdgeEventHandler) Delegate.Combine(this.EdgeRelaxed, new EdgeEventHandler(vis, (IntPtr) vis.TreeEdge));
            this.FinishVertex = (VertexEventHandler) Delegate.Combine(this.FinishVertex, new VertexEventHandler(vis, (IntPtr) vis.FinishVertex));
        }

        public void RegisterVertexColorizerHandlers(IVertexColorizerVisitor vis)
        {
            this.InitializeVertex = (VertexEventHandler) Delegate.Combine(this.InitializeVertex, new VertexEventHandler(vis, (IntPtr) vis.InitializeVertex));
            this.DiscoverVertex = (VertexEventHandler) Delegate.Combine(this.DiscoverVertex, new VertexEventHandler(vis, (IntPtr) vis.DiscoverVertex));
            this.FinishVertex = (VertexEventHandler) Delegate.Combine(this.FinishVertex, new VertexEventHandler(vis, (IntPtr) vis.FinishVertex));
        }

        internal bool Relax(IEdge e)
        {
            double d = this.distances.get_Item(e.get_Source());
            double b = this.distances.get_Item(e.get_Target());
            double w = this.weights.get_Item(e);
            if (this.Compare(this.Combine(d, w), b))
            {
                this.distances.set_Item(e.get_Target(), this.Combine(d, w));
                return true;
            }
            return false;
        }

        private void TreeEdge(object sender, EdgeEventArgs args)
        {
            if (this.Relax(args.get_Edge()))
            {
                this.OnEdgeRelaxed(args.get_Edge());
            }
            else
            {
                this.OnEdgeNotRelaxed(args.get_Edge());
            }
        }

        public static EdgeDoubleDictionary UnaryWeightsFromEdgeList(IEdgeListGraph graph)
        {
            if (graph == null)
            {
                throw new ArgumentNullException("graph");
            }
            EdgeDoubleDictionary dictionary = new EdgeDoubleDictionary();
            IEdgeEnumerator enumerator = graph.get_Edges().GetEnumerator();
            while (enumerator.MoveNext())
            {
                IEdge edge = enumerator.get_Current();
                dictionary.set_Item(edge, 1.0);
            }
            return dictionary;
        }

        public static EdgeDoubleDictionary UnaryWeightsFromVertexList(IVertexListGraph graph)
        {
            if (graph == null)
            {
                throw new ArgumentNullException("graph");
            }
            EdgeDoubleDictionary dictionary = new EdgeDoubleDictionary();
            IVertexEnumerator enumerator = graph.get_Vertices().GetEnumerator();
            while (enumerator.MoveNext())
            {
                IVertex vertex = enumerator.get_Current();
                IEdgeEnumerator enumerator2 = graph.OutEdges(vertex).GetEnumerator();
                while (enumerator2.MoveNext())
                {
                    IEdge edge = enumerator2.get_Current();
                    dictionary.set_Item(edge, 1.0);
                }
            }
            return dictionary;
        }

        public VertexColorDictionary Colors
        {
            get
            {
                return this.colors;
            }
        }

        public VertexDoubleDictionary Distances
        {
            get
            {
                return this.distances;
            }
        }

        protected VertexBuffer VertexQueue
        {
            get
            {
                return this.vertexQueue;
            }
        }

        public IVertexListGraph VisitedGraph
        {
            get
            {
                return this.visitedGraph;
            }
        }
    }
}

