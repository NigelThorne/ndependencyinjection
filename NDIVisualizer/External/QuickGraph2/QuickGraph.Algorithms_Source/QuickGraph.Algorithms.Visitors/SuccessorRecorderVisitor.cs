namespace QuickGraph.Algorithms.Visitors
{
    using QuickGraph.Collections;
    using QuickGraph.Concepts;
    using System;

    public class SuccessorRecorderVisitor
    {
        private VertexEdgeDictionary successors = new VertexEdgeDictionary();

        public void ClearTreeVertex(object sender, VertexEventArgs args)
        {
            this.Successors.Remove(args.get_Vertex());
        }

        public void InitializeVertex(object sender, VertexEventArgs args)
        {
            this.Successors.Remove(args.get_Vertex());
        }

        public void TreeEdge(object sender, EdgeEventArgs args)
        {
            if (args.get_Edge() is ReversedEdge)
            {
                this.Successors.set_Item(args.get_Edge().get_Source(), ((ReversedEdge) args.get_Edge()).get_Wrapped());
            }
            else
            {
                this.Successors.set_Item(args.get_Edge().get_Source(), args.get_Edge());
            }
        }

        public VertexEdgeDictionary Successors
        {
            get
            {
                return this.successors;
            }
        }
    }
}

