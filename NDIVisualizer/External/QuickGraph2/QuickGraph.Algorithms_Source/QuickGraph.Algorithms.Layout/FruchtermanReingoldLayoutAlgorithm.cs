namespace QuickGraph.Algorithms.Layout
{
    using QuickGraph.Collections;
    using QuickGraph.Concepts;
    using QuickGraph.Concepts.Collections;
    using QuickGraph.Concepts.Traversals;
    using System;
    using System.Drawing;

    [Obsolete("Use ForceDirectedLayoutAlgorithm instead")]
    public class FruchtermanReingoldLayoutAlgorithm
    {
        private double c = 1.0;
        private VertexVector2DDictionary disp;
        private IVertexAndEdgeListGraph graph;
        protected double k;
        private VertexPointFDictionary pos;
        private SizeF size;
        private double temperature;

        public FruchtermanReingoldLayoutAlgorithm(IVertexAndEdgeListGraph graph, SizeF size)
        {
            this.size = size;
            this.graph = graph;
            this.pos = new VertexPointFDictionary();
            this.disp = new VertexVector2DDictionary();
        }

        protected virtual double AttractiveForce(double distance)
        {
            return ((distance * distance) / this.k);
        }

        protected void CalculateAttractiveForces()
        {
            IEdgeEnumerator enumerator = this.graph.get_Edges().GetEnumerator();
            while (enumerator.MoveNext())
            {
                IEdge edge = enumerator.get_Current();
                Vector2D vectord = (Vector2D) (this.pos.get_Item(edge.get_Target()) - this.pos.get_Item(edge.get_Source()));
                if (vectord.Norm() > 0.0)
                {
                    VertexVector2DDictionary dictionary;
                    IVertex vertex;
                    VertexVector2DDictionary dictionary2;
                    IVertex vertex2;
                    (dictionary = this.disp).set_Item(vertex = edge.get_Target(), dictionary.get_Item(vertex) - ((Vector2D) ((vectord / vectord.Norm()) * this.AttractiveForce(vectord.Norm()))));
                    (dictionary2 = this.disp).set_Item(vertex2 = edge.get_Source(), dictionary2.get_Item(vertex2) + ((Vector2D) ((vectord / vectord.Norm()) * this.AttractiveForce(vectord.Norm()))));
                }
            }
        }

        protected void CalculateRepulsiveForces()
        {
            IVertexEnumerator enumerator = this.graph.get_Vertices().GetEnumerator();
            while (enumerator.MoveNext())
            {
                IVertex vertex = enumerator.get_Current();
                Vector2D vectord2 = new Vector2D();
                this.disp.set_Item(vertex, vectord2);
                IVertexEnumerator enumerator2 = this.graph.get_Vertices().GetEnumerator();
                while (enumerator2.MoveNext())
                {
                    IVertex vertex2 = enumerator2.get_Current();
                    if (vertex2 != vertex)
                    {
                        VertexVector2DDictionary dictionary;
                        IVertex vertex3;
                        Vector2D vectord = (Vector2D) (this.pos.get_Item(vertex) - this.pos.get_Item(vertex2));
                        while (vectord.Norm() == 0.0)
                        {
                            Random random = new Random();
                            vectord = new Vector2D((random.NextDouble() * 2.0) - 1.0, (random.NextDouble() * 2.0) - 1.0);
                        }
                        (dictionary = this.disp).set_Item(vertex3 = vertex, dictionary.get_Item(vertex3) + ((Vector2D) ((vectord / vectord.Norm()) * this.RepulsiveForce(vectord.Norm()))));
                    }
                }
            }
        }

        public void Compute()
        {
            this.Initialize();
            for (int i = 0; i < 50; i++)
            {
                this.Iterate();
            }
        }

        protected void DisplaceVertices()
        {
            IVertexEnumerator enumerator = this.graph.get_Vertices().GetEnumerator();
            while (enumerator.MoveNext())
            {
                IVertex vertex = enumerator.get_Current();
                PointF tf = this.pos.get_Item(vertex);
                if (this.disp.get_Item(vertex).Norm() > 0.0)
                {
                    tf += (PointF) ((this.disp.get_Item(vertex) / this.disp.get_Item(vertex).Norm()) * Math.Min(this.disp.get_Item(vertex).Norm(), this.temperature));
                }
                tf.X = Math.Min(this.size.Width, Math.Max(0f, tf.X));
                tf.Y = Math.Min(this.size.Height, Math.Max(0f, tf.Y));
                this.pos.set_Item(vertex, tf);
            }
        }

        public void Initialize()
        {
            this.k = this.C * Math.Sqrt((double) ((this.size.Width * this.size.Height) / ((float) this.graph.get_VerticesCount())));
            Random random = new Random();
            IVertexEnumerator enumerator = this.graph.get_Vertices().GetEnumerator();
            while (enumerator.MoveNext())
            {
                IVertex vertex = enumerator.get_Current();
                this.pos.set_Item(vertex, new PointF(((float) random.NextDouble()) * this.size.Width, ((float) random.NextDouble()) * this.size.Height));
            }
            this.temperature = this.size.Width * 0.1;
        }

        public void Iterate()
        {
            this.CalculateRepulsiveForces();
            this.CalculateAttractiveForces();
            this.DisplaceVertices();
            this.ReduceTemperature();
        }

        protected void ReduceTemperature()
        {
            this.temperature *= 0.991;
        }

        protected virtual double RepulsiveForce(double distance)
        {
            return ((this.k * this.k) / distance);
        }

        public double C
        {
            get
            {
                return this.c;
            }
            set
            {
                this.c = value;
            }
        }

        public VertexPointFDictionary Positions
        {
            get
            {
                return this.pos;
            }
        }

        public IVertexAndEdgeListGraph VisitedGraph
        {
            get
            {
                return this.graph;
            }
        }
    }
}

