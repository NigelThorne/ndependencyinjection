namespace QuickGraph.Algorithms.Metrics
{
    using QuickGraph.Collections;
    using QuickGraph.Concepts;
    using QuickGraph.Concepts.Collections;
    using QuickGraph.Concepts.Traversals;
    using System;

    public class VertexCoverageMetric
    {
        private double coverage = 0.0;
        private IVertexListGraph graph;
        private VertexIntDictionary passCounts = new VertexIntDictionary();

        public VertexCoverageMetric(IVertexListGraph graph)
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
            IVertexEnumerator enumerator = this.graph.get_Vertices().GetEnumerator();
            while (enumerator.MoveNext())
            {
                IVertex vertex = enumerator.get_Current();
                this.passCounts.set_Item(vertex, 0);
            }
        }

        public void Measure(IVertexEnumerable vertices)
        {
            if (vertices == null)
            {
                throw new ArgumentNullException("vertices");
            }
            this.Clear();
            int num = 0;
            IVertexEnumerator enumerator = vertices.GetEnumerator();
            while (enumerator.MoveNext())
            {
                VertexIntDictionary dictionary;
                IVertex vertex2;
                IVertex vertex = enumerator.get_Current();
                if (this.passCounts.get_Item(vertex) == 0)
                {
                    num++;
                }
                (dictionary = this.passCounts).set_Item(vertex2 = vertex, dictionary.get_Item(vertex2) + 1);
            }
            if (this.graph.get_VerticesEmpty())
            {
                this.coverage = 0.0;
            }
            else
            {
                this.coverage = ((double) num) / ((double) this.graph.get_VerticesCount());
            }
        }

        public void MeasurePath(IEdgeEnumerable edges)
        {
            this.Clear();
            int num = 0;
            bool flag = true;
            IEdgeEnumerator enumerator = edges.GetEnumerator();
            while (enumerator.MoveNext())
            {
                VertexIntDictionary dictionary2;
                IVertex vertex2;
                IEdge edge = enumerator.get_Current();
                if (flag)
                {
                    VertexIntDictionary dictionary;
                    IVertex vertex;
                    (dictionary = this.passCounts).set_Item(vertex = edge.get_Source(), dictionary.get_Item(vertex) + 1);
                    flag = false;
                }
                if (this.passCounts.get_Item(edge.get_Target()) == 0)
                {
                    num++;
                }
                (dictionary2 = this.passCounts).set_Item(vertex2 = edge.get_Target(), dictionary2.get_Item(vertex2) + 1);
            }
            if (this.graph.get_VerticesEmpty())
            {
                this.coverage = 0.0;
            }
            else
            {
                this.coverage = ((double) num) / ((double) this.graph.get_VerticesCount());
            }
        }

        public double Coverage
        {
            get
            {
                return this.coverage;
            }
        }

        public IVertexListGraph Graph
        {
            get
            {
                return this.graph;
            }
        }

        public VertexIntDictionary PassCounts
        {
            get
            {
                return this.PassCounts;
            }
        }
    }
}

