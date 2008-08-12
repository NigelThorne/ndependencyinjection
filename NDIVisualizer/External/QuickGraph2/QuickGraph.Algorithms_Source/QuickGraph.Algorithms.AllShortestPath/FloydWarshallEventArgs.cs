namespace QuickGraph.Algorithms.AllShortestPath
{
    using QuickGraph.Concepts;
    using System;

    public class FloydWarshallEventArgs : EventArgs
    {
        private IVertex intermediate;
        private IVertex source;
        private IVertex target;

        public FloydWarshallEventArgs(IVertex source, IVertex target)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }
            if (target == null)
            {
                throw new ArgumentNullException("target");
            }
            this.source = source;
            this.target = target;
            this.intermediate = null;
        }

        public FloydWarshallEventArgs(IVertex source, IVertex target, IVertex intermediate)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }
            if (target == null)
            {
                throw new ArgumentNullException("target");
            }
            if (intermediate == null)
            {
                throw new ArgumentNullException("intermediate");
            }
            this.source = source;
            this.target = target;
            this.intermediate = intermediate;
        }

        public IVertex Intermediate
        {
            get
            {
                return this.intermediate;
            }
        }

        public IVertex Source
        {
            get
            {
                return this.source;
            }
        }

        public IVertex Target
        {
            get
            {
                return this.target;
            }
        }
    }
}

