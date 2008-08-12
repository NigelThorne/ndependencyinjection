namespace QuickGraph.Algorithms.Layout
{
    using QuickGraph.Concepts;
    using System;
    using System.Drawing;

    public class VertexRenderer : IVertexRenderer
    {
        private SolidBrush brush = new SolidBrush(Color.Black);
        private float radius = 4f;

        public virtual void PreRender()
        {
        }

        public virtual void Render(Graphics g, IVertex u, PointF p)
        {
            g.FillEllipse(this.brush, (float) (p.X - this.radius), (float) (p.Y - this.radius), (float) (2f * this.radius), (float) (2f * this.radius));
        }

        public Color FillColor
        {
            get
            {
                return this.brush.Color;
            }
            set
            {
                this.brush = new SolidBrush(value);
            }
        }

        public float Radius
        {
            get
            {
                return this.radius;
            }
            set
            {
                this.radius = value;
            }
        }
    }
}

