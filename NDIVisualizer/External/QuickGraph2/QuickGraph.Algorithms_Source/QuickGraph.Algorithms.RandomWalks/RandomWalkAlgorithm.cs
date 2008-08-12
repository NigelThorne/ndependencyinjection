namespace QuickGraph.Algorithms.RandomWalks
{
    using QuickGraph.Concepts;
    using QuickGraph.Concepts.Algorithms;
    using QuickGraph.Concepts.Predicates;
    using QuickGraph.Concepts.Traversals;
    using System;
    using System.Runtime.CompilerServices;

    public class RandomWalkAlgorithm : IAlgorithm
    {
        private IMarkovEdgeChain edgeChain;
        private IEdgePredicate endPredicate;
        private Random rnd;
        private IImplicitGraph visitedGraph;

        public event VertexEventHandler EndVertex;

        public event VertexEventHandler StartVertex;

        public event EdgeEventHandler TreeEdge;

        public RandomWalkAlgorithm(IVertexListGraph g)
        {
            this.visitedGraph = null;
            this.endPredicate = null;
            this.edgeChain = new NormalizedMarkovEdgeChain();
            this.rnd = new Random((int) DateTime.Now.Ticks);
            if (g == null)
            {
                throw new ArgumentNullException("g");
            }
            this.visitedGraph = g;
        }

        public RandomWalkAlgorithm(IVertexListGraph g, IMarkovEdgeChain edgeChain)
        {
            this.visitedGraph = null;
            this.endPredicate = null;
            this.edgeChain = new NormalizedMarkovEdgeChain();
            this.rnd = new Random((int) DateTime.Now.Ticks);
            if (g == null)
            {
                throw new ArgumentNullException("g");
            }
            if (edgeChain == null)
            {
                throw new ArgumentNullException("edgeChain");
            }
            this.visitedGraph = g;
            this.edgeChain = edgeChain;
        }

        public void Generate(IVertex root)
        {
            this.Generate(root, 0x7fffffff);
        }

        public void Generate(IVertex root, int walkCount)
        {
            int num = 0;
            IEdge e = null;
            IVertex u = root;
            this.OnStartVertex(root);
            while (num < walkCount)
            {
                e = this.RandomSuccessor(u);
                if ((e == null) || ((this.endPredicate != null) && this.endPredicate.Test(e)))
                {
                    break;
                }
                this.OnTreeEdge(e);
                u = e.get_Target();
                num++;
            }
            this.OnEndVertex(u);
        }

        protected void OnEndVertex(IVertex v)
        {
            if (this.EndVertex != null)
            {
                this.EndVertex.Invoke(this, new VertexEventArgs(v));
            }
        }

        protected void OnStartVertex(IVertex v)
        {
            if (this.StartVertex != null)
            {
                this.StartVertex.Invoke(this, new VertexEventArgs(v));
            }
        }

        protected void OnTreeEdge(IEdge e)
        {
            if (this.TreeEdge != null)
            {
                this.TreeEdge.Invoke(this, new EdgeEventArgs(e));
            }
        }

        object IAlgorithm.get_VisitedGraph()
        {
            return this.VisitedGraph;
        }

        protected IEdge RandomSuccessor(IVertex u)
        {
            return this.EdgeChain.Successor(this.VisitedGraph, u);
        }

        public IMarkovEdgeChain EdgeChain
        {
            get
            {
                return this.edgeChain;
            }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("edgeChain");
                }
                this.edgeChain = value;
            }
        }

        public IEdgePredicate EndPredicate
        {
            get
            {
                return this.endPredicate;
            }
            set
            {
                this.endPredicate = value;
            }
        }

        public Random Rnd
        {
            get
            {
                return this.rnd;
            }
            set
            {
                this.rnd = value;
            }
        }

        public IImplicitGraph VisitedGraph
        {
            get
            {
                return this.visitedGraph;
            }
        }
    }
}

