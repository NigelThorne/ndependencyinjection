namespace QuickGraph.Algorithms.TestGames
{
    using System;

    public interface IPerformanceComparer
    {
        bool Compare(double leftProb, double leftCost, double rightProb, double rightCost);
    }
}

