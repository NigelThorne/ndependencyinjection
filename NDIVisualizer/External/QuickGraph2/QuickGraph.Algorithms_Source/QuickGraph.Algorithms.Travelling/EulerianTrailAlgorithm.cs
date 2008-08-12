namespace QuickGraph.Algorithms.Travelling
{
    using QuickGraph.Algorithms;
    using QuickGraph.Algorithms.Search;
    using QuickGraph.Algorithms.Visitors;
    using QuickGraph.Collections;
    using QuickGraph.Concepts;
    using QuickGraph.Concepts.Algorithms;
    using QuickGraph.Concepts.Collections;
    using QuickGraph.Concepts.Modifications;
    using QuickGraph.Concepts.MutableTraversals;
    using QuickGraph.Concepts.Traversals;
    using QuickGraph.Predicates;
    using System;
    using System.Runtime.CompilerServices;

    public class EulerianTrailAlgorithm : IAlgorithm
    {
        private EdgeCollection circuit;
        private IVertex currentVertex;
        private EdgeCollection temporaryCircuit;
        private EdgeCollection temporaryEdges;
        private IVertexAndEdgeListGraph visitedGraph;

        public event EdgeEventHandler CircuitEdge;

        public event EdgeEventHandler TreeEdge;

        public event EdgeEventHandler VisitEdge;

        public EulerianTrailAlgorithm(IVertexAndEdgeListGraph g)
        {
            if (g == null)
            {
                throw new ArgumentNullException("g");
            }
            this.visitedGraph = g;
            this.circuit = new EdgeCollection();
            this.temporaryCircuit = new EdgeCollection();
            this.currentVertex = null;
            this.temporaryEdges = new EdgeCollection();
        }

        public EdgeCollection AddTemporaryEdges(IMutableVertexAndEdgeListGraph g)
        {
            if (g == null)
            {
                throw new ArgumentNullException("g");
            }
            VertexCollection vertexs = QuickGraph.Algorithms.AlgoUtility.OddVertices(g);
            if ((vertexs.Count % 2) != 0)
            {
                throw new Exception("number of odd vertices in not even!");
            }
            EdgeCollection edges = new EdgeCollection();
            while (vertexs.Count > 0)
            {
                IVertex vertex = vertexs.get_Item(0);
                bool flag = false;
                bool flag3 = false;
                IEdgeEnumerator enumerator = g.OutEdges(vertex).GetEnumerator();
                while (enumerator.MoveNext())
                {
                    IVertex vertex2 = enumerator.get_Current().get_Target();
                    if ((vertex2 != vertex) && vertexs.Contains(vertex2))
                    {
                        flag3 = true;
                        bool flag2 = false;
                        IEdgeEnumerator enumerator2 = g.OutEdges(vertex2).GetEnumerator();
                        while (enumerator2.MoveNext())
                        {
                            if (enumerator2.get_Current().get_Target() == vertex)
                            {
                                flag2 = true;
                                break;
                            }
                        }
                        if (!flag2)
                        {
                            IEdge edge3 = g.AddEdge(vertex2, vertex);
                            edges.Add(edge3);
                            vertexs.Remove(vertex);
                            vertexs.Remove(vertex2);
                            flag = true;
                            break;
                        }
                    }
                }
                if (!flag3)
                {
                    if (vertexs.Count < 2)
                    {
                        throw new Exception("Eulerian trail failure");
                    }
                    IVertex vertex3 = vertexs.get_Item(1);
                    IEdge edge4 = g.AddEdge(vertex, vertex3);
                    edges.Add(edge4);
                    vertexs.Remove(vertex);
                    vertexs.Remove(vertex3);
                    flag = true;
                }
                if (!flag)
                {
                    vertexs.Remove(vertex);
                    vertexs.Add(vertex);
                }
            }
            this.temporaryEdges = edges;
            return edges;
        }

        protected bool CircuitAugmentation()
        {
            EdgeCollection edges = new EdgeCollection();
            int num = 0;
            while (num < this.Circuit.Count)
            {
                IEdge edge = this.Circuit.get_Item(num);
                if (edge.get_Source() == this.CurrentVertex)
                {
                    break;
                }
                edges.Add(edge);
                num++;
            }
            for (int i = 0; i < this.TemporaryCircuit.Count; i++)
            {
                IEdge e = this.TemporaryCircuit.get_Item(i);
                edges.Add(e);
                this.OnCircuitEdge(e);
                if (e.get_Target() == this.CurrentVertex)
                {
                    break;
                }
            }
            this.TemporaryCircuit.Clear();
            while (num < this.Circuit.Count)
            {
                IEdge edge3 = this.Circuit.get_Item(num);
                edges.Add(edge3);
                num++;
            }
            this.circuit = edges;
            return (this.Circuit.Count == this.VisitedGraph.get_EdgesCount());
        }

        public void Compute()
        {
            this.CurrentVertex = Traversal.FirstVertex(this.VisitedGraph);
            this.Search(this.CurrentVertex);
            if (!this.CircuitAugmentation())
            {
                while (this.Visit() && !this.CircuitAugmentation())
                {
                }
            }
        }

        public static int ComputeEulerianPathCount(IVertexAndEdgeListGraph g)
        {
            if (g == null)
            {
                throw new ArgumentNullException("g");
            }
            int count = QuickGraph.Algorithms.AlgoUtility.OddVertices(g).Count;
            if (count == 0)
            {
                return -1;
            }
            if ((count % 2) != 0)
            {
                return 0;
            }
            if (count == 0)
            {
                return 1;
            }
            return (count / 2);
        }

        protected virtual void OnCircuitEdge(IEdge e)
        {
            if (this.CircuitEdge != null)
            {
                this.CircuitEdge.Invoke(this, new EdgeEventArgs(e));
            }
        }

        protected virtual void OnTreeEdge(IEdge e)
        {
            if (this.TreeEdge != null)
            {
                this.TreeEdge.Invoke(this, new EdgeEventArgs(e));
            }
        }

        protected virtual void OnVisitEdge(IEdge e)
        {
            if (this.VisitEdge != null)
            {
                this.VisitEdge.Invoke(this, new EdgeEventArgs(e));
            }
        }

        object IAlgorithm.get_VisitedGraph()
        {
            return this.VisitedGraph;
        }

        public void RemoveTemporaryEdges(IEdgeMutableGraph g)
        {
            EdgeCollection.Enumerator enumerator = this.TemporaryEdges.GetEnumerator();
            while (enumerator.MoveNext())
            {
                IEdge edge = enumerator.get_Current();
                g.RemoveEdge(edge);
            }
        }

        protected bool Search(IVertex u)
        {
            IEdgeEnumerator enumerator = this.SelectOutEdgesNotInCircuit(u).GetEnumerator();
            while (enumerator.MoveNext())
            {
                IEdge e = enumerator.get_Current();
                this.OnTreeEdge(e);
                IVertex vertex = e.get_Target();
                this.TemporaryCircuit.Add(e);
                if (e.get_Target() == this.CurrentVertex)
                {
                    return true;
                }
                if (this.Search(vertex))
                {
                    return true;
                }
                this.TemporaryCircuit.Remove(e);
            }
            return false;
        }

        private IEdgeEnumerable SelectOutEdgesNotInCircuit(IVertex v)
        {
            return new FilteredEdgeEnumerable(this.VisitedGraph.OutEdges(v), new NotInCircuitEdgePredicate(this.Circuit, this.TemporaryCircuit));
        }

        private IEdge SelectSingleOutEdgeNotInCircuit(IVertex v)
        {
            IEdgeEnumerator enumerator = this.SelectOutEdgesNotInCircuit(v).GetEnumerator();
            if (!enumerator.MoveNext())
            {
                return null;
            }
            return enumerator.get_Current();
        }

        public EdgeCollectionCollection Trails()
        {
            EdgeCollectionCollection collections = new EdgeCollectionCollection();
            EdgeCollection edges = new EdgeCollection();
            EdgeCollection.Enumerator enumerator = this.Circuit.GetEnumerator();
            while (enumerator.MoveNext())
            {
                IEdge edge = enumerator.get_Current();
                if (this.TemporaryEdges.Contains(edge))
                {
                    if (edges.Count != 0)
                    {
                        collections.Add(edges);
                    }
                    edges = new EdgeCollection();
                }
                else
                {
                    edges.Add(edge);
                }
            }
            if (edges.Count != 0)
            {
                collections.Add(edges);
            }
            return collections;
        }

        public EdgeCollectionCollection Trails(IVertex s)
        {
            if (s == null)
            {
                throw new ArgumentNullException("s");
            }
            if (this.Circuit.Count == 0)
            {
                throw new Exception("Circuit is empty");
            }
            int num = 0;
            num = 0;
            while (num < this.Circuit.Count)
            {
                IEdge edge = this.Circuit.get_Item(num);
                if (!this.TemporaryEdges.Contains(edge) && (edge.get_Source() == s))
                {
                    break;
                }
                num++;
            }
            if (num == this.Circuit.Count)
            {
                throw new Exception("Did not find vertex in eulerian trail?");
            }
            EdgeCollectionCollection collections = new EdgeCollectionCollection();
            EdgeCollection edges = new EdgeCollection();
            BreadthFirstSearchAlgorithm algorithm = new BreadthFirstSearchAlgorithm(this.VisitedGraph);
            PredecessorRecorderVisitor vis = new PredecessorRecorderVisitor();
            algorithm.RegisterPredecessorRecorderHandlers(vis);
            algorithm.Compute(s);
            int num2 = num;
            while (num < this.Circuit.Count)
            {
                IEdge edge2 = this.Circuit.get_Item(num);
                if (this.TemporaryEdges.Contains(edge2))
                {
                    if (edges.Count != 0)
                    {
                        collections.Add(edges);
                    }
                    edges = vis.Path(edge2.get_Target());
                }
                else
                {
                    edges.Add(edge2);
                }
                num++;
            }
            for (num = 0; num < num2; num++)
            {
                IEdge edge3 = this.Circuit.get_Item(num);
                if (this.TemporaryEdges.Contains(edge3))
                {
                    if (edges.Count != 0)
                    {
                        collections.Add(edges);
                    }
                    edges = vis.Path(edge3.get_Target());
                }
                else
                {
                    edges.Add(edge3);
                }
            }
            if (edges.Count != 0)
            {
                collections.Add(edges);
            }
            return collections;
        }

        protected bool Visit()
        {
            EdgeCollection.Enumerator enumerator = this.Circuit.GetEnumerator();
            while (enumerator.MoveNext())
            {
                IEdge edge = enumerator.get_Current();
                IEdge e = this.SelectSingleOutEdgeNotInCircuit(edge.get_Source());
                if (e != null)
                {
                    this.OnVisitEdge(e);
                    this.CurrentVertex = edge.get_Source();
                    if (this.Search(this.CurrentVertex))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public EdgeCollection Circuit
        {
            get
            {
                return this.circuit;
            }
        }

        internal IVertex CurrentVertex
        {
            get
            {
                return this.currentVertex;
            }
            set
            {
                this.currentVertex = value;
            }
        }

        internal EdgeCollection TemporaryCircuit
        {
            get
            {
                return this.temporaryCircuit;
            }
        }

        internal EdgeCollection TemporaryEdges
        {
            get
            {
                return this.temporaryEdges;
            }
        }

        public IVertexAndEdgeListGraph VisitedGraph
        {
            get
            {
                return this.visitedGraph;
            }
        }
    }
}

