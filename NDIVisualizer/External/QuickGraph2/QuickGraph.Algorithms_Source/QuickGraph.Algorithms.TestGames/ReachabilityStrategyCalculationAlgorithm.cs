namespace QuickGraph.Algorithms.TestGames
{
    using QuickGraph.Collections;
    using QuickGraph.Concepts;
    using QuickGraph.Concepts.Collections;
    using QuickGraph.Predicates;
    using System;

    public class ReachabilityStrategyCalculationAlgorithm
    {
        private VertexDoublesDictionary costs = null;
        private VertexCollection front = null;
        private VertexCollection goals = new VertexCollection();
        private VertexCollection newFront = null;
        private IPerformanceComparer performanceComparer;
        private VertexDoublesDictionary probs = null;
        private IStrategy strategy;
        private ITestGraph testGraph;

        public ReachabilityStrategyCalculationAlgorithm(ITestGraph testGraph, IStrategy strategy)
        {
            if (this.testGraph == null)
            {
                throw new ArgumentNullException("testGraph");
            }
            this.testGraph = testGraph;
            this.strategy = new QuickGraph.Algorithms.TestGames.Strategy();
            this.performanceComparer = new PairPreOrderPerformanceComparer();
        }

        public void Calculate(int moveCount)
        {
            this.Initialize();
            for (int i = 1; i < moveCount; i++)
            {
                this.PropagateChanges(i);
                IEdgeEnumerator enumerator = this.EdgesWidthTargetInFront.GetEnumerator();
                while (enumerator.MoveNext())
                {
                    IEdge e = enumerator.get_Current();
                    this.TraverseEdge(e, i);
                }
                this.front = this.newFront;
                this.newFront = new VertexCollection();
            }
        }

        protected bool Improving(IEdge e, int i)
        {
            return this.PerformanceComparer.Compare(this.probs.get_Item(e.get_Target()).get_Item(i - 1), this.TestGraph.Cost(e) + this.costs.get_Item(e.get_Target()).get_Item(i - 1), this.probs.get_Item(e.get_Source()).get_Item(i), this.costs.get_Item(e.get_Source()).get_Item(i));
        }

        protected void Initialize()
        {
            this.front = new VertexCollection();
            this.front.AddRange(this.goals);
            this.newFront = new VertexCollection();
            this.probs = new VertexDoublesDictionary();
            this.costs = new VertexDoublesDictionary();
            IVertexEnumerator enumerator = this.TestGraph.Graph.get_Vertices().GetEnumerator();
            while (enumerator.MoveNext())
            {
                IVertex vertex = enumerator.get_Current();
                DoubleCollection doubles = new DoubleCollection();
                if (this.Goals.Contains(vertex))
                {
                    doubles.Add(0.0);
                }
                else
                {
                    doubles.Add(1.0);
                }
                this.probs.Add(vertex, doubles);
                doubles = new DoubleCollection();
                doubles.Add(0.0);
                this.costs.Add(vertex, doubles);
            }
            IVertexEnumerator enumerator2 = this.TestGraph.States.GetEnumerator();
            while (enumerator2.MoveNext())
            {
                IVertex v = enumerator2.get_Current();
                this.Strategy.SetChooseEdge(v, 0, null);
            }
        }

        protected void PropagateChanges(int i)
        {
            IVertexEnumerator enumerator = this.TestGraph.Graph.get_Vertices().GetEnumerator();
            while (enumerator.MoveNext())
            {
                IVertex vertex = enumerator.get_Current();
                this.PropagateLast(this.probs.get_Item(vertex));
                this.PropagateLast(this.costs.get_Item(vertex));
            }
            IVertexEnumerator enumerator2 = this.TestGraph.States.GetEnumerator();
            while (enumerator2.MoveNext())
            {
                IVertex v = enumerator2.get_Current();
                this.Strategy.SetChooseEdge(v, i, this.Strategy.ChooseEdge(v, i - 1));
            }
        }

        private void PropagateLast(DoubleCollection col)
        {
            col.Add(col.get_Item(col.Count - 1));
        }

        protected void TraverseEdge(IEdge e, int i)
        {
            IVertex v = e.get_Source();
            IVertex vertex2 = e.get_Target();
            if (this.TestGraph.ContainsChoicePoint(v))
            {
                if (!this.newFront.Contains(v))
                {
                    double num = 0.0;
                    double num2 = 0.0;
                    IEdgeEnumerator enumerator = this.TestGraph.Graph.OutEdges(v).GetEnumerator();
                    while (enumerator.MoveNext())
                    {
                        IEdge edge = enumerator.get_Current();
                        num += this.TestGraph.Prob(edge) * this.probs.get_Item(edge.get_Target()).get_Item(i - 1);
                        num2 += this.TestGraph.Cost(edge) + this.costs.get_Item(edge.get_Target()).get_Item(i - 1);
                    }
                    this.probs.get_Item(v).Add(num);
                    this.costs.get_Item(v).Add(num2);
                    this.newFront.Add(v);
                }
            }
            else if (this.Improving(e, i))
            {
                this.Strategy.SetChooseEdge(v, i, e);
                this.probs.get_Item(v).Add(this.probs.get_Item(vertex2).get_Item(i - 1));
                this.costs.get_Item(v).Add(this.TestGraph.Cost(e) + this.costs.get_Item(vertex2).get_Item(i - 1));
                this.newFront.Add(v);
            }
        }

        private IEdgeEnumerable EdgesWidthTargetInFront
        {
            get
            {
                return new FilteredEdgeEnumerable(this.TestGraph.Graph.get_Edges(), Preds.OutEdge(Preds.KeepAllEdges(), Preds.InCollection(this.front)));
            }
        }

        public VertexCollection Goals
        {
            get
            {
                return this.goals;
            }
        }

        public IVertexEnumerable NotGoals
        {
            get
            {
                return new FilteredVertexEnumerable(this.testGraph.Graph.get_Vertices(), Preds.Not(Preds.InCollection(this.goals)));
            }
        }

        public IPerformanceComparer PerformanceComparer
        {
            get
            {
                return this.performanceComparer;
            }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("performanceComparer");
                }
                this.performanceComparer = value;
            }
        }

        public IStrategy Strategy
        {
            get
            {
                return this.strategy;
            }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("strategy");
                }
                this.strategy = value;
            }
        }

        public ITestGraph TestGraph
        {
            get
            {
                return this.testGraph;
            }
        }
    }
}

