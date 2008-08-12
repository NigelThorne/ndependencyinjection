namespace QuickGraph.Algorithms.RandomWalks
{
    using QuickGraph.Concepts;
    using QuickGraph.Concepts.Collections;
    using QuickGraph.Concepts.Traversals;
    using System;

    public class NormalizedMarkovEdgeChain : IMarkovEdgeChain
    {
        private Random rnd = new Random((int) DateTime.Now.Ticks);

        internal IEdge NextState(int edgeCount, IEdgeEnumerable edges)
        {
            if (edgeCount == 0)
            {
                return null;
            }
            double num = this.rnd.NextDouble();
            int num2 = (int) Math.Floor(edgeCount * num);
            if (num2 == edgeCount)
            {
                num2--;
            }
            int num3 = 0;
            IEdgeEnumerator enumerator = edges.GetEnumerator();
            while (enumerator.MoveNext())
            {
                IEdge edge = enumerator.get_Current();
                if (num3 == num2)
                {
                    return edge;
                }
                num3++;
            }
            throw new InvalidOperationException("This is a bug");
        }

        public IEdge Successor(IImplicitGraph g, IVertex u)
        {
            if (g == null)
            {
                throw new ArgumentNullException("g");
            }
            if (u == null)
            {
                throw new ArgumentNullException("u");
            }
            return this.NextState(g.OutDegree(u), g.OutEdges(u));
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
    }
}

