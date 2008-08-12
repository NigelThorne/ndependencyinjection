namespace QuickGraph.Representations
{
    using QuickGraph.Algorithms.Search;
    using QuickGraph.Algorithms.Visitors;
    using QuickGraph.Collections;
    using QuickGraph.Concepts;
    using QuickGraph.Concepts.Serialization;
    using QuickGraph.Concepts.Traversals;
    using QuickGraph.Exceptions;
    using System;

    public sealed class Representation
    {
        private Representation()
        {
        }

        public static void CloneOutVertexTree(IVertexListGraph g, ISerializableVertexAndEdgeListGraph sub, IVertex v, int maxDepth)
        {
            if (g == null)
            {
                throw new ArgumentNullException("g");
            }
            if (sub == null)
            {
                throw new ArgumentNullException("sub");
            }
            if (v == null)
            {
                throw new ArgumentNullException("v");
            }
            DepthFirstSearchAlgorithm algorithm = new DepthFirstSearchAlgorithm(g);
            PopulatorVisitor visitor = new PopulatorVisitor(sub);
            algorithm.StartVertex += new VertexEventHandler(visitor, (IntPtr) this.StartVertex);
            algorithm.TreeEdge += new EdgeEventHandler(visitor, (IntPtr) this.TreeEdge);
            algorithm.MaxDepth = maxDepth;
            algorithm.Initialize();
            algorithm.Visit(v, 0);
        }

        internal static void dfs_BackEdge(object sender, EdgeEventArgs e)
        {
            throw new NonAcyclicGraphException();
        }

        public static EdgeCollection InEdgeTree(IBidirectionalVertexAndEdgeListGraph g, IEdge e, int maxDepth)
        {
            if (g == null)
            {
                throw new ArgumentNullException("g");
            }
            if (e == null)
            {
                throw new ArgumentNullException("e");
            }
            EdgeHeightFirstSearchAlgorithm algorithm = new EdgeHeightFirstSearchAlgorithm(g);
            algorithm.BackEdge += new EdgeEventHandler(null, (IntPtr) dfs_BackEdge);
            EdgeRecorderVisitor visitor = new EdgeRecorderVisitor();
            visitor.Edges.Add(e);
            algorithm.DiscoverTreeEdge += new EdgeEdgeEventHandler(visitor, (IntPtr) this.RecordTarget);
            algorithm.MaxDepth = maxDepth;
            algorithm.Initialize();
            algorithm.Visit(e, 0);
            return visitor.Edges;
        }

        public static VertexCollection InVertexTree(IBidirectionalVertexAndEdgeListGraph g, IVertex v, int maxDepth)
        {
            if (g == null)
            {
                throw new ArgumentNullException("g");
            }
            if (v == null)
            {
                throw new ArgumentNullException("v");
            }
            HeightFirstSearchAlgorithm algorithm = new HeightFirstSearchAlgorithm(g);
            algorithm.BackEdge += new EdgeEventHandler(null, (IntPtr) dfs_BackEdge);
            VertexRecorderVisitor visitor = new VertexRecorderVisitor();
            visitor.Vertices.Add(v);
            algorithm.TreeEdge += new EdgeEventHandler(visitor, (IntPtr) this.RecordTarget);
            algorithm.MaxDepth = maxDepth;
            algorithm.Initialize();
            algorithm.Visit(v, 0);
            return visitor.Vertices;
        }

        public static EdgeCollection OutEdgeTree(IEdgeListAndIncidenceGraph g, IEdge e, int maxDepth)
        {
            if (g == null)
            {
                throw new ArgumentNullException("g");
            }
            if (e == null)
            {
                throw new ArgumentNullException("e");
            }
            EdgeDepthFirstSearchAlgorithm algorithm = new EdgeDepthFirstSearchAlgorithm(g);
            algorithm.BackEdge += new EdgeEventHandler(null, (IntPtr) dfs_BackEdge);
            EdgeRecorderVisitor visitor = new EdgeRecorderVisitor();
            visitor.Edges.Add(e);
            algorithm.DiscoverTreeEdge += new EdgeEdgeEventHandler(visitor, (IntPtr) this.RecordTarget);
            algorithm.MaxDepth = maxDepth;
            algorithm.Initialize();
            algorithm.Visit(e, 0);
            return visitor.Edges;
        }

        public static VertexCollection OutVertexTree(IVertexListGraph g, IVertex v, int maxDepth)
        {
            if (g == null)
            {
                throw new ArgumentNullException("g");
            }
            if (v == null)
            {
                throw new ArgumentNullException("v");
            }
            DepthFirstSearchAlgorithm algorithm = new DepthFirstSearchAlgorithm(g);
            algorithm.BackEdge += new EdgeEventHandler(null, (IntPtr) dfs_BackEdge);
            VertexRecorderVisitor visitor = new VertexRecorderVisitor();
            visitor.Vertices.Add(v);
            algorithm.TreeEdge += new EdgeEventHandler(visitor, (IntPtr) this.RecordTarget);
            algorithm.MaxDepth = maxDepth;
            algorithm.Initialize();
            algorithm.Visit(v, 0);
            return visitor.Vertices;
        }
    }
}

