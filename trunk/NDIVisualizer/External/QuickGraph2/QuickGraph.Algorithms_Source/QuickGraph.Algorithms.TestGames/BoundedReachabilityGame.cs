namespace QuickGraph.Algorithms.TestGames
{
    using QuickGraph.Collections;
    using QuickGraph.Concepts;
    using QuickGraph.Concepts.Collections;
    using System;
    using System.Runtime.CompilerServices;

    public class BoundedReachabilityGame
    {
        private VertexCollection goals = new VertexCollection();
        private IBoundedReachabilityGamePlayer implementationUnderTest = null;
        private int moveCount = 0;
        private IVertex root = null;
        private IBoundedReachabilityGamePlayer tester = null;
        private ITestGraph testGraph = null;

        public event EdgeEventHandler ChosenEdge;

        public event EventHandler GameLost;

        public event EventHandler GameWon;

        protected void OnChosenEdge(IEdge e)
        {
            if (this.ChosenEdge != null)
            {
                this.ChosenEdge.Invoke(this, new EdgeEventArgs(e));
            }
        }

        protected void OnGameLost()
        {
            if (this.GameLost != null)
            {
                this.GameLost(this, new EventArgs());
            }
        }

        protected void OnGameWon()
        {
            if (this.GameWon != null)
            {
                this.GameWon(this, new EventArgs());
            }
        }

        public void Play()
        {
            IVertex root = this.root;
            for (int i = 0; i < this.moveCount; i++)
            {
                root = this.PlayOnce(root, i);
                if (root == null)
                {
                    break;
                }
            }
        }

        public IVertex PlayOnce(IVertex v, int k)
        {
            if (this.testGraph.ContainsChoicePoint(v))
            {
                if (k >= this.moveCount)
                {
                    this.OnGameLost();
                    return null;
                }
                IEdge edge = this.ImplementationUnderTest.ChooseEdge(v, k);
                if (edge == null)
                {
                    this.OnGameLost();
                    return null;
                }
                this.OnChosenEdge(edge);
                v = edge.get_Target();
                k++;
                return v;
            }
            if (this.goals.Contains(v))
            {
                this.OnGameWon();
                return null;
            }
            if (k >= this.moveCount)
            {
                this.OnGameLost();
                return null;
            }
            IEdge e = this.tester.ChooseEdge(v, k);
            if (e == null)
            {
                this.OnGameLost();
                return null;
            }
            this.OnChosenEdge(e);
            v = e.get_Target();
            k++;
            return v;
        }

        public IVertexEnumerable Goals
        {
            get
            {
                return this.goals;
            }
        }

        public IBoundedReachabilityGamePlayer ImplementationUnderTest
        {
            get
            {
                return this.implementationUnderTest;
            }
        }

        public int MoveCount
        {
            get
            {
                return this.moveCount;
            }
        }

        public IVertex Root
        {
            get
            {
                return this.root;
            }
        }

        public IBoundedReachabilityGamePlayer Tester
        {
            get
            {
                return this.tester;
            }
        }

        public ITestGraph TestGraph
        {
            get
            {
                return this.testGraph;
            }
        }
    }
}

