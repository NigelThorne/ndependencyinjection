namespace QuickGraph.Algorithms.Clone
{
    using QuickGraph.Concepts;
    using System;

    public class CloneVertexEventArgs : EventArgs
    {
        private IVertex clone;
        private IVertex original;

        public CloneVertexEventArgs(IVertex original, IVertex clone)
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

        public IVertex Clone
        {
            get
            {
                return this.clone;
            }
        }

        public IVertex Original
        {
            get
            {
                return this.original;
            }
        }
    }
}

