namespace QuickGraph.Algorithms.MaximumFlow
{
    using QuickGraph.Algorithms.Search;
    using QuickGraph.Algorithms.Visitors;
    using QuickGraph.Collections;
    using QuickGraph.Collections.Filtered;
    using QuickGraph.Concepts;
    using QuickGraph.Concepts.Collections;
    using QuickGraph.Concepts.Traversals;
    using QuickGraph.Predicates;
    using System;

    public class PushRelabelMaximumFlowAlgorithm : MaximumFlowAlgorithm
    {
        private const int alpha = 6;
        private const int beta = 12;
        private VertexIntDictionary current;
        private VertexIntDictionary distances;
        private VertexDoubleDictionary excessFlow;
        private const double globalUpdateFrequency = 0.5;
        private PreflowLayer[] layers;
        private int maxActive;
        private int maxDistance;
        private int minActive;
        private int n;
        private int nm;
        private IVertex sink;
        private IVertex src;
        private IIndexedVertexListGraph visitedGraph;
        private int workSinceLastUpdate;

        public PushRelabelMaximumFlowAlgorithm(IIndexedVertexListGraph g, EdgeDoubleDictionary capacities, EdgeEdgeDictionary reversedEdges) : base(g, capacities, reversedEdges)
        {
            this.visitedGraph = g;
            this.excessFlow = new VertexDoubleDictionary();
            this.current = new VertexIntDictionary();
            this.distances = new VertexIntDictionary();
        }

        private void AddToActiveList(IVertex u, PreflowLayer layer)
        {
            layer.ActiveVertices.Insert(0, u);
            this.maxActive = Math.Max(this.Distances.get_Item(u), this.maxActive);
            this.minActive = Math.Min(this.Distances.get_Item(u), this.minActive);
        }

        private void AddToInactiveList(IVertex u, PreflowLayer layer)
        {
            layer.InactiveVertices.Insert(0, u);
        }

        public override double Compute(IVertex src, IVertex sink)
        {
            if (src == null)
            {
                throw new ArgumentNullException("src");
            }
            if (sink == null)
            {
                throw new ArgumentNullException("sink");
            }
            this.src = src;
            this.sink = sink;
            this.Initialize();
            double num = this.MaximumPreflow();
            this.ConvertPreflowToFlow();
            return num;
        }

