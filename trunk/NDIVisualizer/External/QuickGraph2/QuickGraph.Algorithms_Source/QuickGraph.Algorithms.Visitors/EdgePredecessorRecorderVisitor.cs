namespace QuickGraph.Algorithms.Visitors
{
    using QuickGraph.Collections;
    using QuickGraph.Concepts;
    using QuickGraph.Concepts.Visitors;
    using System;
    using System.Collections;

    public class EdgePredecessorRecorderVisitor : IEdgePredecessorRecorderVisitor
    {
        private EdgeEdgeDictionary edgePredecessors;
        private EdgeCollection endPathEdges;

        public EdgePredecessorRecorderVisitor()
        {
            this.edgePredecessors = new EdgeEdgeDictionary();
            this.endPathEdges = new EdgeCollection();
        }

        public EdgePredecessorRecorderVisitor(EdgeEdgeDictionary edgePredecessors, EdgeCollection endPathEdges)
        {
            if (edgePredecessors == null)
            {
                throw new ArgumentNullException("edgePredecessors");
            }
            if (endPathEdges == null)
            {
                throw new ArgumentNullException("endPathEdges");
            }
            this.edgePredecessors = edgePredecessors;
            this.endPathEdges = endPathEdges;
        }

        public EdgeCollection[] AllMergedPaths()
        {
            EdgeCollection[] edgesArray = new EdgeCollection[this.EndPathEdges.Count];
            EdgeColorDictionary colors = new EdgeColorDictionary();
            IDictionaryEnumerator enumerator = this.EdgePredecessors.GetEnumerator();
            while (enumerator.MoveNext())
            {
                DictionaryEntry current = (DictionaryEntry) enumerator.Current;
                colors.set_Item((IEdge) current.Key, 0);
                colors.set_Item((IEdge) current.Value, 0);
            }
            for (int i = 0; i < this.EndPathEdges.Count; i++)
            {
                edgesArray[i] = this.MergedPath(this.EndPathEdges.get_Item(i), colors);
            }
            return edgesArray;
        }

        public EdgeCollectionCollection AllPaths()
        {
            EdgeCollectionCollection collections = new EdgeCollectionCollection();
            EdgeCollection.Enumerator enumerator = this.EndPathEdges.GetEnumerator();
            while (enumerator.MoveNext())
            {
                IEdge se = enumerator.get_Current();
                collections.Add(this.Path(se));
            }
            return collections;
        }

        public void DiscoverTreeEdge(object sender, EdgeEdgeEventArgs args)
        {
            if (args.get_Edge() != args.get_TargetEdge())
            {
                this.EdgePredecessors.set_Item(args.get_TargetEdge(), args.get_Edge());
            }
        }

        public void FinishEdge(object sender, EdgeEventArgs args)
        {
            if (!this.EdgePredecessors.ContainsValue(args.get_Edge()))
            {
                this.EndPathEdges.Add(args.get_Edge());
            }
        }

        public void InitializeEdge(object sender, EdgeEventArgs args)
        {
        }

        public EdgeCollection MergedPath(IEdge se, EdgeColorDictionary colors)
        {
            EdgeCollection edges = new EdgeCollection();
            IEdge edge = se;
            if (colors.get_Item(edge) == null)
            {
                colors.set_Item(edge, 1);
                edges.Insert(0, edge);
                while (this.EdgePredecessors.Contains(edge))
                {
                    IEdge edge2 = this.EdgePredecessors.get_Item(edge);
                    if (colors.get_Item(edge2) != null)
                    {
                        return edges;
                    }
                    colors.set_Item(edge2, 1);
                    edges.Insert(0, edge2);
                    edge = edge2;
                }
            }
            return edges;
        }

        public EdgeCollection Path(IEdge se)
        {
            EdgeCollection edges = new EdgeCollection();
            IEdge edge = se;
            edges.Insert(0, edge);
            while (this.EdgePredecessors.Contains(edge))
            {
                IEdge edge2 = this.EdgePredecessors.get_Item(edge);
                edges.Insert(0, edge2);
                edge = edge2;
            }
            return edges;
        }

        public EdgeEdgeDictionary EdgePredecessors
        {
            get
            {
                return this.edgePredecessors;
            }
        }

        public EdgeCollection EndPathEdges
        {
            get
            {
                return this.endPathEdges;
            }
        }
    }
}

