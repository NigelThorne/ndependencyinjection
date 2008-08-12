namespace QuickGraph.Algorithms.AllShortestPath
{
    using QuickGraph.Algorithms.AllShortestPath.Testers;
    using QuickGraph.Collections;
    using QuickGraph.Concepts;
    using QuickGraph.Concepts.Algorithms;
    using QuickGraph.Concepts.Collections;
    using QuickGraph.Concepts.Traversals;
    using QuickGraph.Exceptions;
    using System;
    using System.Collections;
    using System.Runtime.CompilerServices;

    public class FloydWarshallAllShortestPathAlgorithm : IAlgorithm
    {
        private Hashtable definedPaths;
        private IFloydWarshallTester tester;
        private IVertexAndEdgeListGraph visitedGraph;

        public event FloydWarshallEventHandler InitiliazePath;

        public event FloydWarshallEventHandler NotReducePath;

        public event FloydWarshallEventHandler ProcessPath;

        public event FloydWarshallEventHandler ReducePath;

        public FloydWarshallAllShortestPathAlgorithm(IVertexAndEdgeListGraph visitedGraph, IFloydWarshallTester tester)
        {
            if (visitedGraph == null)
            {
                throw new ArgumentNullException("visitedGraph");
            }
            if (tester == null)
            {
                throw new ArgumentNullException("test");
            }
            this.visitedGraph = visitedGraph;
            this.tester = tester;
            this.definedPaths = null;
        }

        public void CheckConnectivityAndNegativeCycles(IVertexDistanceMatrix costs)
        {
            IVertexEnumerator enumerator = this.VisitedGraph.get_Vertices().GetEnumerator();
            while (enumerator.MoveNext())
            {
                IVertex vertex = enumerator.get_Current();
                if ((costs != null) && (costs.Distance(vertex, vertex) < 0.0))
                {
                    throw new NegativeCycleException("Graph has negative cycle");
                }
                IVertexEnumerator enumerator2 = this.VisitedGraph.get_Vertices().GetEnumerator();
                while (enumerator2.MoveNext())
                {
                    IVertex vertex2 = enumerator2.get_Current();
                    if (!this.DefinedPaths.Contains(new VertexPair(vertex, vertex2)))
                    {
                        throw new Exception("Graph is not strongly connected");
                    }
                }
            }
        }

        public void Compute()
        {
            this.definedPaths = new Hashtable();
            IVertexEnumerator enumerator = this.VisitedGraph.get_Vertices().GetEnumerator();
            while (enumerator.MoveNext())
            {
                IVertex source = enumerator.get_Current();
                IVertexEnumerator enumerator2 = this.VisitedGraph.get_Vertices().GetEnumerator();
                while (enumerator2.MoveNext())
                {
                    IVertex target = enumerator2.get_Current();
                    if (this.VisitedGraph.ContainsEdge(source, target))
                    {
                        this.DefinedPaths.Add(new VertexPair(source, target), null);
                    }
                    this.OnInitiliazePath(source, target);
                }
            }
            IVertexEnumerator enumerator3 = this.VisitedGraph.get_Vertices().GetEnumerator();
            while (enumerator3.MoveNext())
            {
                IVertex intermediate = enumerator3.get_Current();
                IVertexEnumerator enumerator4 = this.VisitedGraph.get_Vertices().GetEnumerator();
                while (enumerator4.MoveNext())
                {
                    IVertex vertex4 = enumerator4.get_Current();
                    if (this.DefinedPaths.Contains(new VertexPair(vertex4, intermediate)))
                    {
                        IVertexEnumerator enumerator5 = this.VisitedGraph.get_Vertices().GetEnumerator();
                        while (enumerator5.MoveNext())
                        {
                            IVertex vertex5 = enumerator5.get_Current();
                            this.OnProcessPath(vertex4, vertex5, intermediate);
                            bool flag = this.DefinedPaths.Contains(new VertexPair(intermediate, vertex5));
                            bool flag2 = this.DefinedPaths.Contains(new VertexPair(vertex4, vertex5));
                            if (flag && (flag2 || this.Tester.Test(vertex4, vertex5, intermediate)))
                            {
                                this.DefinedPaths[new VertexPair(vertex4, vertex5)] = null;
                                this.OnReducePath(vertex4, vertex5, intermediate);
                            }
                            else
                            {
                                this.OnNotReducePath(vertex4, vertex5, intermediate);
                            }
                        }
                    }
                }
            }
        }

        protected virtual void OnInitiliazePath(IVertex source, IVertex target)
        {
            if (this.InitiliazePath != null)
            {
                this.InitiliazePath(this, new FloydWarshallEventArgs(source, target));
            }
        }

        protected virtual void OnNotReducePath(IVertex source, IVertex target, IVertex intermediate)
        {
            if (this.NotReducePath != null)
            {
                this.NotReducePath(this, new FloydWarshallEventArgs(source, target, intermediate));
            }
        }

        protected virtual void OnProcessPath(IVertex source, IVertex target, IVertex intermediate)
        {
            if (this.ProcessPath != null)
            {
                this.ProcessPath(this, new FloydWarshallEventArgs(source, target, intermediate));
            }
        }

        protected virtual void OnReducePath(IVertex source, IVertex target, IVertex intermediate)
        {
            if (this.ReducePath != null)
            {
                this.ReducePath(this, new FloydWarshallEventArgs(source, target, intermediate));
            }
        }

        object IAlgorithm.get_VisitedGraph()
        {
            return this.VisitedGraph;
        }

        private Hashtable DefinedPaths
        {
            get
            {
                return this.definedPaths;
            }
        }

        public IFloydWarshallTester Tester
        {
            get
            {
                return this.tester;
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

