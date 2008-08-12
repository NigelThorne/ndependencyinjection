namespace QuickGraph.Algorithms.AllShortestPath.Testers
{
    using QuickGraph.Concepts;
    using QuickGraph.Concepts.Collections;
    using System;

    public class FloydWarshallDistanceTester : IFloydWarshallTester
    {
        private IVertexDistanceMatrix distances;
        private IFloydWarshallDistanceTester tester;

        public FloydWarshallDistanceTester(IVertexDistanceMatrix distances, IFloydWarshallDistanceTester tester)
        {
            if (distances == null)
            {
                throw new ArgumentNullException("distances");
            }
            if (tester == null)
            {
                throw new ArgumentNullException("tester");
            }
            this.distances = distances;
            this.tester = tester;
        }

        public bool Test(IVertex source, IVertex target, IVertex intermediate)
        {
            return this.Tester.TestDistance(this.Distances, source, target, intermediate);
        }

        public IVertexDistanceMatrix Distances
        {
            get
            {
                return this.distances;
            }
        }

        public IFloydWarshallDistanceTester Tester
        {
            get
            {
                return this.tester;
            }
        }
    }
}

