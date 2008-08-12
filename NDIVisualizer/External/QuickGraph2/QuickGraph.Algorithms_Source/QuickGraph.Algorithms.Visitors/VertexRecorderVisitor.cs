namespace QuickGraph.Algorithms.Visitors
{
    using QuickGraph.Collections;
    using QuickGraph.Concepts;
    using System;

    public class VertexRecorderVisitor
    {
        private VertexCollection vertices;

        public VertexRecorderVisitor()
        {
            this.vertices = new VertexCollection();
        }

        public VertexRecorderVisitor(VertexCollection vertices)
        {
            if (vertices == null)
            {
                throw new ArgumentNullException("vertices");
            }
            this.vertices = vertices;
        }

        public void RecordSource(object sender, EdgeEventArgs args)
        {
            this.vertices.Add(args.get_Edge().get_Source());
        }

        public void RecordTarget(object sender, EdgeEventArgs args)
        {
            this.vertices.Add(args.get_Edge().get_Target());
        }

        public void RecordVertex(object sender, VertexEventArgs args)
        {
            this.vertices.Add(args.get_Vertex());
        }

        public VertexCollection Vertices
        {
            get
            {
                return this.vertices;
            }
        }
    }
}

