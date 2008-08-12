namespace QuickGraph.Algorithms
{
    using QuickGraph.Algorithms.Search;
    using QuickGraph.Concepts;
    using QuickGraph.Concepts.Traversals;
    using QuickGraph.Exceptions;
    using System;
    using System.Collections;

    public class TopologicalSortAlgorithm
    {
        private IList m_Vertices;
        private IVertexListGraph m_VisitedGraph;

        public TopologicalSortAlgorithm(IVertexListGraph g)
        {
            if (g == null)
            {
                throw new ArgumentNullException("g");
            }
            this.m_VisitedGraph = g;
            this.m_Vertices = new ArrayList();
        }

        public TopologicalSortAlgorithm(IVertexListGraph g, IList vertices)
        {
            if (g == null)
            {
                throw new ArgumentNullException("g");
            }
            if (vertices == null)
            {
                throw new ArgumentNullException("vertices");
            }
            this.m_VisitedGraph = g;
            this.m_Vertices = vertices;
        }

        public void BackEdge(object sender, EdgeEventArgs args)
        {
            throw new NonAcyclicGraphException();
        }

        public void Compute()
        {
            this.Compute(null);
        }

        public void Compute(IList vertices)
        {
            if (vertices != null)
            {
                this.m_Vertices = vertices;
            }
            DepthFirstSearchAlgorithm algorithm = new DepthFirstSearchAlgorithm(this.VisitedGraph);
            algorithm.BackEdge += new EdgeEventHandler(this, (IntPtr) this.BackEdge);
            algorithm.FinishVertex += new VertexEventHandler(this, (IntPtr) this.FinishVertex);
            this.m_Vertices.Clear();
            algorithm.Compute();
        }

        public void FinishVertex(object sender, VertexEventArgs args)
        {
            this.m_Vertices.Insert(0, args.get_Vertex());
        }

        public IList SortedVertices
        {
            get
            {
                return this.m_Vertices;
            }
        }

        public IVertexListGraph VisitedGraph
        {
            get
            {
                return this.m_VisitedGraph;
            }
        }
    }
}