        private void ConvertPreflowToFlow()
        {
            IVertex vertex3;
            IVertex vertex4 = null;
            IVertex vertex5 = null;
            VertexVertexDictionary dictionary = new VertexVertexDictionary();
            VertexVertexDictionary dictionary2 = new VertexVertexDictionary();
            IVertexEnumerator enumerator3 = this.VisitedGraph.get_Vertices().GetEnumerator();
            while (enumerator3.MoveNext())
            {
                IVertex vertex6 = enumerator3.get_Current();
                IEdgeEnumerator enumerator4 = this.VisitedGraph.OutEdges(vertex6).GetEnumerator();
                while (enumerator4.MoveNext())
                {
                    IEdge edge = enumerator4.get_Current();
                    if (edge.get_Target() == vertex6)
                    {
                        base.ResidualCapacities.set_Item(edge, base.Capacities.get_Item(edge));
                    }
                }
                base.Colors.set_Item(vertex6, 0);
                dictionary.set_Item(vertex6, vertex6);
                this.current.set_Item(vertex6, 0);
            }
            IVertexEnumerator enumerator = this.VisitedGraph.get_Vertices().GetEnumerator();
            while (enumerator.MoveNext())
            {
                vertex3 = enumerator.get_Current();
                if (((base.Colors.get_Item(vertex3) != null) || (this.excessFlow.get_Item(vertex3) <= 0.0)) || ((vertex3 == this.src) || (vertex3 == this.sink)))
                {
                    continue;
                }
                IVertex vertex = vertex3;
                base.Colors.set_Item(vertex, 2);
            Label_0392:
                do
                {
                    while (this.current.get_Item(vertex3) < this.VisitedGraph.OutDegree(vertex3))
                    {
                        VertexIntDictionary dictionary6;
                        IVertex vertex9;
                        IEdge a = this.VisitedGraph.OutEdges(vertex3).get_Item(this.current.get_Item(vertex3));
                        if ((base.Capacities.get_Item(a) == 0.0) && this.IsResidualEdge(a))
                        {
                            IVertex vertex7 = a.get_Target();
                            if (base.Colors.get_Item(vertex7) == null)
                            {
                                base.Colors.set_Item(vertex7, 2);
                                dictionary.set_Item(vertex7, vertex3);
                                vertex3 = vertex7;
                                break;
                            }
                            if (base.Colors.get_Item(vertex7) == 2)
                            {
                                double num = base.ResidualCapacities.get_Item(a);
                                while (true)
                                {
                                    IEdge edge3 = this.VisitedGraph.OutEdges(vertex7).get_Item(this.current.get_Item(vertex7));
                                    num = Math.Min(num, base.ResidualCapacities.get_Item(edge3));
                                    if (vertex7 == vertex3)
                                    {
                                        break;
                                    }
                                    vertex7 = edge3.get_Target();
                                }
                                vertex7 = vertex3;
                                do
                                {
                                    EdgeDoubleDictionary dictionary3;
                                    IEdge edge4;
                                    EdgeDoubleDictionary dictionary4;
                                    IEdge edge5;
                                    a = this.VisitedGraph.OutEdges(vertex7).get_Item(this.current.get_Item(vertex7));
                                    (dictionary3 = base.ResidualCapacities).set_Item(edge4 = a, dictionary3.get_Item(edge4) - num);
                                    (dictionary4 = base.ResidualCapacities).set_Item(edge5 = base.ReversedEdges.get_Item(a), dictionary4.get_Item(edge5) + num);
                                    vertex7 = a.get_Target();
                                }
                                while (vertex7 != vertex3);
                                IVertex vertex2 = vertex3;
                                for (vertex7 = this.VisitedGraph.OutEdges(vertex3).get_Item(this.current.get_Item(vertex3)).get_Target(); vertex7 != vertex3; vertex7 = a.get_Target())
                                {
                                    a = this.VisitedGraph.OutEdges(vertex7).get_Item(this.current.get_Item(vertex7));
                                    if ((base.Colors.get_Item(vertex7) == null) || this.IsSaturated(a))
                                    {
                                        base.Colors.set_Item(this.VisitedGraph.OutEdges(vertex7).get_Item(this.current.get_Item(vertex7)).get_Target(), 0);
                                        if (base.Colors.get_Item(vertex7) != null)
                                        {
                                            vertex2 = vertex7;
                                        }
                                    }
                                }
                                if (vertex2 != vertex3)
                                {
                                    VertexIntDictionary dictionary5;
                                    IVertex vertex8;
                                    vertex3 = vertex2;
                                    (dictionary5 = this.current).set_Item(vertex8 = vertex3, dictionary5.get_Item(vertex8) + 1);
                                    break;
                                }
                            }
                        }
                        (dictionary6 = this.current).set_Item(vertex9 = vertex3, dictionary6.get_Item(vertex9) + 1);
                    }
                }
                while (this.current.get_Item(vertex3) != this.VisitedGraph.OutDegree(vertex3));
                base.Colors.set_Item(vertex3, 1);
                if (vertex3 != this.src)
                {
                    if (vertex4 == null)
                    {
                        vertex4 = vertex3;
                        vertex5 = vertex3;
                    }
                    else
                    {
                        dictionary2.set_Item(vertex3, vertex5);
                        vertex5 = vertex3;
                    }
                }
                if (vertex3 != vertex)
                {
                    VertexIntDictionary dictionary7;
                    IVertex vertex10;
                    vertex3 = dictionary.get_Item(vertex3);
                    (dictionary7 = this.current).set_Item(vertex10 = vertex3, dictionary7.get_Item(vertex10) + 1);
                    goto Label_0392;
                }
            }
            if (vertex4 != null)
            {
                IEdgeEnumerator enumerator2;
                for (vertex3 = vertex5; vertex3 != vertex4; vertex3 = dictionary2.get_Item(vertex3))
                {
                    enumerator2 = this.VisitedGraph.OutEdges(vertex3).GetEnumerator();
                    while ((this.excessFlow.get_Item(vertex3) > 0.0) && enumerator2.MoveNext())
                    {
                        if ((base.Capacities.get_Item(enumerator2.get_Current()) == 0.0) && this.IsResidualEdge(enumerator2.get_Current()))
                        {
                            this.PushFlow(enumerator2.get_Current());
                        }
                    }
                }
                vertex3 = vertex4;
                enumerator2 = this.VisitedGraph.OutEdges(vertex3).GetEnumerator();
                while ((this.excessFlow.get_Item(vertex3) > 0.0) && enumerator2.MoveNext())
                {
                    if ((base.Capacities.get_Item(enumerator2.get_Current()) == 0.0) && this.IsResidualEdge(enumerator2.get_Current()))
                    {
                        this.PushFlow(enumerator2.get_Current());
                    }
                }
            }
        }

