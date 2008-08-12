namespace QuickGraph.Algorithms.RandomWalks
{
    using QuickGraph.Collections;
    using QuickGraph.Concepts;
    using QuickGraph.Concepts.Algorithms;
    using QuickGraph.Concepts.Collections;
    using QuickGraph.Concepts.Traversals;
    using System;
    using System.Runtime.CompilerServices;

    public class CyclePoppingRandomTreeAlgorithm : IAlgorithm
    {
        private VertexColorDictionary colors;
        private IMarkovEdgeChain edgeChain;
        private Random rnd;
        private VertexEdgeDictionary successors;
        private IVertexListGraph visitedGraph;

        public event VertexEventHandler ClearTreeVertex;

        public event VertexEventHandler FinishVertex;

        public event VertexEventHandler InitializeVertex;

        public event EdgeEventHandler TreeEdge;

        public CyclePoppingRandomTreeAlgorithm(IVertexListGraph g)
        {
            this.visitedGraph = null;
            this.colors = new VertexColorDictionary();
            this.edgeChain = new NormalizedMarkovEdgeChain();
            this.successors = new VertexEdgeDictionary();
            this.rnd = new Random((int) DateTime.Now.Ticks);
            if (g == null)
            {
                throw new ArgumentNullException("g");
            }
            this.visitedGraph = g;
        }

        public CyclePoppingRandomTreeAlgorithm(IVertexListGraph g, IMarkovEdgeChain edgeChain)
        {
            this.visitedGraph = null;
            this.colors = new VertexColorDictionary();
            this.edgeChain = new NormalizedMarkovEdgeChain();
            this.successors = new VertexEdgeDictionary();
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

        protected bool Attempt(double eps)
        {
            this.Initialize();
            int num = 0;
            IVertex u = null;
            IVertexEnumerator enumerator = this.VisitedGraph.get_Vertices().GetEnumerator();
            while (enumerator.MoveNext())
            {
                IVertex vertex2 = enumerator.get_Current();
                u = vertex2;
                while ((u != null) && this.NotInTree(u))
                {
                    if (this.Chance(eps))
                    {
                        this.ClearTree(u);
                        this.SetInTree(u);
                        num++;
                        if (num > 1)
                        {
                            return false;
                        }
                    }
                    else
                    {
                        this.Tree(u, this.RandomSuccessor(u));
                        u = this.NextInTree(u);
                    }
                }
                for (u = vertex2; (u != null) && this.NotInTree(u); u = this.NextInTree(u))
                {
                    this.SetInTree(u);
                }
            }
            return true;
        }

        protected bool Chance(double eps)
        {
            return (this.rnd.NextDouble() <= eps);
        }

        protected void ClearTree(IVertex u)
        {
            this.successors.set_Item(u, null);
            this.OnClearTreeVertex(u);
        }

        protected void Initialize()
        {
            this.successors.Clear();
            this.colors.Clear();
            IVertexEnumerator enumerator = this.VisitedGraph.get_Vertices().GetEnumerator();
            while (enumerator.MoveNext())
            {
                IVertex v = enumerator.get_Current();
                this.colors.set_Item(v, 0);
                this.OnInitializeVertex(v);
            }
        }

        protected IVertex NextInTree(IVertex u)
        {
            IEdge edge = this.successors.get_Item(u);
            if (edge == null)
            {
                return null;
            }
            return edge.get_Target();
        }

        protected bool NotInTree(IVertex u)
        {
            return (this.colors.get_Item(u) == 0);
        }

        protected void OnClearTreeVertex(IVertex v)
        {
            if (this.ClearTreeVertex != null)
            {
                this.ClearTreeVertex.Invoke(this, new VertexEventArgs(v));
            }
        }

        protected void OnFinishVertex(IVertex v)
        {
            if (this.FinishVertex != null)
            {
                this.FinishVertex.Invoke(this, new VertexEventArgs(v));
            }
        }

        protected void OnInitializeVertex(IVertex v)
        {
            if (this.InitializeVertex != null)
            {
                this.InitializeVertex.Invoke(this, new VertexEventArgs(v));
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

        public void RandomTree()
        {
            double eps = 1.0;
            do
            {
                eps /= 2.0;
            }
            while (!this.Attempt(eps));
        }

        public void RandomTreeWithRoot(IVertex root)
        {
            if (root == null)
            {
                throw new ArgumentNullException("root");
            }
            this.Initialize();
            this.ClearTree(root);
            this.SetInTree(root);
            IVertex u = null;
            IVertexEnumerator enumerator = this.VisitedGraph.get_Vertices().GetEnumerator();
            while (enumerator.MoveNext())
            {
                IVertex vertex2 = enumerator.get_Current();
                u = vertex2;
                while ((u != null) && this.NotInTree(u))
                {
                    this.Tree(u, this.RandomSuccessor(u));
                    u = this.NextInTree(u);
                }
                for (u = vertex2; (u != null) && this.NotInTree(u); u = this.NextInTree(u))
                {
                    this.SetInTree(u);
                }
            }
        }

        protected void SetInTree(IVertex u)
        {
            this.colors.set_Item(u, 1);
            this.OnFinishVertex(u);
        }

        protected void Tree(IVertex u, IEdge next)
        {
            if (next != null)
            {
                this.successors.set_Item(u, next);
                this.OnTreeEdge(next);
            }
        }

        public VertexColorDictionary Colors
        {
            get
            {
                return this.colors;
            }
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

        public VertexEdgeDictionary Successors
        {
            get
            {
                return this.successors;
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

