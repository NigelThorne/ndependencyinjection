namespace QuickGraph.Algorithms.MinimumFlow
{
    using QuickGraph.Collections;
    using QuickGraph.Concepts;
    using QuickGraph.Concepts.Algorithms;
    using QuickGraph.Concepts.Collections;
    using QuickGraph.Concepts.MutableTraversals;
    using System;
    using System.Runtime.CompilerServices;

    public class GraphBalancerAlgorithm : IAlgorithm
    {
        private bool balanced;
        private IVertex balancingSink;
        private IEdge balancingSinkEdge;
        private IVertex balancingSource;
        private IEdge balancingSourceEdge;
        private EdgeDoubleDictionary capacities;
        private EdgeCollection deficientEdges;
        private VertexCollection deficientVertices;
        private EdgeIntDictionary preFlow;
        private IVertex sink;
        private IVertex source;
        private EdgeCollection surplusEdges;
        private VertexCollection surplusVertices;
        private IMutableBidirectionalVertexAndEdgeListGraph visitedGraph;

        public event VertexEventHandler BalancingSinkAdded;

        public event VertexEventHandler BalancingSourceAdded;

        public event VertexEventHandler DeficientVertexAdded;

        public event EdgeEventHandler EdgeAdded;

        public event VertexEventHandler SurplusVertexAdded;

        public GraphBalancerAlgorithm(IMutableBidirectionalVertexAndEdgeListGraph visitedGraph, IVertex source, IVertex sink)
        {
            this.source = null;
            this.sink = null;
            this.balancingSource = null;
            this.balancingSourceEdge = null;
            this.balancingSink = null;
            this.balancingSinkEdge = null;
            this.capacities = new EdgeDoubleDictionary();
            this.preFlow = new EdgeIntDictionary();
            this.surplusVertices = new VertexCollection();
            this.surplusEdges = new EdgeCollection();
            this.deficientVertices = new VertexCollection();
            this.deficientEdges = new EdgeCollection();
            this.balanced = false;
            if (visitedGraph == null)
            {
                throw new ArgumentNullException("visitedGraph");
            }
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }
            if (!visitedGraph.ContainsVertex(source))
            {
                throw new ArgumentException("source is not part of the graph");
            }
            if (sink == null)
            {
                throw new ArgumentNullException("sink");
            }
            if (!visitedGraph.ContainsVertex(sink))
            {
                throw new ArgumentException("sink is not part of the graph");
            }
            this.visitedGraph = visitedGraph;
            this.source = source;
            this.sink = sink;
            IEdgeEnumerator enumerator = this.VisitedGraph.get_Edges().GetEnumerator();
            while (enumerator.MoveNext())
            {
                IEdge edge = enumerator.get_Current();
                this.capacities.Add(edge, double.MaxValue);
            }
            IEdgeEnumerator enumerator2 = this.VisitedGraph.get_Edges().GetEnumerator();
            while (enumerator2.MoveNext())
            {
                IEdge edge2 = enumerator2.get_Current();
                this.preFlow.Add(edge2, 1);
            }
        }

        public GraphBalancerAlgorithm(IMutableBidirectionalVertexAndEdgeListGraph visitedGraph, IVertex source, IVertex sink, EdgeDoubleDictionary capacities)
        {
            this.source = null;
            this.sink = null;
            this.balancingSource = null;
            this.balancingSourceEdge = null;
            this.balancingSink = null;
            this.balancingSinkEdge = null;
            this.capacities = new EdgeDoubleDictionary();
            this.preFlow = new EdgeIntDictionary();
            this.surplusVertices = new VertexCollection();
            this.surplusEdges = new EdgeCollection();
            this.deficientVertices = new VertexCollection();
            this.deficientEdges = new EdgeCollection();
            this.balanced = false;
            if (visitedGraph == null)
            {
                throw new ArgumentNullException("visitedGraph");
            }
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }
            if (!visitedGraph.ContainsVertex(source))
            {
                throw new ArgumentException("source is not part of the graph");
            }
            if (sink == null)
            {
                throw new ArgumentNullException("sink");
            }
            if (!visitedGraph.ContainsVertex(sink))
            {
                throw new ArgumentException("sink is not part of the graph");
            }
            if (capacities == null)
            {
                throw new ArgumentNullException("capacities");
            }
            this.visitedGraph = visitedGraph;
            this.source = source;
            this.sink = sink;
            this.capacities = capacities;
            IEdgeEnumerator enumerator = this.VisitedGraph.get_Edges().GetEnumerator();
            while (enumerator.MoveNext())
            {
                IEdge edge = enumerator.get_Current();
                this.preFlow.Add(edge, 1);
            }
        }

        public void Balance()
        {
            if (this.Balanced)
            {
                throw new InvalidOperationException("Graph already balanced");
            }
            this.balancingSource = this.visitedGraph.AddVertex();
            this.OnBalancingSourceAdded();
            this.balancingSink = this.visitedGraph.AddVertex();
            this.OnBalancingSinkAdded();
            this.balancingSourceEdge = this.VisitedGraph.AddEdge(this.BalancingSource, this.Source);
            this.capacities.Add(this.balancingSourceEdge, double.MaxValue);
            this.preFlow.Add(this.balancingSourceEdge, 0);
            this.OnEdgeAdded(this.balancingSourceEdge);
            this.balancingSinkEdge = this.VisitedGraph.AddEdge(this.Sink, this.BalancingSink);
            this.capacities.Add(this.balancingSinkEdge, double.MaxValue);
            this.preFlow.Add(this.balancingSinkEdge, 0);
            this.OnEdgeAdded(this.balancingSinkEdge);
            IVertexEnumerator enumerator = this.VisitedGraph.get_Vertices().GetEnumerator();
            while (enumerator.MoveNext())
            {
                IVertex v = enumerator.get_Current();
                if (((v != this.balancingSource) && (v != this.balancingSink)) && ((v != this.source) && (v != this.sink)))
                {
                    int balancingIndex = this.GetBalancingIndex(v);
                    if (balancingIndex != 0)
                    {
                        if (balancingIndex < 0)
                        {
                            IEdge edge = this.VisitedGraph.AddEdge(this.BalancingSource, v);
                            this.surplusEdges.Add(edge);
                            this.surplusVertices.Add(v);
                            this.preFlow.Add(edge, 0);
                            this.capacities.Add(edge, (double) -balancingIndex);
                            this.OnSurplusVertexAdded(v);
                            this.OnEdgeAdded(edge);
                        }
                        else
                        {
                            IEdge edge2 = this.VisitedGraph.AddEdge(v, this.BalancingSink);
                            this.deficientEdges.Add(edge2);
                            this.deficientVertices.Add(v);
                            this.preFlow.Add(edge2, 0);
                            this.capacities.Add(edge2, (double) balancingIndex);
                            this.OnDeficientVertexAdded(v);
                            this.OnEdgeAdded(edge2);
                        }
                    }
                }
            }
            this.balanced = true;
        }

        public int GetBalancingIndex(IVertex v)
        {
            int num = 0;
            IEdgeEnumerator enumerator = this.VisitedGraph.OutEdges(v).GetEnumerator();
            while (enumerator.MoveNext())
            {
                IEdge edge = enumerator.get_Current();
                int num2 = this.preFlow.get_Item(edge);
                num += num2;
            }
            IEdgeEnumerator enumerator2 = this.VisitedGraph.InEdges(v).GetEnumerator();
            while (enumerator2.MoveNext())
            {
                IEdge edge2 = enumerator2.get_Current();
                int num3 = this.preFlow.get_Item(edge2);
                num -= num3;
            }
            return num;
        }

        protected virtual void OnBalancingSinkAdded()
        {
            if (this.BalancingSinkAdded != null)
            {
                this.BalancingSinkAdded.Invoke(this, new VertexEventArgs(this.sink));
            }
        }

        protected virtual void OnBalancingSourceAdded()
        {
            if (this.BalancingSourceAdded != null)
            {
                this.BalancingSourceAdded.Invoke(this, new VertexEventArgs(this.source));
            }
        }

        protected virtual void OnDeficientVertexAdded(IVertex vertex)
        {
            if (this.DeficientVertexAdded != null)
            {
                this.DeficientVertexAdded.Invoke(this, new VertexEventArgs(vertex));
            }
        }

        protected virtual void OnEdgeAdded(IEdge edge)
        {
            if (this.EdgeAdded != null)
            {
                this.EdgeAdded.Invoke(this, new EdgeEventArgs(edge));
            }
        }

        protected virtual void OnSurplusVertexAdded(IVertex vertex)
        {
            if (this.SurplusVertexAdded != null)
            {
                this.SurplusVertexAdded.Invoke(this, new VertexEventArgs(vertex));
            }
        }

        object IAlgorithm.get_VisitedGraph()
        {
            return this.VisitedGraph;
        }

        public void UnBalance()
        {
            if (!this.Balanced)
            {
                throw new InvalidOperationException("Graph is not balanced");
            }
            EdgeCollection.Enumerator enumerator = this.surplusEdges.GetEnumerator();
            while (enumerator.MoveNext())
            {
                IEdge edge = enumerator.get_Current();
                this.VisitedGraph.RemoveEdge(edge);
                this.capacities.Remove(edge);
                this.preFlow.Remove(edge);
            }
            EdgeCollection.Enumerator enumerator2 = this.deficientEdges.GetEnumerator();
            while (enumerator2.MoveNext())
            {
                IEdge edge2 = enumerator2.get_Current();
                this.VisitedGraph.RemoveEdge(edge2);
                this.capacities.Remove(edge2);
                this.preFlow.Remove(edge2);
            }
            this.capacities.Remove(this.BalancingSinkEdge);
            this.capacities.Remove(this.BalancingSourceEdge);
            this.preFlow.Remove(this.BalancingSinkEdge);
            this.preFlow.Remove(this.BalancingSourceEdge);
            this.VisitedGraph.RemoveEdge(this.BalancingSourceEdge);
            this.VisitedGraph.RemoveEdge(this.BalancingSinkEdge);
            this.VisitedGraph.RemoveVertex(this.BalancingSource);
            this.VisitedGraph.RemoveVertex(this.BalancingSink);
            this.balancingSource = null;
            this.balancingSink = null;
            this.balancingSourceEdge = null;
            this.balancingSinkEdge = null;
            this.surplusEdges.Clear();
            this.deficientEdges.Clear();
            this.surplusVertices.Clear();
            this.deficientVertices.Clear();
            this.balanced = false;
        }

        public bool Balanced
        {
            get
            {
                return this.balanced;
            }
        }

        public IVertex BalancingSink
        {
            get
            {
                return this.balancingSink;
            }
        }

        public IEdge BalancingSinkEdge
        {
            get
            {
                return this.balancingSinkEdge;
            }
        }

        public IVertex BalancingSource
        {
            get
            {
                return this.balancingSource;
            }
        }

        public IEdge BalancingSourceEdge
        {
            get
            {
                return this.balancingSourceEdge;
            }
        }

        public EdgeDoubleDictionary Capacities
        {
            get
            {
                return this.capacities;
            }
        }

        public IEdgeCollection DeficientEdges
        {
            get
            {
                return this.deficientEdges;
            }
        }

        public IVertexCollection DeficientVertices
        {
            get
            {
                return this.deficientVertices;
            }
        }

        public IVertex Sink
        {
            get
            {
                return this.sink;
            }
        }

        public IVertex Source
        {
            get
            {
                return this.source;
            }
        }

        public IEdgeCollection SurplusEdges
        {
            get
            {
                return this.surplusEdges;
            }
        }

        public IVertexCollection SurplusVertices
        {
            get
            {
                return this.surplusVertices;
            }
        }

        public IMutableBidirectionalVertexAndEdgeListGraph VisitedGraph
        {
            get
            {
                return this.visitedGraph;
            }
        }
    }
}

