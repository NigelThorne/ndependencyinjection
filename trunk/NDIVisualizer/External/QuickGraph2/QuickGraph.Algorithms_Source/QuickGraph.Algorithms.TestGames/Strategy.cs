namespace QuickGraph.Algorithms.TestGames
{
    using QuickGraph.Collections;
    using QuickGraph.Concepts;
    using System;

    public class Strategy : IStrategy, IBoundedReachabilityGamePlayer
    {
        private VertexEdgesDictionary successors = new VertexEdgesDictionary();

        public IEdge ChooseEdge(IVertex state, int i)
        {
            return this.successors.get_Item(state).get_Item(i);
        }

        public void SetChooseEdge(IVertex v, int k, IEdge e)
        {
            if (k == 0)
            {
                this.successors.Add(v, new EdgeCollection());
            }
            EdgeCollection edges = this.successors.get_Item(v);
            if (edges.Count <= k)
            {
                edges.Add(e);
            }
            else
            {
                edges.set_Item(k, e);
            }
        }
    }
}

