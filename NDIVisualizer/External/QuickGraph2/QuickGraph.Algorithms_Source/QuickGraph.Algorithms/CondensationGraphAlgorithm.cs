namespace QuickGraph.Algorithms
{
    using QuickGraph.Collections;
    using QuickGraph.Concepts;
    using QuickGraph.Concepts.Algorithms;
    using QuickGraph.Concepts.Collections;
    using QuickGraph.Concepts.MutableTraversals;
    using QuickGraph.Concepts.Traversals;
    using System;
    using System.Collections;
    using System.Runtime.CompilerServices;

    public class CondensationGraphAlgorithm : IAlgorithm
    {
        private VertexIntDictionary components;
        private SortedList sccVertexMap;
        private IVertexListGraph visitedGraph;

        public event CondensationGraphVertexEventHandler InitCondensationGraphVertex;

        public CondensationGraphAlgorithm(IVertexListGraph g)
        {
            if (g == null)
            {
                throw new ArgumentNullException("g");
            }
            this.visitedGraph = g;
            this.components = null;
        }

        private SortedList BuildSCCVertexMap(VertexIntDictionary vSccMap)
        {
            SortedList list = new SortedList();
            VertexCollection vertexs = null;
            IDictionaryEnumerator enumerator = vSccMap.GetEnumerator();
            while (enumerator.MoveNext())
            {
                DictionaryEntry current = (DictionaryEntry) enumerator.Current;
                IVertex key = (IVertex) current.Key;
                int num = (int) current.Value;
                if (list.ContainsKey(num))
                {
                    ((VertexCollection) list[num]).Add(key);
                }
                else
                {
                    vertexs = new VertexCollection();
                    vertexs.Add(key);
                    list.Add(num, vertexs);
                }
            }
            return list;
        }

        public void ClearComponents()
        {
            this.components = null;
            if (this.sccVertexMap != null)
            {
                this.sccVertexMap.Clear();
            }
        }

        internal void ComputeComponents()
        {
            if (this.components == null)
            {
                this.components = new VertexIntDictionary();
            }
            new StrongComponentsAlgorithm(this.VisitedGraph, this.components).Compute();
        }

        public void Create(IMutableVertexAndEdgeListGraph cg)
        {
            if (cg == null)
            {
                throw new ArgumentNullException("cg");
            }
            if (this.components == null)
            {
                this.ComputeComponents();
            }
            this.sccVertexMap = this.BuildSCCVertexMap(this.components);
            VertexCollection vertexs = new VertexCollection();
            IDictionaryEnumerator enumerator = this.sccVertexMap.GetEnumerator();
            while (enumerator.MoveNext())
            {
                IVertex cgVertex = cg.AddVertex();
                this.OnInitCondensationGraphVertex(new CondensationGraphVertexEventArgs(cgVertex, (IVertexCollection) enumerator.Value));
                vertexs.Add(cgVertex);
            }
            for (int i = 0; i < this.sccVertexMap.Keys.Count; i++)
            {
                VertexCollection vertexs2 = new VertexCollection();
                IVertexEnumerator enumerator2 = ((IVertexCollection) this.sccVertexMap[i]).GetEnumerator();
                while (enumerator2.MoveNext())
                {
                    IVertex vertex2 = enumerator2.get_Current();
                    IEdgeEnumerator enumerator3 = this.VisitedGraph.OutEdges(vertex2).GetEnumerator();
                    while (enumerator3.MoveNext())
                    {
                        IVertex vertex3 = enumerator3.get_Current().get_Target();
                        int num2 = this.components.get_Item(vertex3);
                        if (i != num2)
                        {
                            IVertex vertex4 = vertexs.get_Item(num2);
                            if (!vertexs2.Contains(vertex4))
                            {
                                vertexs2.Add(vertex4);
                            }
                        }
                    }
                }
                IVertex vertex5 = vertexs.get_Item(i);
                VertexCollection.Enumerator enumerator4 = vertexs2.GetEnumerator();
                while (enumerator4.MoveNext())
                {
                    IVertex vertex6 = enumerator4.get_Current();
                    cg.AddEdge(vertex5, vertex6);
                }
            }
        }

        protected void OnInitCondensationGraphVertex(CondensationGraphVertexEventArgs arg)
        {
            if (this.InitCondensationGraphVertex != null)
            {
                this.InitCondensationGraphVertex(this, arg);
            }
        }

        object IAlgorithm.get_VisitedGraph()
        {
            return this.VisitedGraph;
        }

        public SortedList SCCVerticesMap
        {
            get
            {
                return this.sccVertexMap;
            }
        }

        public VertexIntDictionary VertexToSCCMap
        {
            get
            {
                return this.components;
            }
        }

        public IVertexListGraph VisitedGraph
        {
            get
            {
                return this.visitedGraph;
            }
        }
    }
}

