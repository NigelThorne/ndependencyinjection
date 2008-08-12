namespace QuickGraph.Algorithms.RandomWalks
{
    using QuickGraph.Collections;
    using QuickGraph.Concepts;
    using QuickGraph.Concepts.Collections;
    using QuickGraph.Concepts.Traversals;
    using System;

    public class WeightedMarkovEdgeChain : IMarkovEdgeChain
    {
        private Random rnd = new Random((int) DateTime.Now.Ticks);
        private EdgeDoubleDictionary weights = null;

        public WeightedMarkovEdgeChain(EdgeDoubleDictionary weights)
        {
            if (weights == null)
            {
                throw new ArgumentNullException("weights");
            }
            this.weights = weights;
        }

        public virtual IEdge Successor(IImplicitGraph g, IVertex u)
        {
            if (g == null)
            {
                throw new ArgumentNullException("g");
            }
            if (u == null)
            {
                throw new ArgumentNullException("u");
            }
            int num = g.OutDegree(u);
            double num2 = 0.0;
            IEdgeEnumerator enumerator = g.OutEdges(u).GetEnumerator();
            while (enumerator.MoveNext())
            {
                IEdge edge = enumerator.get_Current();
                num2 += this.weights.get_Item(edge);
            }
            double num3 = this.rnd.NextDouble() * num2;
            double num4 = 0.0;
            double num5 = 0.0;
            IEdgeEnumerator enumerator2 = g.OutEdges(u).GetEnumerator();
            while (enumerator2.MoveNext())
            {
                IEdge edge2 = enumerator2.get_Current();
                num5 = num4 + this.weights.get_Item(edge2);
                if ((num3 >= num4) && (num3 <= num5))
                {
                    return edge2;
                }
                num4 = num5;
            }
            return null;
        }

        public Random Rnd
        {
            get
            {
                return this.rnd;
            }
            set
            {
                this.rnd = value;
            }
        }

        public EdgeDoubleDictionary Weights
        {
            get
            {
                return this.weights;
            }
        }
    }
}

