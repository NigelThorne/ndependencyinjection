namespace QuickGraph.Algorithms.Visitors
{
    using QuickGraph.Collections;
    using QuickGraph.Concepts;
    using QuickGraph.Concepts.Visitors;
    using System;

    public class DistanceRecorderVisitor : IDistanceRecorderVisitor
    {
        private VertexIntDictionary distances;

        public DistanceRecorderVisitor()
        {
            this.distances = new VertexIntDictionary();
        }

        public DistanceRecorderVisitor(VertexIntDictionary distances)
        {
            if (distances == null)
            {
                throw new ArgumentNullException("distances");
            }
            this.distances = distances;
        }

        public void DiscoverVertex(object sender, VertexEventArgs args)
        {
            this.distances.set_Item(args.get_Vertex(), 0);
        }

        public void InitializeVertex(object sender, VertexEventArgs args)
        {
            this.distances.set_Item(args.get_Vertex(), 0x7fffffff);
        }

        public void TreeEdge(object sender, EdgeEventArgs args)
        {
            this.distances.set_Item(args.get_Edge().get_Target(), this.distances.get_Item(args.get_Edge().get_Source()) + 1);
        }

        public VertexIntDictionary Distances
        {
            get
            {
                return this.distances;
            }
        }
    }
}

