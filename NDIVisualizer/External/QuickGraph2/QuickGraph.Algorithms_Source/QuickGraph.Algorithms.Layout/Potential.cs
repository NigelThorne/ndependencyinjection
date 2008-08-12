namespace QuickGraph.Algorithms.Layout
{
    using QuickGraph.Concepts.Collections;
    using System;

    public abstract class Potential : IPotential
    {
        private IIteratedLayoutAlgorithm algorithm = null;

        public Potential(IIteratedLayoutAlgorithm algorithm)
        {
            if (algorithm == null)
            {
                throw new ArgumentNullException("algorithm");
            }
            this.algorithm = algorithm;
        }

        public abstract void Compute(IVertexPointFDictionary potentials);

        public IIteratedLayoutAlgorithm Algorithm
        {
            get
            {
                return this.algorithm;
            }
            set
            {
                this.algorithm = value;
            }
        }
    }
}

