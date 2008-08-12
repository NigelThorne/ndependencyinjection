namespace QuickGraph.Algorithms.Layout
{
    using System;
    using System.Drawing;

    public sealed class PointMath
    {
        private PointMath()
        {
        }

        public static PointF Add(PointF p1, PointF p2)
        {
            return new PointF(p1.X + p2.X, p1.Y + p2.Y);
        }

        public static PointF Add(PointF p1, float x, float y)
        {
            return new PointF(p1.X + x, p1.Y + y);
        }

        public static PointF Combili(PointF p0, double m, PointF p)
        {
            return new PointF(p0.X + (((float) m) * p.X), p0.Y + (((float) m) * p.Y));
        }

        public static PointF Combili(PointF p0, float m, PointF p)
        {
            return new PointF(p0.X + (m * p.X), p0.Y + (m * p.Y));
        }

        public static PointF Combili(float a, PointF p0, float b, PointF p)
        {
            return new PointF((a * p0.X) + (b * p.X), (a * p0.Y) + (b * p.Y));
        }

        public static double Distance(PointF p1, PointF p2)
        {
            return Math.Sqrt(SqrDistance(p1, p2));
        }

        public static PointF Mul(float m, PointF p)
        {
            return new PointF(m * p.X, m * p.Y);
        }

        public static PointF ScaleSaturate(PointF o, PointF p, float scale, float saturation)
        {
            float num = p.X * scale;
            num = Math.Max(-saturation, Math.Min(saturation, num));
            float num2 = p.Y * scale;
            num2 = Math.Max(-saturation, Math.Min(saturation, num2));
            return new PointF(o.X + num, o.Y + num2);
        }

        public static double SqrDistance(PointF p1, PointF p2)
        {
            return (double) (((p1.X - p2.X) * (p1.X - p2.X)) + ((p1.Y - p2.Y) * (p1.Y - p2.Y)));
        }

        public static PointF Sub(PointF p1, PointF p2)
        {
            return new PointF(p1.X - p2.X, p1.Y - p2.Y);
        }
    }
}

