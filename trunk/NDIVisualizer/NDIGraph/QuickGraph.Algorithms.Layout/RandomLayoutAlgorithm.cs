namespace QuickGraph.Algorithms.Layout
{
    using QuickGraph.Concepts;
    using QuickGraph.Concepts.Collections;
    using QuickGraph.Concepts.Traversals;
    using System;
    using System.Drawing;

    public class RandomLayoutAlgorithm : LayoutAlgorithm
    {
        private Random rnd;

        public RandomLayoutAlgorithm(IVertexAndEdgeListGraph visitedGraph) : base(visitedGraph)
        {
            this.rnd = new Random((int) DateTime.Now.Ticks);
        }

        public RandomLayoutAlgorithm(IVertexAndEdgeListGraph visitedGraph, IVertexPointFDictionary positions) : base(visitedGraph, positions)
        {
            this.rnd = new Random((int) DateTime.Now.Ticks);
        }

        public override void Compute()
        {
            this.OnPreCompute();
            IVertexEnumerator enumerator = base.VisitedGraph.get_Vertices().GetEnumerator();
            while (enumerator.MoveNext())
            {
                IVertex vertex = enumerator.get_Current();
                base.Positions.set_Item(vertex, new PointF(this.nextX(), this.nextY()));
            }
            this.OnPostCompute();
        }

        private float nextX()
        {
            return (float) (this.rnd.NextDouble() * base.LayoutSize.Width);
        }

        private float nextY()
        {
            return (float) (this.rnd.NextDouble() * base.LayoutSize.Height);
        }
    }
}

