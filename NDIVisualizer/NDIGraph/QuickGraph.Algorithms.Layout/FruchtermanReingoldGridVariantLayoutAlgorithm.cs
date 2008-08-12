namespace QuickGraph.Algorithms.Layout
{
    using QuickGraph.Concepts.Traversals;
    using System;
    using System.Drawing;

    public class FruchtermanReingoldGridVariantLayoutAlgorithm : FruchtermanReingoldLayoutAlgorithm
    {
        public FruchtermanReingoldGridVariantLayoutAlgorithm(IVertexAndEdgeListGraph graph, SizeF size) : base(graph, size)
        {
        }

        protected override double RepulsiveForce(double distance)
        {
            if (distance < (2.0 * base.k))
            {
                return base.RepulsiveForce(distance);
            }
            return 0.0;
        }
    }
}

