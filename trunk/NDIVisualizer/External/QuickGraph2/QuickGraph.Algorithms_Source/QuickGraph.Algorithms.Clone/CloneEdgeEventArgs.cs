namespace QuickGraph.Algorithms.Clone
{
    using QuickGraph.Concepts;
    using System;

    public class CloneEdgeEventArgs : EventArgs
    {
        private IEdge clone;
        private IEdge original;

        public CloneEdgeEventArgs(IEdge original, IEdge clone)
        {
            if (original == null)
            {
                throw new ArgumentNullException("original");
            }
            if (clone == null)
            {
                throw new ArgumentNullException("clone");
            }
            this.original = original;
            this.clone = clone;
        }

        public IEdge Clone
        {
            get
            {
                return this.clone;
            }
        }

        public IEdge Original
        {
            get
            {
                return this.original;
            }
        }
    }
}

