namespace QuickGraph.Algorithms.Clone
{
    using QuickGraph.Collections;
    using QuickGraph.Concepts;
    using QuickGraph.Concepts.Collections;
    using QuickGraph.Concepts.Modifications;
    using QuickGraph.Concepts.Traversals;
    using System;
    using System.Runtime.CompilerServices;

    public class GraphClonerAlgorithm
    {
        public event CloneEdgeEventHandler CloneEdge;

        public event CloneVertexEventHandler CloneVertex;

        public void Clone(IVertexAndEdgeListGraph source, IEdgeMutableGraph target)
        {
            VertexVertexDictionary dictionary = new VertexVertexDictionary();
            IVertexEnumerator enumerator = source.get_Vertices().GetEnumerator();
            while (enumerator.MoveNext())
            {
                IVertex v = enumerator.get_Current();
                IVertex vc = target.AddVertex();
                this.OnCloneVertex(v, vc);
                dictionary.set_Item(v, vc);
            }
            IEdgeEnumerator enumerator2 = source.get_Edges().GetEnumerator();
            while (enumerator2.MoveNext())
            {
                IEdge e = enumerator2.get_Current();
                IEdge ec = target.AddEdge(dictionary.get_Item(e.get_Source()), dictionary.get_Item(e.get_Target()));
                this.OnCloneEdge(e, ec);
            }
        }

        protected void OnCloneEdge(IEdge e, IEdge ec)
        {
            if (this.CloneEdge != null)
            {
                this.CloneEdge(this, new CloneEdgeEventArgs(e, ec));
            }
        }

        protected void OnCloneVertex(IVertex v, IVertex vc)
        {
            if (this.CloneVertex != null)
            {
                this.CloneVertex(this, new CloneVertexEventArgs(v, vc));
            }
        }

        public void ReversedClone(IVertexAndEdgeListGraph source, IEdgeMutableGraph target)
        {
            VertexVertexDictionary dictionary = new VertexVertexDictionary();
            IVertexEnumerator enumerator = source.get_Vertices().GetEnumerator();
            while (enumerator.MoveNext())
            {
                IVertex v = enumerator.get_Current();
                IVertex vc = target.AddVertex();
                this.OnCloneVertex(v, vc);
                dictionary.set_Item(v, vc);
            }
            IEdgeEnumerator enumerator2 = source.get_Edges().GetEnumerator();
            while (enumerator2.MoveNext())
            {
                IEdge e = enumerator2.get_Current();
                IEdge ec = target.AddEdge(dictionary.get_Item(e.get_Target()), dictionary.get_Item(e.get_Source()));
                this.OnCloneEdge(e, ec);
            }
        }
    }
}

