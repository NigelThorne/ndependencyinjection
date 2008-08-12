namespace QuickGraph.Algorithms.MinimumFlow
{
    using QuickGraph.Algorithms.MaximumFlow;
    using QuickGraph.Collections;
    using QuickGraph.Concepts;
    using QuickGraph.Concepts.Algorithms;
    using QuickGraph.Concepts.Collections;
    using QuickGraph.Representations;
    using System;

    public class MinimumFlowAlgorithm : IAlgorithm
    {
        private GraphBalancerAlgorithm balancer;
        private EdgeDoubleDictionary capacities;
        private MaximumFlowAlgorithm maxFlowF1;
        private MaximumFlowAlgorithm maxFlowf2;
        private ReversedEdgeAugmentorAlgorithm reverser;
        private BidirectionalGraph visitedGraph;

        public MinimumFlowAlgorithm(BidirectionalGraph visitedGraph)
        {
            this.reverser = null;
            this.balancer = null;
            this.maxFlowF1 = null;
            this.maxFlowf2 = null;
            if (visitedGraph == null)
            {
                throw new ArgumentNullException("visitedGraph");
            }
            if (this.capacities == null)
            {
                throw new ArgumentNullException("capacities");
            }
            this.visitedGraph = visitedGraph;
            this.capacities = new EdgeDoubleDictionary();
            VertexEdgesEnumerator enumerator = this.visitedGraph.Edges.GetEnumerator();
            while (enumerator.MoveNext())
            {
                IEdge edge = enumerator.get_Current();
                this.capacities.Add(edge, double.MaxValue);
            }
            this.Initialize();
        }

        public MinimumFlowAlgorithm(BidirectionalGraph visitedGraph, EdgeDoubleDictionary capacities)
        {
            this.reverser = null;
            this.balancer = null;
            this.maxFlowF1 = null;
            this.maxFlowf2 = null;
            if (visitedGraph == null)
            {
                throw new ArgumentNullException("visitedGraph");
            }
            if (capacities == null)
            {
                throw new ArgumentNullException("capacities");
            }
            this.visitedGraph = visitedGraph;
            this.capacities = capacities;
            this.Initialize();
        }

        public void Compute(IVertex source, IVertex sink)
        {
            this.balancer = new GraphBalancerAlgorithm(this.VisitedGraph, source, sink, this.capacities);
            this.balancer.Balance();
            this.capacities.set_Item(this.balancer.BalancingSourceEdge, 0.0);
            this.capacities.set_Item(this.balancer.BalancingSinkEdge, 0.0);
            this.reverser.AddReversedEdges();
            this.maxFlowF1.Compute(source, sink);
            this.capacities.set_Item(this.balancer.BalancingSourceEdge, double.MaxValue);
            IEdgeEnumerator enumerator = this.balancer.SurplusEdges.GetEnumerator();
            while (enumerator.MoveNext())
            {
                IVertex v = enumerator.get_Current().get_Target();
                EdgeCollection.Enumerator enumerator2 = this.VisitedGraph.OutEdges(v).GetEnumerator();
                while (enumerator2.MoveNext())
                {
                    IEdge edge2 = enumerator2.get_Current();
                    if (edge2.get_Target() == this.balancer.BalancingSource)
                    {
                        this.capacities.set_Item(edge2, 0.0);
                    }
                }
            }
        }

        private void Initialize()
        {
            this.reverser = new ReversedEdgeAugmentorAlgorithm(this.VisitedGraph);
            this.reverser.ReversedEdgeAdded += new EdgeEventHandler(this, (IntPtr) this.reverser_ReversedEdgeAdded);
            this.maxFlowF1 = new PushRelabelMaximumFlowAlgorithm(this.VisitedGraph, this.capacities, this.reverser.ReversedEdges);
        }

        object IAlgorithm.get_VisitedGraph()
        {
            return this.VisitedGraph;
        }

        private void reverser_ReversedEdgeAdded(object sender, EdgeEventArgs e)
        {
            this.capacities.set_Item(e.get_Edge(), 0.0);
        }

        public BidirectionalGraph VisitedGraph
        {
            get
            {
                return this.visitedGraph;
            }
        }
    }
}

