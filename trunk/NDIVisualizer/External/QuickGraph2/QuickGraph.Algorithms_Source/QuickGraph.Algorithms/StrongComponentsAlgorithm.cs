namespace QuickGraph.Algorithms
{
    using QuickGraph.Algorithms.Search;
    using QuickGraph.Collections;
    using QuickGraph.Concepts;
    using QuickGraph.Concepts.Collections;
    using QuickGraph.Concepts.Traversals;
    using System;
    using System.Collections;

    public class StrongComponentsAlgorithm
    {
        private VertexIntDictionary components;
        private int count;
        private int dfsTime;
        private VertexIntDictionary discoverTimes;
        private VertexVertexDictionary roots;
        private Stack stack;
        private IVertexListGraph visitedGraph;

        public StrongComponentsAlgorithm(IVertexListGraph g)
        {
            if (g == null)
            {
                throw new ArgumentNullException("g");
            }
            this.visitedGraph = g;
            this.components = new VertexIntDictionary();
            this.roots = new VertexVertexDictionary();
            this.discoverTimes = new VertexIntDictionary();
            this.stack = new Stack();
            this.count = 0;
            this.dfsTime = 0;
        }

        public StrongComponentsAlgorithm(IVertexListGraph g, VertexIntDictionary components)
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
            this.roots = new VertexVertexDictionary();
            this.discoverTimes = new VertexIntDictionary();
            this.stack = new Stack();
            this.count = 0;
            this.dfsTime = 0;
        }

        public int Compute()
        {
            this.Components.Clear();
            this.Roots.Clear();
            this.DiscoverTimes.Clear();
            this.count = 0;
            this.dfsTime = 0;
            DepthFirstSearchAlgorithm algorithm = new DepthFirstSearchAlgorithm(this.VisitedGraph);
            algorithm.DiscoverVertex += new VertexEventHandler(this, (IntPtr) this.DiscoverVertex);
            algorithm.FinishVertex += new VertexEventHandler(this, (IntPtr) this.FinishVertex);
            algorithm.Compute();
            return this.count;
        }

        private void DiscoverVertex(object sender, VertexEventArgs args)
        {
            IVertex vertex = args.get_Vertex();
            this.Roots.set_Item(vertex, vertex);
            this.Components.set_Item(vertex, 0x7fffffff);
            this.DiscoverTimes.set_Item(vertex, this.dfsTime++);
            this.stack.Push(vertex);
        }

        private void FinishVertex(object sender, VertexEventArgs args)
        {
            IVertex vertex = args.get_Vertex();
            IEdgeEnumerator enumerator = this.VisitedGraph.OutEdges(vertex).GetEnumerator();
            while (enumerator.MoveNext())
            {
                IVertex vertex2 = enumerator.get_Current().get_Target();
                if (this.Components.get_Item(vertex2) == 0x7fffffff)
                {
                    this.Roots.set_Item(vertex, this.MinDiscoverTime(this.Roots.get_Item(vertex), this.Roots.get_Item(vertex2)));
                }
            }
            if (this.Roots.get_Item(vertex) == vertex)
            {
                IVertex vertex3 = null;
                do
                {
                    vertex3 = (IVertex) this.stack.Peek();
                    this.stack.Pop();
                    this.Components.set_Item(vertex3, this.count);
                }
                while (vertex3 != vertex);
                this.count++;
            }
        }

        internal IVertex MinDiscoverTime(IVertex u, IVertex v)
        {
            if (this.DiscoverTimes.get_Item(u) < this.DiscoverTimes.get_Item(v))
            {
                return u;
            }
            return v;
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

        public VertexIntDictionary DiscoverTimes
        {
            get
            {
                return this.discoverTimes;
            }
        }

        public VertexVertexDictionary Roots
        {
            get
            {
                return this.roots;
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

