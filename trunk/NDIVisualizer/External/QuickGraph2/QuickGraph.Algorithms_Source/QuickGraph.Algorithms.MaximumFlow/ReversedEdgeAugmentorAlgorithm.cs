namespace QuickGraph.Algorithms.MaximumFlow
{
    using QuickGraph.Collections;
    using QuickGraph.Concepts;
    using QuickGraph.Concepts.Algorithms;
    using QuickGraph.Concepts.Collections;
    using QuickGraph.Concepts.MutableTraversals;
    using System;
    using System.Runtime.CompilerServices;

    public class ReversedEdgeAugmentorAlgorithm : IAlgorithm
    {
        private bool augmented = false;
        private EdgeCollection augmentedEgdes = new EdgeCollection();
        private EdgeEdgeDictionary reversedEdges = new EdgeEdgeDictionary();
        private IMutableVertexAndEdgeListGraph visitedGraph;

        public event EdgeEventHandler ReversedEdgeAdded;

        public ReversedEdgeAugmentorAlgorithm(IMutableVertexAndEdgeListGraph visitedGraph)
        {
            if (visitedGraph == null)
            {
                throw new ArgumentNullException("visitedGraph");
            }
            this.visitedGraph = visitedGraph;
        }

        public void AddReversedEdges()
        {
            if (this.Augmented)
            {
                throw new InvalidOperationException("Graph already augmented");
            }
            EdgeCollection edges = new EdgeCollection();
            IEdgeEnumerator enumerator = this.VisitedGraph.get_Edges().GetEnumerator();
            while (enumerator.MoveNext())
            {
                IEdge edge = enumerator.get_Current();
                if (!this.reversedEdges.Contains(edge))
                {
                    IEdge edge2 = this.FindReversedEdge(edge);
                    if (edge2 != null)
                    {
                        this.reversedEdges.set_Item(edge, edge2);
                        if (!this.reversedEdges.Contains(edge2))
                        {
                            this.reversedEdges.set_Item(edge2, edge);
                        }
                    }
                    else
                    {
                        edges.Add(edge);
                    }
                }
            }
            EdgeCollection.Enumerator enumerator2 = edges.GetEnumerator();
            while (enumerator2.MoveNext())
            {
                IEdge edge3 = enumerator2.get_Current();
                if (!this.reversedEdges.Contains(edge3))
                {
                    IEdge edge4 = this.FindReversedEdge(edge3);
                    if (edge4 != null)
                    {
                        this.reversedEdges.set_Item(edge3, edge4);
                    }
                    else
                    {
                        edge4 = this.VisitedGraph.AddEdge(edge3.get_Target(), edge3.get_Source());
                        this.augmentedEgdes.Add(edge4);
                        this.reversedEdges.set_Item(edge3, edge4);
                        this.reversedEdges.set_Item(edge4, edge3);
                        this.OnReservedEdgeAdded(new EdgeEventArgs(edge4));
                    }
                }
            }
            this.augmented = true;
        }

        protected IEdge FindReversedEdge(IEdge edge)
        {
            IEdgeEnumerator enumerator = this.VisitedGraph.OutEdges(edge.get_Target()).GetEnumerator();
            while (enumerator.MoveNext())
            {
                IEdge edge2 = enumerator.get_Current();
                if (edge2.get_Target() == edge.get_Source())
                {
                    return edge2;
                }
            }
            return null;
        }

        protected virtual void OnReservedEdgeAdded(EdgeEventArgs e)
        {
            if (this.ReversedEdgeAdded != null)
            {
                this.ReversedEdgeAdded.Invoke(this, e);
            }
        }

        object IAlgorithm.get_VisitedGraph()
        {
            return this.VisitedGraph;
        }

        public void RemoveReversedEdges()
        {
            if (!this.Augmented)
            {
                throw new InvalidOperationException("Graph is not yet augmented");
            }
            EdgeCollection.Enumerator enumerator = this.augmentedEgdes.GetEnumerator();
            while (enumerator.MoveNext())
            {
                IEdge edge = enumerator.get_Current();
                this.VisitedGraph.RemoveEdge(edge);
            }
            this.augmentedEgdes.Clear();
            this.reversedEdges.Clear();
            this.augmented = false;
        }

        public bool Augmented
        {
            get
            {
                return this.augmented;
            }
        }

        public EdgeCollection AugmentedEdges
        {
            get
            {
                return this.augmentedEgdes;
            }
        }

        public EdgeEdgeDictionary ReversedEdges
        {
            get
            {
                return this.reversedEdges;
            }
        }

        public IMutableVertexAndEdgeListGraph VisitedGraph
        {
            get
            {
                return this.visitedGraph;
            }
        }
    }
}

