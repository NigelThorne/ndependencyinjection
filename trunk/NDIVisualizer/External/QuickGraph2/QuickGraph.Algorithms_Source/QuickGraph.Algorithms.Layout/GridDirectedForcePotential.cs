namespace QuickGraph.Algorithms.Layout
{
    using System;

    public class GridDirectedForcePotential : DirectedForcePotential
    {
        public GridDirectedForcePotential(IIteratedLayoutAlgorithm algorithm) : base(algorithm)
        {
        }

        protected override double RepulsionForce(double distance)
        {
            if (distance < (2f * base.Algorithm.EdgeLength))
            {
                return base.RepulsionForce(distance);
            }
            return 0.0;
        }
    }
}