        private void Discharge(IVertex u)
        {
            int num;
        Label_0005:
            num = this.current.get_Item(u);
            while (num < this.VisitedGraph.OutDegree(u))
            {
                IEdge a = this.VisitedGraph.OutEdges(u).get_Item(num);
                if (this.IsResidualEdge(a))
                {
                    IVertex v = a.get_Target();
                    if (this.IsAdmissible(u, v))
                    {
                        if ((v != this.sink) && (this.excessFlow.get_Item(v) == 0.0))
                        {
                            this.RemoveFromInactiveList(v);
                            this.AddToActiveList(v, this.layers[this.distances.get_Item(v)]);
                        }
                        this.PushFlow(a);
                        if (this.excessFlow.get_Item(u) == 0.0)
                        {
                            break;
                        }
                    }
                }
                num++;
            }
            PreflowLayer layer = this.layers[this.distances.get_Item(u)];
            int emptyDistance = this.distances.get_Item(u);
            if (num == this.VisitedGraph.OutDegree(u))
            {
                this.RelabelDistance(u);
                if ((layer.ActiveVertices.Count == 0) && (layer.InactiveVertices.Count == 0))
                {
                    this.Gap(emptyDistance);
                }
                if (this.distances.get_Item(u) == this.n)
                {
                    return;
                }
                goto Label_0005;
            }
            this.current.set_Item(u, num);
            this.AddToInactiveList(u, layer);
        }

        private void Gap(int emptyDistance)
        {
            int num = emptyDistance - 1;
            for (int i = emptyDistance + 1; i < this.maxDistance; i++)
            {
                VertexCollection.Enumerator enumerator = this.layers[i].InactiveVertices.GetEnumerator();
                while (enumerator.MoveNext())
                {
                    IVertex vertex = enumerator.get_Current();
                    this.distances.set_Item(vertex, this.n);
                }
                this.layers[i].InactiveVertices.Clear();
            }
            this.maxDistance = num;
            this.maxActive = num;
        }

        private void GlobalDistanceUpdate()
        {
            IVertexEnumerator enumerator = this.VisitedGraph.get_Vertices().GetEnumerator();
            while (enumerator.MoveNext())
            {
                IVertex vertex = enumerator.get_Current();
                base.Colors.set_Item(vertex, 0);
                this.Distances.set_Item(vertex, this.n);
            }
            this.Distances.set_Item(this.sink, 0);
            for (int i = 0; i <= this.maxDistance; i++)
            {
                this.layers[i].ActiveVertices.Clear();
                this.layers[i].InactiveVertices.Clear();
            }
            this.maxDistance = this.maxActive = 0;
            this.minActive = this.n;
            BreadthFirstSearchAlgorithm algorithm = new BreadthFirstSearchAlgorithm(this.ResidualGraph, new VertexBuffer(), base.Colors);
            DistanceRecorderVisitor visitor = new DistanceRecorderVisitor(this.Distances);
            algorithm.TreeEdge += new EdgeEventHandler(visitor, (IntPtr) this.TreeEdge);
            algorithm.DiscoverVertex += new VertexEventHandler(this, (IntPtr) this.GlobalDistanceUpdateHelper);
            algorithm.Compute(this.sink);
        }

