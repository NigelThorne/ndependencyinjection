namespace QuickGraph.Algorithms.Visitors
{
    using QuickGraph.Collections;
    using QuickGraph.Concepts;
    using System;

    public class EdgeWeightScalerVisitor
    {
        private double factor;
        private EdgeDoubleDictionary weights;

        public EdgeWeightScalerVisitor(EdgeDoubleDictionary weights, double factor)
        {
            if (weights == null)
            {
                throw new ArgumentNullException("weights");
            }
            this.weights = weights;
            this.factor = factor;
        }

        public void TreeEdge(object sender, EdgeEventArgs e)
        {
            this.weights.set_Item(e.get_Edge(), this.weights.get_Item(e.get_Edge()) * this.factor);
        }

        public double Factor
        {
            get
            {
                return this.factor;
            }
            set
            {
                this.factor = value;
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

