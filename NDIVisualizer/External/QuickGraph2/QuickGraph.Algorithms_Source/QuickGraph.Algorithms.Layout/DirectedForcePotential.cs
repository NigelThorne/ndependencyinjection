namespace QuickGraph.Algorithms.Layout
{
    using QuickGraph.Concepts;
    using QuickGraph.Concepts.Collections;
    using System;
    using System.Drawing;

    public class DirectedForcePotential : Potential
    {
        public DirectedForcePotential(IIteratedLayoutAlgorithm algorithm) : base(algorithm)
        {
        }

        protected virtual double AttractionForce(double distance)
        {
            return ((distance * distance) / ((double) base.Algorithm.EdgeLength));
        }

        public override void Compute(IVertexPointFDictionary potentials)
        {
            int num = 0;
            IVertexEnumerator enumerator = base.Algorithm.VisitedGraph.get_Vertices().GetEnumerator();
            while (enumerator.MoveNext())
            {
                IVertex vertex = enumerator.get_Current();
                PointF tf = base.Algorithm.Positions.get_Item(vertex);
                int num2 = 0;
                IVertexEnumerator enumerator2 = base.Algorithm.VisitedGraph.get_Vertices().GetEnumerator();
                while (enumerator2.MoveNext())
                {
                    IVertex vertex2 = enumerator2.get_Current();
                    if (num2 <= num)
                    {
                        num2++;
                    }
                    else
                    {
                        PointF tf2 = base.Algorithm.Positions.get_Item(vertex2);
                        PointF p = PointMath.Sub(tf2, tf);
                        double distance = PointMath.Distance(tf, tf2);
                        double num4 = this.RepulsionForce(distance);
                        potentials.set_Item(vertex, PointMath.Combili(potentials.get_Item(vertex), -(num4 / distance), p));
                        potentials.set_Item(vertex2, PointMath.Combili(potentials.get_Item(vertex2), (double) (num4 / distance), p));
                        if (base.Algorithm.VisitedGraph.ContainsEdge(vertex, vertex2) || base.Algorithm.VisitedGraph.ContainsEdge(vertex2, vertex))
                        {
                            double num5 = this.AttractionForce(distance);
                            potentials.set_Item(vertex, PointMath.Combili(potentials.get_Item(vertex), (double) (num5 / distance), p));
                            potentials.set_Item(vertex2, PointMath.Combili(potentials.get_Item(vertex2), -(num5 / distance), p));
                        }
                        num2++;
                    }
                }
                num++;
            }
        }

        protected virtual double RepulsionForce(double distance)
        {
            return (((double) (base.Algorithm.EdgeLength * base.Algorithm.EdgeLength)) / distance);
        }
    }
}

