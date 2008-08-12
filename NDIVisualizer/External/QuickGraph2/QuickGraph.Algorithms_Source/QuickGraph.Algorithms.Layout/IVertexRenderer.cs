namespace QuickGraph.Algorithms.Layout
{
    using QuickGraph.Concepts;
    using System;
    using System.Drawing;

    public interface IVertexRenderer
    {
        void PreRender();
        void Render(Graphics g, IVertex u, PointF p);
    }
}