        private void GlobalDistanceUpdateHelper(object sender, VertexEventArgs e)
        {
            if (e.get_Vertex() != this.sink)
            {
                this.current.set_Item(e.get_Vertex(), 0);
                this.maxDistance = Math.Max(this.distances.get_Item(e.get_Vertex()), this.maxDistance);
                if (this.excessFlow.get_Item(e.get_Vertex()) > 0.0)
                {
                    this.AddToActiveList(e.get_Vertex(), this.layers[this.distances.get_Item(e.get_Vertex())]);
                }
                else
                {
                    this.AddToInactiveList(e.get_Vertex(), this.layers[this.distances.get_Item(e.get_Vertex())]);
                }
            }
        }

        private void Initialize()
        {
            int num = 0;
            this.n = this.VisitedGraph.get_VerticesCount();
            IVertexEnumerator enumerator = this.VisitedGraph.get_Vertices().GetEnumerator();
            while (enumerator.MoveNext())
            {
                IVertex vertex = enumerator.get_Current();
                num += this.VisitedGraph.OutDegree(vertex);
            }
            num /= 2;
            this.nm = (6 * this.n) + num;
            this.excessFlow.Clear();
            this.current.Clear();
            this.distances.Clear();
            this.layers = new PreflowLayer[this.n];
            for (int i = 0; i < this.n; i++)
            {
                this.layers[i] = new PreflowLayer();
            }
            IVertexEnumerator enumerator2 = this.VisitedGraph.get_Vertices().GetEnumerator();
            while (enumerator2.MoveNext())
            {
                IVertex vertex2 = enumerator2.get_Current();
                IEdgeEnumerator enumerator3 = this.VisitedGraph.OutEdges(vertex2).GetEnumerator();
                while (enumerator3.MoveNext())
                {
                    IEdge edge = enumerator3.get_Current();
                    base.ResidualCapacities.set_Item(edge, base.Capacities.get_Item(edge));
                }
                this.excessFlow.set_Item(vertex2, 0.0);
                this.current.set_Item(vertex2, 0);
            }
            bool flag = false;
            double d = 0.0;
            IEdgeEnumerator enumerator4 = this.VisitedGraph.OutEdges(this.src).GetEnumerator();
            while (enumerator4.MoveNext())
            {
                IEdge edge2 = enumerator4.get_Current();
                if (edge2.get_Target() != this.src)
                {
                    d += base.ResidualCapacities.get_Item(edge2);
                }
            }
            if ((d >= double.MaxValue) || double.IsPositiveInfinity(d))
            {
                flag = true;
            }
            if (flag)
            {
                this.excessFlow.set_Item(this.src, double.MaxValue);
            }
            else
            {
                this.excessFlow.set_Item(this.src, 0.0);
                IEdgeEnumerator enumerator5 = this.VisitedGraph.OutEdges(this.src).GetEnumerator();
                while (enumerator5.MoveNext())
                {
                    IEdge edge3 = enumerator5.get_Current();
                    if (edge3.get_Target() != this.src)
                    {
                        EdgeDoubleDictionary dictionary;
                        IEdge edge4;
                        EdgeDoubleDictionary dictionary2;
                        IEdge edge5;
                        VertexDoubleDictionary dictionary3;
                        IVertex vertex4;
                        double num4 = base.ResidualCapacities.get_Item(edge3);
                        (dictionary = base.ResidualCapacities).set_Item(edge4 = edge3, dictionary.get_Item(edge4) - num4);
                        (dictionary2 = base.ResidualCapacities).set_Item(edge5 = base.ReversedEdges.get_Item(edge3), dictionary2.get_Item(edge5) + num4);
                        (dictionary3 = this.excessFlow).set_Item(vertex4 = edge3.get_Target(), dictionary3.get_Item(vertex4) + num4);
                    }
                }
            }
            this.maxDistance = this.VisitedGraph.get_VerticesCount() - 1;
            this.maxActive = 0;
            this.minActive = this.n;
            IVertexEnumerator enumerator6 = this.VisitedGraph.get_Vertices().GetEnumerator();
            while (enumerator6.MoveNext())
            {
                IVertex u = enumerator6.get_Current();
                if (u == this.sink)
                {
                    this.distances.set_Item(u, 0);
                }
                else
                {
                    if ((u == this.src) && !flag)
                    {
                        this.distances.set_Item(u, this.n);
                    }
                    else
                    {
                        this.distances.set_Item(u, 1);
                    }
                    if (this.excessFlow.get_Item(u) > 0.0)
                    {
                        this.AddToActiveList(u, this.layers[1]);
                    }
                    else if (this.distances.get_Item(u) < this.n)
                    {
                        this.AddToInactiveList(u, this.layers[1]);
                    }
                }
            }
        }

