namespace QuickGraph.Algorithms.TestGames
{
    using QuickGraph.Collections;
    using QuickGraph.Concepts;
    using QuickGraph.Concepts.Collections;
    using QuickGraph.Predicates;
    using System;

    public class OptimalWinningStrategyCalculationAlgorithm
    {
        private VertexDoubleDictionary costs = null;
        private VertexCollection goals = new VertexCollection();
        private PriorithizedVertexBuffer priorityQueue = null;
        private VertexEdgeDictionary states = new VertexEdgeDictionary();
        private ITestGraph testGraph;
        private VertexIntDictionary unvisitedSuccessorCounts = null;

        public OptimalWinningStrategyCalculationAlgorithm(ITestGraph testGraph)
        {
            if (testGraph == null)
            {
                throw new ArgumentNullException("testGraph");
            }
            this.testGraph = testGraph;
        }

        public void Calculate()
        {
            this.Initialize();
            this.CalculateWinningStrategy();
        }

        protected void CalculateWinningStrategy()
        {
            while (this.priorityQueue.get_Count() != 0)
            {
                IVertex v = this.priorityQueue.Pop();
                this.Relax(v);
            }
        }

        protected void Initialize()
        {
            this.costs = new VertexDoubleDictionary();
            this.priorityQueue = new PriorithizedVertexBuffer(this.costs);
            this.unvisitedSuccessorCounts = new VertexIntDictionary();
            this.states.Clear();
            VertexCollection.Enumerator enumerator = this.goals.GetEnumerator();
            while (enumerator.MoveNext())
            {
                IVertex vertex = enumerator.get_Current();
                this.costs.Add(vertex, 0.0);
                this.priorityQueue.Push(vertex);
            }
            IVertexEnumerator enumerator2 = this.NotGoals.GetEnumerator();
            while (enumerator2.MoveNext())
            {
                IVertex vertex2 = enumerator2.get_Current();
                this.costs.Add(vertex2, double.PositiveInfinity);
            }
            IVertexEnumerator enumerator3 = this.TestGraph.ChoicePoints.GetEnumerator();
            while (enumerator3.MoveNext())
            {
                IVertex vertex3 = enumerator3.get_Current();
                this.unvisitedSuccessorCounts.Add(vertex3, this.testGraph.Graph.OutDegree(vertex3));
            }
            IVertexEnumerator enumerator4 = this.TestGraph.Graph.get_Vertices().GetEnumerator();
            while (enumerator4.MoveNext())
            {
                IVertex vertex4 = enumerator4.get_Current();
                this.states.Add(vertex4, null);
            }
        }

        protected void Relax(IVertex v)
        {
            IEdgeEnumerator enumerator = this.testGraph.Graph.InEdges(v).GetEnumerator();
            while (enumerator.MoveNext())
            {
                IEdge e = enumerator.get_Current();
                IVertex vertex = e.get_Source();
                if (this.testGraph.ContainsChoicePoint(vertex))
                {
                    VertexIntDictionary dictionary;
                    IVertex vertex2;
                    (dictionary = this.unvisitedSuccessorCounts).set_Item(vertex2 = vertex, dictionary.get_Item(vertex2) - 1);
                    if (this.unvisitedSuccessorCounts.Count != 0)
                    {
                        double num = 0.0;
                        IEdgeEnumerator enumerator2 = this.testGraph.Graph.OutEdges(vertex).GetEnumerator();
                        while (enumerator2.MoveNext())
                        {
                            IEdge edge2 = enumerator2.get_Current();
                            num = Math.Max(num, this.TestGraph.Cost(edge2) + this.costs.get_Item(edge2.get_Target()));
                        }
                        this.costs.set_Item(vertex, num);
                        this.priorityQueue.Push(vertex);
                    }
                }
                else
                {
                    double num2 = this.TestGraph.Cost(e) + this.costs.get_Item(v);
                    if (this.costs.get_Item(vertex) > num2)
                    {
                        this.costs.set_Item(vertex, num2);
                        this.priorityQueue.Push(vertex);
                        this.states.set_Item(vertex, e);
                    }
                }
            }
        }

        public VertexCollection Goals
        {
            get
            {
                return this.goals;
            }
        }

        protected IVertexEnumerable NotGoals
        {
            get
            {
                return new FilteredVertexEnumerable(this.testGraph.Graph.get_Vertices(), Preds.Not(Preds.InCollection(this.goals)));
            }
        }

        public VertexEdgeDictionary States
        {
            get
            {
                return this.states;
            }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("states");
                }
                this.states = value;
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

