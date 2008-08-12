namespace QuickGraph.Algorithms.TestGames
{
    using QuickGraph.Collections;
    using QuickGraph.Concepts;
    using System;

    public class OptimalStrategy : IStrategy, IBoundedReachabilityGamePlayer
    {
        private VertexEdgeDictionary successors;

        public OptimalStrategy()
        {
            this.successors = new VertexEdgeDictionary();
        }

        public OptimalStrategy(VertexEdgeDictionary successors)
        {
            this.successors = new VertexEdgeDictionary();
            if (successors == null)
            {
                throw new ArgumentNullException("successors");
            }
            this.successors = successors;
        }

        public IEdge ChooseEdge(IVertex state, int i)
        {
            return this.successors.get_Item(state);
        }

        public void SetChooseEdge(IVertex v, int k, IEdge e)
        {
            this.successors.set_Item(v, e);
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

