namespace QuickGraph.Algorithms.Visitors
{
    using QuickGraph.Collections;
    using QuickGraph.Concepts;
    using System;

    public class EdgeRecorderVisitor
    {
        private EdgeCollection edges;

        public EdgeRecorderVisitor()
        {
            this.edges = new EdgeCollection();
        }

        public EdgeRecorderVisitor(EdgeCollection edges)
        {
            if (edges == null)
            {
                throw new ArgumentNullException("edges");
            }
            this.edges = edges;
        }

        public void RecordEdge(object sender, EdgeEventArgs args)
        {
            this.Edges.Add(args.get_Edge());
        }

        public void RecordSource(object sender, EdgeEdgeEventArgs args)
        {
            this.Edges.Add(args.get_Edge());
        }

        public void RecordTarget(object sender, EdgeEdgeEventArgs args)
        {
            this.Edges.Add(args.get_TargetEdge());
        }

        public EdgeCollection Edges
        {
            get
            {
                return this.edges;
            }
        }
    }
}

