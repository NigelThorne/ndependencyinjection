namespace QuickGraph.Algorithms.Visitors
{
    using QuickGraph.Collections;
    using QuickGraph.Concepts;
    using QuickGraph.Concepts.Traversals;
    using QuickGraph.Concepts.Visitors;
    using System;

    public class SinkRecorderVisitor : IVertexColorizerVisitor
    {
        private VertexCollection sinks;
        private IIncidenceGraph visitedGraph;

        public SinkRecorderVisitor(IIncidenceGraph g)
        {
            this.sinks = new VertexCollection();
            if (g == null)
            {
                throw new ArgumentNullException("g");
            }
            this.visitedGraph = g;
        }

        public SinkRecorderVisitor(IIncidenceGraph g, VertexCollection sinks)
        {
            this.sinks = new VertexCollection();
            if (g == null)
            {
                throw new ArgumentNullException("g");
            }
            if (sinks == null)
            {
                throw new ArgumentNullException("sinks");
            }
            this.visitedGraph = g;
            this.sinks = sinks;
        }

        public void DiscoverVertex(object sender, VertexEventArgs args)
        {
        }

        public void FinishVertex(object sender, VertexEventArgs args)
        {
            if (this.VisitedGraph.OutEdgesEmpty(args.get_Vertex()))
            {
                this.sinks.Add(args.get_Vertex());
            }
        }

        public void InitializeVertex(object sender, VertexEventArgs args)
        {
        }

        public VertexCollection Sinks
        {
            get
            {
                return this.sinks;
            }
        }

        public IIncidenceGraph VisitedGraph
        {
            get
            {
                return this.visitedGraph;
            }
        }
    }
}

