namespace QuickGraph.Algorithms.Layout
{
    using System;
    using System.Drawing;

    public sealed class ForceDirectedLayoutAlgorithm<Vertex, Edge, Graph> : LayoutAlgorithmBase<Vertex, Edge, Graph> where Edge: IEdge<Vertex> where Graph: IVertexAndEdgeListGraph<Vertex, Edge>
    {
        private float gravitationFactor;
        private PointF magneticDirection;
        private float springFactor;

        public ForceDirectedLayoutAlgorithm(Graph visitedGraph) : base(visitedGraph)
        {
            this.springFactor = 0.2f;
            this.magneticDirection = new PointF(1f, 0f);
            this.gravitationFactor = 0.1f;
        }

        protected override void InternalCompute()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public float GravitationFactor
        {
            get
            {
                return this.gravitationFactor;
            }
            set
            {
                this.gravitationFactor = value;
            }
        }

        public PointF MagneticDirection
        {
            get
            {
                return this.magneticDirection;
            }
            set
            {
                this.magneticDirection = value;
            }
        }

        public float SpringFactor
        {
            get
            {
                return this.springFactor;
            }
            set
            {
                this.springFactor = value;
            }
        }
    }
}

