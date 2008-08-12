namespace QuickGraph.Algorithms.Layout
{
    using QuickGraph.Concepts;
    using System;
    using System.Drawing;

    public class EdgeRenderer : IEdgeRenderer
    {
        private Pen pen = Pens.LightGray;

        public virtual void Render(Graphics g, IEdge e, PointF sourcePosition, PointF targetPosition)
        {
            g.DrawLine(this.pen, sourcePosition, targetPosition);
        }

        public Color StrokeColor
        {
            get
            {
                return this.pen.Color;
            }
            set
            {
                this.pen = new Pen(value);
            }
        }
    }
}

