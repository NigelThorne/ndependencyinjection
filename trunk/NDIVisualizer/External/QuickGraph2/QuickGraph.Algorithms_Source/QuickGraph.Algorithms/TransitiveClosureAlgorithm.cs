namespace QuickGraph.Algorithms
{
    using QuickGraph.Collections;
    using QuickGraph.Collections.Sort;
    using QuickGraph.Concepts;
    using QuickGraph.Concepts.Algorithms;
    using QuickGraph.Concepts.Collections;
    using QuickGraph.Concepts.MutableTraversals;
    using QuickGraph.Concepts.Traversals;
    using QuickGraph.Representations;
    using System;
    using System.Collections;
    using System.Reflection;
    using System.Runtime.CompilerServices;

    public class TransitiveClosureAlgorithm : IAlgorithm
    {
        private IMutableVertexAndEdgeListGraph cg;
        private VertexVertexDictionary graphTransitiveClosures;
        private IVertexListGraph visitedGraph;

        public event EdgeEventHandler ExamineEdge;

        public event TransitiveClosureVertexEventHandler InitTransitiveClosureVertex;

        public TransitiveClosureAlgorithm(IVertexListGraph g)
        {
            if (g == null)
            {
                throw new ArgumentNullException("g");
            }
            this.visitedGraph = g;
            this.cg = new AdjacencyGraph();
        }

        public void Create(IMutableVertexAndEdgeListGraph tc)
        {
            if (tc == null)
            {
                throw new ArgumentNullException("tc");
            }
            CondensationGraphAlgorithm algorithm = new CondensationGraphAlgorithm(this.VisitedGraph);
            algorithm.Create(this.cg);
            ArrayList vertices = new ArrayList(this.cg.get_VerticesCount());
            new TopologicalSortAlgorithm(this.cg, vertices).Compute();
            VertexIntDictionary dictionary = new VertexIntDictionary();
            VertexIntDictionary dictionary2 = new VertexIntDictionary();
            for (int i = 0; i < vertices.Count; i++)
            {
                IVertex vertex = (IVertex) vertices[i];
                dictionary2.Add(vertex, i);
                if (!dictionary.Contains(vertex))
                {
                    dictionary.Add(vertex, 0);
                }
            }
            VertexListMatrix chains = new VertexListMatrix();
            int num2 = -1;
        Label_0112:
            foreach (IVertex vertex2 in vertices)
            {
                if (dictionary.get_Item(vertex2) == 0)
                {
                    num2 = chains.AddRow();
                    IVertex vertex3 = vertex2;
                    while (true)
                    {
                        chains[num2].Add(vertex3);
                        dictionary.set_Item(vertex3, 1);
                        ArrayList list2 = this.TopoSortAdjVertices(vertex3, this.cg, dictionary2);
                        vertex3 = this.FirstNotInChain(list2, dictionary);
                        if (vertex3 == null)
                        {
                            goto Label_0112;
                        }
                    }
                }
            }
            VertexIntDictionary dictionary3 = new VertexIntDictionary();
            VertexIntDictionary dictionary4 = new VertexIntDictionary();
            this.SetChainPositions(chains, dictionary3, dictionary4);
            VertexListMatrix matrix2 = new VertexListMatrix();
            matrix2.CreateObjectMatrix(this.cg.get_VerticesCount(), chains.RowCount, 0x7fffffff);
            if (vertices.Count > 0)
            {
                for (int j = vertices.Count - 1; j > -1; j--)
                {
                    IVertex v = (IVertex) vertices[j];
                    foreach (IVertex vertex5 in this.TopoSortAdjVertices(v, this.cg, dictionary2))
                    {
                        if (dictionary2.get_Item(vertex5) < ((int) matrix2[v.get_ID()][dictionary3.get_Item(vertex5)]))
                        {
                            this.LeftUnion(matrix2[v.get_ID()], matrix2[vertex5.get_ID()]);
                            matrix2[v.get_ID()][dictionary3.get_Item(vertex5)] = dictionary2.get_Item(vertex5);
                        }
                    }
                }
            }
            ArrayList list3 = new ArrayList();
            IEdgeEnumerator enumerator = this.cg.get_Edges().GetEnumerator();
            while (enumerator.MoveNext())
            {
                IEdge edge = enumerator.get_Current();
                list3.Add(edge);
            }
            foreach (IEdge edge2 in list3)
            {
                this.cg.RemoveEdge(edge2);
            }
            IVertexEnumerator enumerator5 = this.cg.get_Vertices().GetEnumerator();
            while (enumerator5.MoveNext())
            {
                IVertex vertex6 = enumerator5.get_Current();
                int num4 = vertex6.get_ID();
                for (int k = 0; k < chains.RowCount; k++)
                {
                    int num6 = (int) matrix2[num4][k];
                    if (num6 < 0x7fffffff)
                    {
                        IVertex vertex7 = (IVertex) vertices[num6];
                        for (int m = dictionary4.get_Item(vertex7); m < chains[k].Count; m++)
                        {
                            this.cg.AddEdge(vertex6, (IVertex) chains[k][m]);
                        }
                    }
                }
            }
            this.graphTransitiveClosures = new VertexVertexDictionary();
            IVertexEnumerator enumerator6 = this.visitedGraph.get_Vertices().GetEnumerator();
            while (enumerator6.MoveNext())
            {
                IVertex vertex8 = enumerator6.get_Current();
                if (!this.graphTransitiveClosures.Contains(vertex8))
                {
                    IVertex vertex9 = tc.AddVertex();
                    this.OnInitTransitiveClosureVertex(new TransitiveClosureVertexEventArgs(vertex8, vertex9));
                    this.graphTransitiveClosures.Add(vertex8, vertex9);
                }
            }
            IVertexCollection vertexs = null;
            IVertexEnumerator enumerator7 = this.cg.get_Vertices().GetEnumerator();
            while (enumerator7.MoveNext())
            {
                IVertex vertex10 = enumerator7.get_Current();
                vertexs = (IVertexCollection) algorithm.SCCVerticesMap[vertex10.get_ID()];
                if (vertexs.Count > 1)
                {
                    IVertexEnumerator enumerator8 = vertexs.GetEnumerator();
                    while (enumerator8.MoveNext())
                    {
                        IVertex vertex11 = enumerator8.get_Current();
                        IVertexEnumerator enumerator9 = vertexs.GetEnumerator();
                        while (enumerator9.MoveNext())
                        {
                            IVertex vertex12 = enumerator9.get_Current();
                            this.OnExamineEdge(tc.AddEdge(this.graphTransitiveClosures.get_Item(vertex11), this.graphTransitiveClosures.get_Item(vertex12)));
                        }
                    }
                }
                IEdgeEnumerator enumerator10 = this.cg.OutEdges(vertex10).GetEnumerator();
                while (enumerator10.MoveNext())
                {
                    IVertex vertex13 = enumerator10.get_Current().get_Target();
                    IVertexEnumerator enumerator11 = ((IVertexCollection) algorithm.SCCVerticesMap[vertex10.get_ID()]).GetEnumerator();
                    while (enumerator11.MoveNext())
                    {
                        IVertex vertex14 = enumerator11.get_Current();
                        IVertexEnumerator enumerator12 = ((IVertexCollection) algorithm.SCCVerticesMap[vertex13.get_ID()]).GetEnumerator();
                        while (enumerator12.MoveNext())
                        {
                            IVertex vertex15 = enumerator12.get_Current();
                            this.OnExamineEdge(tc.AddEdge(this.graphTransitiveClosures.get_Item(vertex14), this.graphTransitiveClosures.get_Item(vertex15)));
                        }
                    }
                }
            }
        }

        private IVertex FirstNotInChain(ArrayList adj_topo_vertices, VertexIntDictionary vertices_in_a_chain)
        {
            if (adj_topo_vertices == null)
            {
                throw new ArgumentNullException("Argument <adj_topo_vertices> in function FirstNotInChain cannot be null");
            }
            if (adj_topo_vertices.Count != 0)
            {
                foreach (IVertex vertex in adj_topo_vertices)
                {
                    if (vertices_in_a_chain.get_Item(vertex) == 0)
                    {
                        return vertex;
                    }
                }
            }
            return null;
        }

        private void LeftUnion(IList u, IList v)
        {
            if (u.Count != v.Count)
            {
                throw new ArgumentException("Exception in LeftUnion. The 2 lists must be of the same size");
            }
            for (int i = 0; i < u.Count; i++)
            {
                if ((u[i] is int) && (v[i] is int))
                {
                    u[i] = Math.Min((int) u[i], (int) v[i]);
                }
            }
        }

        protected void OnExamineEdge(IEdge e)
        {
            if (this.ExamineEdge != null)
            {
                this.ExamineEdge.Invoke(this, new EdgeEventArgs(e));
            }
        }

        protected void OnInitTransitiveClosureVertex(TransitiveClosureVertexEventArgs arg)
        {
            if (this.InitTransitiveClosureVertex != null)
            {
                this.InitTransitiveClosureVertex(this, arg);
            }
        }

        object IAlgorithm.get_VisitedGraph()
        {
            return this.VisitedGraph;
        }

        private void SetChainPositions(VertexListMatrix chains, VertexIntDictionary chain_number, VertexIntDictionary pos_in_chain)
        {
            if (chain_number == null)
            {
                throw new ArgumentNullException("chain_number");
            }
            if (pos_in_chain == null)
            {
                throw new ArgumentNullException("pos_in_chain");
            }
            for (int i = 0; i < chains.RowCount; i++)
            {
                for (int j = 0; j < chains[i].Count; j++)
                {
                    IVertex vertex = (IVertex) chains[i][j];
                    if (!chain_number.ContainsKey(vertex))
                    {
                        chain_number.Add(vertex, i);
                    }
                    if (!pos_in_chain.ContainsKey(vertex))
                    {
                        pos_in_chain.Add(vertex, j);
                    }
                }
            }
        }

        private ArrayList TopoSortAdjVertices(IVertex v, IIncidenceGraph g, VertexIntDictionary topo_ordering)
        {
            IEdgeEnumerator enumerator = g.OutEdges(v).GetEnumerator();
            bool flag = false;
            ArrayList list = new ArrayList();
            while (enumerator.MoveNext())
            {
                flag = true;
                list.Add(enumerator.get_Current().get_Target());
            }
            if (flag)
            {
                CompareTopo topo = new CompareTopo(topo_ordering);
                SwapTopo topo2 = new SwapTopo();
                new QuickSorter(topo, topo2).Sort(list);
            }
            return list;
        }

        public VertexVertexDictionary OrigToTCVertexMap
        {
            get
            {
                return this.graphTransitiveClosures;
            }
        }

        public IVertexListGraph VisitedGraph
        {
            get
            {
                return this.visitedGraph;
            }
        }

        internal class CompareTopo : IComparer
        {
            private VertexIntDictionary m_vid;

            public CompareTopo(VertexIntDictionary topological_ordering)
            {
                this.m_vid = topological_ordering;
            }

            public int Compare(object x, object y)
            {
                IVertex vertex = (IVertex) x;
                IVertex vertex2 = (IVertex) y;
                return ((this.m_vid.get_Item(vertex) < this.m_vid.get_Item(vertex2)) ? -1 : ((this.m_vid.get_Item(vertex) == this.m_vid.get_Item(vertex2)) ? 0 : 1));
            }
        }

        internal class SwapTopo : ISwap
        {
            public void Swap(IList lst, int lindex, int rindex)
            {
                object obj2 = lst[lindex];
                lst[lindex] = lst[rindex];
                lst[rindex] = obj2;
            }
        }

        internal class VertexListMatrix : CollectionBase
        {
            public VertexListMatrix()
            {
            }

            public VertexListMatrix(int create_num_of_rows)
            {
                for (int i = 0; i < create_num_of_rows; i++)
                {
                    base.List.Add(new ArrayList());
                }
            }

            public VertexListMatrix(int create_num_of_rows, int create_num_of_columns)
            {
                for (int i = 0; i < create_num_of_rows; i++)
                {
                    IVertex[] vertexArray = new IVertex[create_num_of_columns];
                    base.List.Add(vertexArray);
                }
            }

            public virtual int AddRow()
            {
                return base.List.Add(new ArrayList());
            }

            public virtual int AddRow(IVertex vertex)
            {
                if (vertex == null)
                {
                    throw new ArgumentException("The <vertex> argument of the AddRow method overload cannot be null");
                }
                ArrayList list = new ArrayList();
                list.Add(vertex);
                return base.List.Add(list);
            }

            public virtual int AddRow(IList array_or_list)
            {
                if (array_or_list == null)
                {
                    throw new ArgumentException("The <array_or_list> argument of the AddRow method overload cannot be null");
                }
                if ((array_or_list.Count != 0) && !(array_or_list[0] is IVertex))
                {
                    throw new Exception("The inner ArrayList in VertexListMatrix must contain IVertex elements only");
                }
                return base.List.Add(array_or_list);
            }

            public void CreateObjectMatrix(int num_of_rows, int num_of_columns, object init_value)
            {
                object[] objArray = null;
                for (int i = 0; i < num_of_rows; i++)
                {
                    objArray = new object[num_of_columns];
                    for (int j = 0; j < num_of_columns; j++)
                    {
                        objArray[j] = init_value;
                    }
                    base.List.Add(objArray);
                }
            }

            private int FetchIndex(IVertex v)
            {
                for (int i = 0; i < base.List.Count; i++)
                {
                    IList list = (IList) base.List[i];
                    if ((list.Count > 0) && (((IVertex) list[0]).get_ID() == v.get_ID()))
                    {
                        return i;
                    }
                }
                return -1;
            }

            public IEnumerator GetEnumerator()
            {
                return new VertexListEnumerator(this);
            }

            public virtual IList RemoveAt(IVertex v)
            {
                int index = this.FetchIndex(v);
                if (index == -1)
                {
                    throw new ArgumentException("The vertex supplied to the RemoveAt overload was not found");
                }
                return this.RemoveAt(index);
            }

            public virtual IList RemoveAt(int index)
            {
                IList list = (IList) base.List[index];
                base.List.RemoveAt(index);
                return list;
            }

            public virtual IList this[int index]
            {
                get
                {
                    return (IList) base.List[index];
                }
                set
                {
                    base.List[index] = value;
                }
            }

            public virtual IList this[IVertex v]
            {
                get
                {
                    int num = this.FetchIndex(v);
                    if (num == -1)
                    {
                        throw new ArgumentException("The vertex supplied to the indexer was not found");
                    }
                    return (IList) base.List[num];
                }
                set
                {
                    int num = this.FetchIndex(v);
                    if (num == -1)
                    {
                        throw new ArgumentException("The vertex supplied to the indexer was not found");
                    }
                    base.List[num] = value;
                }
            }

            public int MaxColumnCount
            {
                get
                {
                    int num = 0;
                    foreach (object obj2 in base.List)
                    {
                        int count = ((IList) obj2).Count;
                        num = (count > num) ? count : num;
                    }
                    return num;
                }
            }

            public int RowCount
            {
                get
                {
                    return base.Count;
                }
            }

            internal class VertexListEnumerator : IEnumerator
            {
                private IEnumerator it;

                public VertexListEnumerator(TransitiveClosureAlgorithm.VertexListMatrix arg)
                {
                    this.it = arg.GetEnumerator();
                }

                public bool MoveNext()
                {
                    return this.it.MoveNext();
                }

                public void Reset()
                {
                    this.it.Reset();
                }

                public IList Current
                {
                    get
                    {
                        return (IList) this.it.Current;
                    }
                }

                object IEnumerator.Current
                {
                    get
                    {
                        return this.it.Current;
                    }
                }
            }
        }
    }
}

