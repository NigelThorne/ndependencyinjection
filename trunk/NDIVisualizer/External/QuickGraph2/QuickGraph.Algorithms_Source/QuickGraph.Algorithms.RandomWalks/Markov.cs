namespace QuickGraph.Algorithms.RandomWalks
{
    using System;

    public sealed class Markov
    {
        public static int UniformNextEntry(int count, Random rnd)
        {
            double num = rnd.NextDouble();
            int num2 = (int) Math.Floor(count * num);
            if (num2 == count)
            {
                num2--;
            }
            return num2;
        }
    }
}

