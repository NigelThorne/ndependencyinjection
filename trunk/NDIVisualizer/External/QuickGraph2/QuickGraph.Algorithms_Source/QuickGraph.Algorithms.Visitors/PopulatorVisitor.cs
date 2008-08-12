namespace QuickGraph.Algorithms.Visitors
{
    using QuickGraph.Concepts;
    using QuickGraph.Concepts.Serialization;
    using System;

    public class PopulatorVisitor
    {
        private ISerializableVertexAndEdgeListGraph graph;

        public PopulatorVisitor(ISerializableVertexAndEdgeListGraph graph)
        {
            if (graph == null)
            {
                throw new ArgumentNullException("graph");
            }
            this.graph = graph;
        }

        public void BackForwardOrCrossEdge(object sender, EdgeEventArgs e)
        {
            this.graph.AddEdge(e.get_Edge());
        }

        public void StartVertex(object sender, VertexEventArgs e)
        {
            this.graph.AddVertex(e.get_Vertex());
        }

        public void TreeEdge(object sender, EdgeEventArgs e)
        {
            this.graph.AddVertex(e.get_Edge().get_Target());
            this.graph.AddEdge(e.get_Edge());
        }
    }
}

