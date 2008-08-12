namespace QuickGraph.Algorithms
{
    using QuickGraph.Collections;
    using QuickGraph.Concepts;
    using QuickGraph.Concepts.Algorithms;
    using QuickGraph.Concepts.Collections;
    using QuickGraph.Concepts.Traversals;
    using QuickGraph.Exceptions;
    using System;
    using System.Runtime.CompilerServices;

    public class SourceFirstTopologicalSortAlgorithm : IAlgorithm
    {
        private PriorithizedVertexBuffer heap;
        private VertexDoubleDictionary inDegrees = new VertexDoubleDictionary();
        private VertexCollection sortedVertices = new VertexCollection();
        private IVertexAndEdgeListGraph visitedGraph;

        public event VertexEventHandler AddVertex;

        public SourceFirstTopologicalSortAlgorithm(IVertexAndEdgeListGraph visitedGraph)
        {
            this.visitedGraph = visitedGraph;
            this.heap = new PriorithizedVertexBuffer(this.inDegrees);
        }

        public void Compute()
        {
            this.InitializeInDegrees();
            while (this.heap.get_Count() != 0)
            {
                IVertex v = this.heap.Pop();
                if (this.inDegrees.get_Item(v) != 0.0)
                {
                    throw new NonAcyclicGraphException();
                }
                this.sortedVertices.Add(v);
                this.OnAddVertex(v);
                IEdgeEnumerator enumerator = this.VisitedGraph.OutEdges(v).GetEnumerator();
                while (enumerator.MoveNext())
                {
                    IEdge edge = enumerator.get_Current();
                    if (edge.get_Source() != edge.get_Target())
                    {
                        VertexDoubleDictionary dictionary;
                        IVertex vertex2;
                        (dictionary = this.inDegrees).set_Item(vertex2 = edge.get_Target(), dictionary.get_Item(vertex2) - 1.0);
                        if (this.inDegrees.get_Item(edge.get_Target()) < 0.0)
                        {
                            throw new InvalidOperationException("InDegree is negative, and cannot be");
                        }
                        this.heap.Update(edge.get_Target());
                    }
                }
            }
        }

        protected virtual void InitializeInDegrees()
        {
            IVertexEnumerator enumerator = this.VisitedGraph.get_Vertices().GetEnumerator();
            while (enumerator.MoveNext())
            {
                IVertex vertex = enumerator.get_Current();
                this.inDegrees.Add(vertex, 0.0);
                this.heap.Push(vertex);
            }
            IEdgeEnumerator enumerator2 = this.VisitedGraph.get_Edges().GetEnumerator();
            while (enumerator2.MoveNext())
            {
                IEdge edge = enumerator2.get_Current();
                if (edge.get_Source() != edge.get_Target())
                {
                    VertexDoubleDictionary dictionary;
                    IVertex vertex2;
                    (dictionary = this.inDegrees).set_Item(vertex2 = edge.get_Target(), dictionary.get_Item(vertex2) + 1.0);
                }
            }
            this.heap.Sort();
        }

        protected virtual void OnAddVertex(IVertex v)
        {
            if (this.AddVertex != null)
            {
                this.AddVertex.Invoke(this, new VertexEventArgs(v));
            }
        }

        object IAlgorithm.get_VisitedGraph()
        {
            return this.visitedGraph;
        }

        public PriorithizedVertexBuffer Heap
        {
            get
            {
                return this.heap;
            }
        }

        public VertexDoubleDictionary InDegrees
        {
            get
            {
                return this.inDegrees;
            }
        }

        public IVertexCollection SortedVertices
        {
            get
            {
                return this.sortedVertices;
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

