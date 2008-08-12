namespace QuickGraph.Algorithms.MaximumFlow
{
    using QuickGraph.Collections;
    using QuickGraph.Concepts;
    using QuickGraph.Concepts.Traversals;
    using System;

    public abstract class MaximumFlowAlgorithm
    {
        private EdgeDoubleDictionary capacities;
        private VertexColorDictionary colors;
        private VertexEdgeDictionary predecessors;
        private EdgeDoubleDictionary residualCapacities;
        private EdgeEdgeDictionary reversedEdges;
        private IVertexListGraph visitedGraph;

        public MaximumFlowAlgorithm(IVertexListGraph g, EdgeDoubleDictionary capacities, EdgeEdgeDictionary reversedEdges)
        {
            if (g == null)
            {
                throw new ArgumentNullException("g");
            }
            if (capacities == null)
            {
                throw new ArgumentNullException("capacities");
            }
            if (reversedEdges == null)
            {
                throw new ArgumentNullException("reversedEdges");
            }
            this.visitedGraph = g;
            this.capacities = capacities;
            this.reversedEdges = reversedEdges;
            this.predecessors = new VertexEdgeDictionary();
            this.residualCapacities = new EdgeDoubleDictionary();
            this.colors = new VertexColorDictionary();
        }

        public abstract double Compute(IVertex src, IVertex sink);

        public EdgeDoubleDictionary Capacities
        {
            get
            {
                return this.capacities;
            }
        }

        public VertexColorDictionary Colors
        {
            get
            {
                return this.colors;
            }
        }

        public VertexEdgeDictionary Predecessors
        {
            get
            {
                return this.predecessors;
            }
        }

        public EdgeDoubleDictionary ResidualCapacities
        {
            get
            {
                return this.residualCapacities;
            }
        }

        public EdgeEdgeDictionary ReversedEdges
        {
            get
            {
                return this.reversedEdges;
            }
        }

        public IVertexListGraph VisitedGraph
        {
            get
            {
                return this.visitedGraph;
            }
        }
    }
}

