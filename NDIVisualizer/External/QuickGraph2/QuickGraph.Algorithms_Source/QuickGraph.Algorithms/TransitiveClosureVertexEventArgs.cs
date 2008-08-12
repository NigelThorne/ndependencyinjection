namespace QuickGraph.Algorithms
{
    using QuickGraph.Concepts;
    using System;

    public class TransitiveClosureVertexEventArgs : EventArgs
    {
        private IVertex vOriginal;
        private IVertex vTransform;

        public TransitiveClosureVertexEventArgs(IVertex original_vertex, IVertex transform_vertex)
        {
            this.vOriginal = original_vertex;
            this.vTransform = transform_vertex;
        }

        public IVertex VertexInOriginalGraph
        {
            get
            {
                return this.vOriginal;
            }
        }

        public IVertex VertexInTransformationGraph
        {
            get
            {
                return this.vTransform;
            }
        }
    }
}

