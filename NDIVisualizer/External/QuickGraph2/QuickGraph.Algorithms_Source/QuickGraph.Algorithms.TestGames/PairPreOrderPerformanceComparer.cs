namespace QuickGraph.Algorithms.TestGames
{
    using System;

    public class PairPreOrderPerformanceComparer : IPerformanceComparer
    {
        public bool Compare(double leftProb, double leftCost, double rightProb, double rightCost)
        {
            return ((rightProb == 0.0) || ((leftProb > rightProb) || ((Math.Abs((double) (leftProb - rightProb)) < double.Epsilon) && (leftCost < rightCost))));
        }
    }
}

