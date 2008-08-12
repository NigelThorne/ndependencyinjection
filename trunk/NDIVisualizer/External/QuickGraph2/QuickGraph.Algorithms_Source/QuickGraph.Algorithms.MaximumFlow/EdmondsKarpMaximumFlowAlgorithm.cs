namespace QuickGraph.Algorithms.MaximumFlow
{
    using QuickGraph.Algorithms.Search;
    using QuickGraph.Algorithms.Visitors;
    using QuickGraph.Collections;
    using QuickGraph.Collections.Filtered;
    using QuickGraph.Concepts;
    using QuickGraph.Concepts.Collections;
    using QuickGraph.Concepts.Traversals;
    using QuickGraph.Predicates;
    using System;

    public class EdmondsKarpMaximumFlowAlgorithm : MaximumFlowAlgorithm
    {
        public EdmondsKarpMaximumFlowAlgorithm(IVertexListGraph g, EdgeDoubleDictionary capacities, EdgeEdgeDictionary reversedEdges) : base(g, capacities, reversedEdges)
        {
        }

        internal void Augment(IVertex src, IVertex sink)
        {
            IEdge edge = null;
            IVertex vertex = null;
            double maxValue = double.MaxValue;
            edge = base.Predecessors.get_Item(sink);
            do
            {
                maxValue = Math.Min(maxValue, base.ResidualCapacities.get_Item(edge));
                vertex = edge.get_Source();
                edge = base.Predecessors.get_Item(vertex);
            }
            while (vertex != src);
            edge = base.Predecessors.get_Item(sink);
            do
            {
                EdgeDoubleDictionary dictionary;
                IEdge edge2;
                EdgeDoubleDictionary dictionary2;
                IEdge edge3;
                (dictionary = base.ResidualCapacities).set_Item(edge2 = edge, dictionary.get_Item(edge2) - maxValue);
                (dictionary2 = base.ResidualCapacities).set_Item(edge3 = base.ReversedEdges.get_Item(edge), dictionary2.get_Item(edge3) + maxValue);
                vertex = edge.get_Source();
                edge = base.Predecessors.get_Item(vertex);
            }
            while (vertex != src);
        }

        public override double Compute(IVertex src, IVertex sink)
        {
            if (src == null)
            {
                throw new ArgumentNullException("src");
            }
            if (sink == null)
            {
                throw new ArgumentNullException("sink");
            }
            IVertexEnumerator enumerator = base.VisitedGraph.get_Vertices().GetEnumerator();
            while (enumerator.MoveNext())
            {
                IVertex vertex = enumerator.get_Current();
                IEdgeEnumerator enumerator2 = base.VisitedGraph.OutEdges(vertex).GetEnumerator();
                while (enumerator2.MoveNext())
                {
                    IEdge edge = enumerator2.get_Current();
                    base.ResidualCapacities.set_Item(edge, base.Capacities.get_Item(edge));
                }
            }
            base.Colors.set_Item(sink, 2);
            while (base.Colors.get_Item(sink) != null)
            {
                PredecessorRecorderVisitor vis = new PredecessorRecorderVisitor(base.Predecessors);
                VertexBuffer q = new VertexBuffer();
                BreadthFirstSearchAlgorithm algorithm = new BreadthFirstSearchAlgorithm(this.ResidualGraph, q, base.Colors);
                algorithm.RegisterPredecessorRecorderHandlers(vis);
                algorithm.Compute(src);
                if (base.Colors.get_Item(sink) != null)
                {
                    this.Augment(src, sink);
                }
            }
            double num = 0.0;
            IEdgeEnumerator enumerator3 = base.VisitedGraph.OutEdges(src).GetEnumerator();
            while (enumerator3.MoveNext())
            {
                IEdge edge2 = enumerator3.get_Current();
                num += base.Capacities.get_Item(edge2) - base.ResidualCapacities.get_Item(edge2);
            }
            return num;
        }

        internal IVertexListGraph ResidualGraph
        {
            get
            {
                return new FilteredVertexListGraph(base.VisitedGraph, new ResidualEdgePredicate(base.ResidualCapacities));
            }
        }
    }
}

