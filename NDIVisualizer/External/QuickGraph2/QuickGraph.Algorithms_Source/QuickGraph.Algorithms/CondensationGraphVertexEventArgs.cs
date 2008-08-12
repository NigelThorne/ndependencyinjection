namespace QuickGraph.Algorithms
{
    using QuickGraph.Concepts;
    using QuickGraph.Concepts.Collections;
    using System;

    public class CondensationGraphVertexEventArgs : EventArgs
    {
        private IVertexCollection stronglyConnectedVertices;
        private IVertex vCG;

        public CondensationGraphVertexEventArgs(IVertex cgVertex, IVertexCollection stronglyConnectedVertices)
        {
            this.vCG = cgVertex;
            this.stronglyConnectedVertices = stronglyConnectedVertices;
        }

        public IVertex CondensationGraphVertex
        {
            get
            {
                return this.vCG;
            }
        }

        public IVertexCollection StronglyConnectedVertices
        {
            get
            {
                return this.stronglyConnectedVertices;
            }
        }
    }
}