        private bool IsAdmissible(IVertex u, IVertex v)
        {
            return (this.distances.get_Item(u) == (this.distances.get_Item(v) + 1));
        }

        private bool IsFlow()
        {
            IVertexEnumerator enumerator = this.VisitedGraph.get_Vertices().GetEnumerator();
            while (enumerator.MoveNext())
            {
                IVertex vertex = enumerator.get_Current();
                IEdgeEnumerator enumerator2 = this.VisitedGraph.OutEdges(vertex).GetEnumerator();
                while (enumerator2.MoveNext())
                {
                    IEdge edge = enumerator2.get_Current();
                    if ((base.Capacities.get_Item(edge) > 0.0) && ((((base.ResidualCapacities.get_Item(edge) + base.ResidualCapacities.get_Item(base.ReversedEdges.get_Item(edge))) != (base.Capacities.get_Item(edge) + base.Capacities.get_Item(base.ReversedEdges.get_Item(edge)))) || (base.ResidualCapacities.get_Item(edge) < 0.0)) || (base.ResidualCapacities.get_Item(base.ReversedEdges.get_Item(edge)) < 0.0)))
                    {
                        return false;
                    }
                }
            }
            IVertexEnumerator enumerator3 = this.VisitedGraph.get_Vertices().GetEnumerator();
            while (enumerator3.MoveNext())
            {
                IVertex vertex2 = enumerator3.get_Current();
                if ((vertex2 != this.src) && (vertex2 != this.sink))
                {
                    if (this.excessFlow.get_Item(vertex2) != 0.0)
                    {
                        return false;
                    }
                    double num = 0.0;
                    IEdgeEnumerator enumerator4 = this.VisitedGraph.OutEdges(vertex2).GetEnumerator();
                    while (enumerator4.MoveNext())
                    {
                        IEdge edge2 = enumerator4.get_Current();
                        if (base.Capacities.get_Item(edge2) > 0.0)
                        {
                            num -= base.Capacities.get_Item(edge2) - base.ResidualCapacities.get_Item(edge2);
                        }
                        else
                        {
                            num += base.ResidualCapacities.get_Item(edge2);
                        }
                    }
                    if (this.excessFlow.get_Item(vertex2) != num)
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        private bool IsOptimal()
        {
            this.GlobalDistanceUpdate();
            return (this.Distances.get_Item(this.src) >= this.n);
        }

        private bool IsResidualEdge(IEdge a)
        {
            return (0.0 < base.ResidualCapacities.get_Item(a));
        }

        private bool IsSaturated(IEdge a)
        {
            return (base.ResidualCapacities.get_Item(a) == 0.0);
        }

        private double MaximumPreflow()
        {
            this.workSinceLastUpdate = 0;
            while (this.maxActive >= this.minActive)
            {
                PreflowLayer layer = this.layers[this.maxActive];
                if (layer.ActiveVertices.Count == 0)
                {
                    this.maxActive--;
                }
                else
                {
                    IVertex u = layer.ActiveVertices.get_Item(0);
                    this.RemoveFromActiveList(u);
                    this.Discharge(u);
                    if ((this.workSinceLastUpdate * 0.5) > this.nm)
                    {
                        this.GlobalDistanceUpdate();
                        this.workSinceLastUpdate = 0;
                    }
                }
            }
            return this.excessFlow.get_Item(this.sink);
        }

        private void PushFlow(IEdge e)
        {
            EdgeDoubleDictionary dictionary;
            IEdge edge;
            EdgeDoubleDictionary dictionary2;
            IEdge edge2;
            VertexDoubleDictionary dictionary3;
            IVertex vertex3;
            VertexDoubleDictionary dictionary4;
            IVertex vertex4;
            IVertex vertex = e.get_Source();
            IVertex vertex2 = e.get_Target();
            double num = Math.Min(this.excessFlow.get_Item(vertex), base.ResidualCapacities.get_Item(e));
            (dictionary = base.ResidualCapacities).set_Item(edge = e, dictionary.get_Item(edge) - num);
            (dictionary2 = base.ResidualCapacities).set_Item(edge2 = base.ReversedEdges.get_Item(e), dictionary2.get_Item(edge2) + num);
            (dictionary3 = this.excessFlow).set_Item(vertex3 = vertex, dictionary3.get_Item(vertex3) - num);
            (dictionary4 = this.excessFlow).set_Item(vertex4 = vertex2, dictionary4.get_Item(vertex4) + num);
        }

        private int RelabelDistance(IVertex u)
        {
            int num = 0;
            this.workSinceLastUpdate += 12;
            int num2 = this.VisitedGraph.get_VerticesCount();
            this.distances.set_Item(u, num2);
            for (int i = 0; i < this.VisitedGraph.OutDegree(u); i++)
            {
                this.workSinceLastUpdate++;
                IEdge a = this.VisitedGraph.OutEdges(u).get_Item(i);
                IVertex vertex = a.get_Target();
                if (this.IsResidualEdge(a) && (this.distances.get_Item(vertex) < num2))
                {
                    num2 = this.distances.get_Item(vertex);
                    num = i;
                }
            }
            num2++;
            if (num2 < this.n)
            {
                this.distances.set_Item(u, num2);
                this.current.set_Item(u, num);
                this.maxDistance = Math.Max(num2, this.maxDistance);
            }
            return num2;
        }

        private void RemoveFromActiveList(IVertex u)
        {
            this.layers[this.Distances.get_Item(u)].ActiveVertices.Remove(u);
        }

        private void RemoveFromInactiveList(IVertex u)
        {
            this.layers[this.distances.get_Item(u)].InactiveVertices.Remove(u);
        }

        public VertexIntDictionary Distances
        {
            get
            {
                return this.distances;
            }
        }

        internal IVertexListGraph ResidualGraph
        {
            get
            {
                return new FilteredVertexListGraph(this.VisitedGraph, new ReversedResidualEdgePredicate(base.ResidualCapacities, base.ReversedEdges));
            }
        }

        public IIndexedVertexListGraph VisitedGraph
        {
            get
            {
                return this.visitedGraph;
            }
        }

        private class PreflowLayer
        {
            private VertexCollection activeVertices = new VertexCollection();
            private VertexCollection inactiveVertices = new VertexCollection();

            public VertexCollection ActiveVertices
            {
                get
                {
                    return this.activeVertices;
                }
            }

            public VertexCollection InactiveVertices
            {
                get
                {
                    return this.inactiveVertices;
                }
            }
        }
    }
}

