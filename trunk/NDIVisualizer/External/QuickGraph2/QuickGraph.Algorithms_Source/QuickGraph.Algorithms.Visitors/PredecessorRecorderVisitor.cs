namespace QuickGraph.Algorithms.Visitors
{
    using QuickGraph.Collections;
    using QuickGraph.Concepts;
    using QuickGraph.Concepts.Visitors;
    using System;
    using System.Collections;

    public class PredecessorRecorderVisitor : IPredecessorRecorderVisitor
    {
        private VertexCollection endPathVertices;
        private VertexEdgeDictionary predecessors;

        public PredecessorRecorderVisitor()
        {
            this.predecessors = new VertexEdgeDictionary();
            this.endPathVertices = new VertexCollection();
        }

        public PredecessorRecorderVisitor(VertexEdgeDictionary predecessors)
        {
            if (predecessors == null)
            {
                throw new ArgumentNullException("predecessors");
            }
            this.predecessors = predecessors;
            this.endPathVertices = new VertexCollection();
        }

        public EdgeCollectionCollection AllPaths()
        {
            EdgeCollectionCollection collections = new EdgeCollectionCollection();
            VertexCollection.Enumerator enumerator = this.EndPathVertices.GetEnumerator();
            while (enumerator.MoveNext())
            {
                IVertex v = enumerator.get_Current();
                collections.Add(this.Path(v));
            }
            return collections;
        }

        public void FinishVertex(object sender, VertexEventArgs args)
        {
            IDictionaryEnumerator enumerator = this.Predecessors.GetEnumerator();
            while (enumerator.MoveNext())
            {
                DictionaryEntry current = (DictionaryEntry) enumerator.Current;
                IEdge edge = (IEdge) current.Value;
                if (edge.get_Source() == args.get_Vertex())
                {
                    return;
                }
            }
            this.EndPathVertices.Add(args.get_Vertex());
        }

        public EdgeCollection Path(IVertex v)
        {
            EdgeCollection edges = new EdgeCollection();
            IVertex vertex = v;
            while (this.Predecessors.Contains(v))
            {
                IEdge edge = this.Predecessors.get_Item(v);
                edges.Insert(0, edge);
                v = edge.get_Source();
            }
            return edges;
        }

        public void TreeEdge(object sender, EdgeEventArgs args)
        {
            this.Predecessors.set_Item(args.get_Edge().get_Target(), args.get_Edge());
        }

        public VertexCollection EndPathVertices
        {
            get
            {
                return this.endPathVertices;
            }
        }

        public VertexEdgeDictionary Predecessors
        {
            get
            {
                return this.predecessors;
            }
        }
    }
}

