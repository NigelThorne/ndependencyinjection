namespace QuickGraph.Algorithms.Layout
{
    using QuickGraph.Concepts;
    using System;
    using System.Drawing;

    public interface IEdgeRenderer
    {
        void Render(Graphics g, IEdge e, PointF sourcePosition, PointF targetPosition);
    }
}

