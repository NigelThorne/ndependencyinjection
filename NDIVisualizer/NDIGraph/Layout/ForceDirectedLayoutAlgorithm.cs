namespace QuickGraph.Algorithms.Layout
{
    using System;
    using System.Drawing;

    /// <summary>
    /// set up initial node velocities to (0,0)
    /// set up initial node positions randomly // make sure no 2 nodes are in exactly the same position
    /// loop
    ///     total_kinetic_energy := 0 // running sum of total kinetic energy over all particles
    ///     for each node
    ///         net-force := (0, 0) // running sum of total force on this particular node
    ///         
    ///         for each other node
    ///             net-force := net-force + Coulomb_repulsion( this_node, other_node )
    ///         next node
    ///         
    ///         for each spring connected to this node
    ///             net-force := net-force + Hooke_attraction( this_node, spring )
    ///         next spring
    ///         
    ///         // without damping, it moves forever
    ///         this_node.velocity := (this_node.velocity + timestep * net-force) * damping
    ///         this_node.position := this_node.position + timestep * this_node.velocity
    ///         total_kinetic_energy := total_kinetic_energy + this_node.mass * (this_node.speed)^2
    ///     next node
    /// until total_kinetic_energy is less than some small number  //the simulation has stopped moving
    /// 
    /// </summary>
    /// <typeparam name="Vertex"></typeparam>
    /// <typeparam name="Edge"></typeparam>
    /// <typeparam name="Graph"></typeparam>
    public class ForceDirectedLayoutAlgorithm<Vertex, Edge, Graph> : LayoutAlgorithmBase<Vertex, Edge, Graph> where Edge: IEdge<Vertex> where Graph: IVertexAndEdgeListGraph<Vertex, Edge>
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

