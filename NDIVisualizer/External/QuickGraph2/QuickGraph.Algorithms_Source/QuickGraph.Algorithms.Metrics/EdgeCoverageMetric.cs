namespace QuickGraph.Algorithms.Metrics
{
    using QuickGraph.Collections;
    using QuickGraph.Concepts;
    using QuickGraph.Concepts.Collections;
    using QuickGraph.Concepts.Traversals;
    using System;

    public class EdgeCoverageMetric
    {
        private double coverage = 0.0;
        private IEdgeListGraph graph;
        private EdgeIntDictionary passCounts = new EdgeIntDictionary();

        public EdgeCoverageMetric(IEdgeListGraph graph)
        {
            if (graph == null)
            {
                throw new ArgumentNullException("graph");
            }
            this.graph = graph;
        }

        public void Clear()
        {
            this.passCounts.Clear();
            this.coverage = 0.0;
            IEdgeEnumerator enumerator = this.graph.get_Edges().GetEnumerator();
            while (enumerator.MoveNext())
            {
                IEdge edge = enumerator.get_Current();
                this.passCounts.set_Item(edge, 0);
            }
        }

        public void Measure(IEdgeEnumerable edges)
        {
            this.Clear();
            int num = 0;
            IEdgeEnumerator enumerator = edges.GetEnumerator();
            while (enumerator.MoveNext())
            {
                EdgeIntDictionary dictionary;
                IEdge edge2;
                IEdge edge = enumerator.get_Current();
                if (this.passCounts.get_Item(edge) == 0)
                {
                    num++;
                }
                (dictionary = this.passCounts).set_Item(edge2 = edge, dictionary.get_Item(edge2) + 1);
            }
            if (this.graph.get_EdgesEmpty())
            {
                this.coverage = 0.0;
            }
            else
            {
                this.coverage = ((double) num) / ((double) this.graph.get_EdgesCount());
            }
        }

        public double Coverage
        {
            get
            {
                return this.coverage;
            }
        }

        public IEdgeListGraph Graph
        {
            get
            {
                return this.graph;
            }
        }

        public EdgeIntDictionary PassCounts
        {
            get
            {
                return this.PassCounts;
            }
        }
    }
}

