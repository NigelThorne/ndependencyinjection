namespace QuickGraph.Algorithms
{
    using QuickGraph.Algorithms.Search;
    using QuickGraph.Collections;
    using QuickGraph.Concepts;
    using QuickGraph.Concepts.Traversals;
    using System;

    public class ConnectedComponentsAlgorithm
    {
        private VertexIntDictionary components;
        private int count;
        private IVertexListGraph visitedGraph;

        public ConnectedComponentsAlgorithm(IVertexListGraph g)
        {
            if (g == null)
            {
                throw new ArgumentNullException("g");
            }
            this.visitedGraph = g;
            this.components = new VertexIntDictionary();
        }

        public ConnectedComponentsAlgorithm(IVertexListGraph g, VertexIntDictionary components)
        {
            if (g == null)
            {
                throw new ArgumentNullException("g");
            }
            if (components == null)
            {
                throw new ArgumentNullException("components");
            }
            this.visitedGraph = g;
            this.components = components;
        }

        public int Compute()
        {
            this.count = -1;
            this.components.Clear();
            if (this.VisitedGraph.get_VerticesEmpty())
            {
                return ++this.count;
            }
            return this.Compute(Traversal.FirstVertex(this.VisitedGraph));
        }

        public int Compute(IVertex startVertex)
        {
            if (startVertex == null)
            {
                throw new ArgumentNullException("startVertex");
            }
            this.count = -1;
            this.components.Clear();
            DepthFirstSearchAlgorithm algorithm = new DepthFirstSearchAlgorithm(this.VisitedGraph);
            algorithm.StartVertex += new VertexEventHandler(this, (IntPtr) this.StartVertex);
            algorithm.DiscoverVertex += new VertexEventHandler(this, (IntPtr) this.DiscoverVertex);
            algorithm.Compute(startVertex);
            return ++this.count;
        }

        private void DiscoverVertex(object sender, VertexEventArgs args)
        {
            this.Components.set_Item(args.get_Vertex(), this.count);
        }

        private void StartVertex(object sender, VertexEventArgs args)
        {
            this.count++;
        }

        public VertexIntDictionary Components
        {
            get
            {
                return this.components;
            }
        }

        public int Count
        {
            get
            {
                return this.count;
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

