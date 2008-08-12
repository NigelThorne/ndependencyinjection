namespace QuickGraph.Algorithms.Ranking
{
    using QuickGraph.Collections;
    using QuickGraph.Collections.Filtered;
    using QuickGraph.Concepts;
    using QuickGraph.Concepts.Algorithms;
    using QuickGraph.Concepts.Collections;
    using QuickGraph.Concepts.Traversals;
    using QuickGraph.Predicates;
    using System;
    using System.Collections;

    public class PageRankAlgorithm : IAlgorithm
    {
        private double damping = 0.85;
        private int maxIterations = 60;
        private VertexDoubleDictionary ranks = new VertexDoubleDictionary();
        private double tolerance = 9.88131291682493E-324;
        private IBidirectionalVertexListGraph visitedGraph;

        public PageRankAlgorithm(IBidirectionalVertexListGraph visitedGraph)
        {
            if (visitedGraph == null)
            {
                throw new ArgumentNullException("visitedGraph");
            }
            this.visitedGraph = visitedGraph;
        }

        public void Compute()
        {
            VertexDoubleDictionary dictionary = new VertexDoubleDictionary();
            FilteredBidirectionalGraph graph = new FilteredBidirectionalGraph(this.VisitedGraph, Preds.KeepAllEdges(), new InDictionaryVertexPredicate(this.ranks));
            int num = 0;
            double num2 = 0.0;
            do
            {
                num2 = 0.0;
                IDictionaryEnumerator enumerator = this.Ranks.GetEnumerator();
                while (enumerator.MoveNext())
                {
                    DictionaryEntry current = (DictionaryEntry) enumerator.Current;
                    IVertex key = (IVertex) current.Key;
                    double num3 = (double) current.Value;
                    double num4 = 0.0;
                    FilteredEdgeEnumerable.Enumerator enumerator2 = graph.InEdges(key).GetEnumerator();
                    while (enumerator2.MoveNext())
                    {
                        IEdge edge = enumerator2.get_Current();
                        num4 += this.ranks.get_Item(edge.get_Source()) / ((double) graph.OutDegree(edge.get_Source()));
                    }
                    double num5 = (1.0 - this.damping) + (this.damping * num4);
                    dictionary.set_Item(key, num5);
                    num2 += Math.Abs((double) (num3 - num5));
                }
                VertexDoubleDictionary ranks = this.ranks;
                this.ranks = dictionary;
                dictionary = ranks;
                num++;
            }
            while ((num2 > this.tolerance) && (num < this.maxIterations));
            Console.WriteLine("{0}, {1}", num, num2);
        }

        public double GetRanksMean()
        {
            return (this.GetRanksSum() / ((double) this.ranks.Count));
        }

        public double GetRanksSum()
        {
            double num = 0.0;
            foreach (double num2 in this.ranks.get_Values())
            {
                num += num2;
            }
            return num;
        }

        public void InitializeRanks()
        {
            this.ranks.Clear();
            IVertexEnumerator enumerator = this.VisitedGraph.get_Vertices().GetEnumerator();
            while (enumerator.MoveNext())
            {
                IVertex vertex = enumerator.get_Current();
                this.ranks.Add(vertex, 0.0);
            }
            this.RemoveDanglingLinks();
        }

        object IAlgorithm.get_VisitedGraph()
        {
            return this.VisitedGraph;
        }

        public void RemoveDanglingLinks()
        {
            VertexCollection vertexs = new VertexCollection();
            do
            {
                vertexs.Clear();
                IVertexListGraph graph = new FilteredVertexListGraph(this.VisitedGraph, new InDictionaryVertexPredicate(this.ranks));
                foreach (IVertex vertex in this.ranks.get_Keys())
                {
                    if (graph.OutDegree(vertex) == 0)
                    {
                        vertexs.Add(vertex);
                    }
                }
                VertexCollection.Enumerator enumerator = vertexs.GetEnumerator();
                while (enumerator.MoveNext())
                {
                    IVertex vertex2 = enumerator.get_Current();
                    this.ranks.Remove(vertex2);
                }
            }
            while (vertexs.Count != 0);
        }

        private static void swapRanks(VertexDoubleDictionary left, VertexDoubleDictionary right)
        {
            VertexDoubleDictionary dictionary = left;
            left = right;
            right = dictionary;
        }

        public double Damping
        {
            get
            {
                return this.damping;
            }
            set
            {
                this.damping = value;
            }
        }

        public int MaxIteration
        {
            get
            {
                return this.maxIterations;
            }
            set
            {
                this.maxIterations = value;
            }
        }

        public VertexDoubleDictionary Ranks
        {
            get
            {
                return this.ranks;
            }
        }

        public double Tolerance
        {
            get
            {
                return this.tolerance;
            }
            set
            {
                this.tolerance = value;
            }
        }

        public IBidirectionalVertexListGraph VisitedGraph
        {
            get
            {
                return this.visitedGraph;
            }
        }
    }
}

