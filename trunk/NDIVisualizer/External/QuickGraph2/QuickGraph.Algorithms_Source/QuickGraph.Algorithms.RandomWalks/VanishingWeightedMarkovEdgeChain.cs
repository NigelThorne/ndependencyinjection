namespace QuickGraph.Algorithms.RandomWalks
{
    using QuickGraph.Collections;
    using QuickGraph.Concepts;
    using QuickGraph.Concepts.Traversals;
    using System;

    public class VanishingWeightedMarkovEdgeChain : WeightedMarkovEdgeChain
    {
        private double factor;

        public VanishingWeightedMarkovEdgeChain(EdgeDoubleDictionary weights, double factor) : base(weights)
        {
            this.factor = 0.5;
            this.factor = factor;
        }

        public override IEdge Successor(IImplicitGraph g, IVertex u)
        {
            EdgeDoubleDictionary dictionary;
            IEdge edge3;
            IEdge edge = base.Successor(g, u);
            (dictionary = base.Weights).set_Item(edge3 = edge, dictionary.get_Item(edge3) * this.Factor);
            return edge;
        }

        public double Factor
        {
            get
            {
                return this.factor;
            }
        }
    }
}

