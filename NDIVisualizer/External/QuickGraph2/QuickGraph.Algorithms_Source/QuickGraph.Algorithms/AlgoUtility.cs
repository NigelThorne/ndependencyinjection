namespace QuickGraph.Algorithms
{
    using QuickGraph.Algorithms.Search;
    using QuickGraph.Algorithms.Visitors;
    using QuickGraph.Collections;
    using QuickGraph.Concepts;
    using QuickGraph.Concepts.Collections;
    using QuickGraph.Concepts.Traversals;
    using QuickGraph.Exceptions;
    using QuickGraph.Predicates;
    using System;
    using System.Collections;

    public sealed class AlgoUtility
    {
        private AlgoUtility()
        {
        }

        public static void CheckAcyclic(IVertexListGraph g)
        {
            if (g == null)
            {
                throw new ArgumentNullException("g");
            }
            DepthFirstSearchAlgorithm algorithm = new DepthFirstSearchAlgorithm(g);
            algorithm.BackEdge += new EdgeEventHandler(null, (IntPtr) dfs_BackEdge);
            algorithm.Compute();
        }

        public static void CheckAcyclic(IVertexListGraph g, IVertex root)
        {
            if (g == null)
            {
                throw new ArgumentNullException("g");
            }
            if (root == null)
            {
                throw new ArgumentNullException("root");
            }
            DepthFirstSearchAlgorithm algorithm = new DepthFirstSearchAlgorithm(g);
            algorithm.BackEdge += new EdgeEventHandler(null, (IntPtr) dfs_BackEdge);
            algorithm.Initialize();
            algorithm.Visit(root, 0);
            algorithm.Compute();
        }

        public static int ConnectedComponents(IVertexListGraph g, VertexIntDictionary components)
        {
            ConnectedComponentsAlgorithm algorithm = new ConnectedComponentsAlgorithm(g, components);
            return algorithm.Compute();
        }

        internal static void dfs_BackEdge(object sender, EdgeEventArgs e)
        {
            throw new NonAcyclicGraphException();
        }

        public static bool IsChild(IVertex parent, IVertex child, VertexVertexDictionary predecessors)
        {
            object obj2 = predecessors.get_Item(child);
            if ((obj2 == null) || (obj2 == child))
            {
                return false;
            }
            return ((obj2 == parent) || IsChild(parent, (IVertex) obj2, predecessors));
        }

        public static bool IsInEdgeSet(IEdgeListGraph g, IEdge e)
        {
            if (g == null)
            {
                throw new ArgumentNullException("g");
            }
            if (e == null)
            {
                throw new ArgumentNullException("e");
            }
            IEdgeEnumerator enumerator = g.get_Edges().GetEnumerator();
            while (enumerator.MoveNext())
            {
                if (enumerator.get_Current() == e)
                {
                    return true;
                }
            }
            return false;
        }

        public static bool IsInEdgeSet(IEdgeListGraph g, IVertex source, IVertex target)
        {
            if (g == null)
            {
                throw new ArgumentNullException("g");
            }
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }
            if (target == null)
            {
                throw new ArgumentNullException("target");
            }
            IEdgeEnumerator enumerator = g.get_Edges().GetEnumerator();
            while (enumerator.MoveNext())
            {
                IEdge edge = enumerator.get_Current();
                if (g.get_IsDirected())
                {
                    if ((edge.get_Source() == source) && (edge.get_Target() == target))
                    {
                        return true;
                    }
                    if (((edge.get_Source() == source) && (edge.get_Target() == target)) || ((edge.get_Source() == target) && (edge.get_Target() == source)))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public static bool IsInVertexSet(IVertexListGraph g, IVertex v)
        {
            if (g == null)
            {
                throw new ArgumentNullException("g");
            }
            if (v == null)
            {
                throw new ArgumentNullException("v");
            }
            IVertexEnumerator enumerator = g.get_Vertices().GetEnumerator();
            while (enumerator.MoveNext())
            {
                if (enumerator.get_Current() == v)
                {
                    return true;
                }
            }
            return false;
        }

        public static bool IsReachable(IVertex source, IVertex target, IVertexListGraph g)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }
            if (target == null)
            {
                throw new ArgumentNullException("target");
            }
            if (g == null)
            {
                throw new ArgumentNullException("g");
            }
            DepthFirstSearchAlgorithm algorithm = new DepthFirstSearchAlgorithm(g);
            algorithm.Compute(source);
            return (algorithm.Colors.get_Item(target) != 0);
        }

        public static bool IsSelfLoop(IEdge e)
        {
            if (e == null)
            {
                throw new ArgumentNullException("e");
            }
            return (e.get_Source() == e.get_Target());
        }

        public static VertexCollection OddVertices(IVertexAndEdgeListGraph g)
        {
            if (g == null)
            {
                throw new ArgumentNullException("g");
            }
            VertexIntDictionary dictionary = new VertexIntDictionary();
            IVertexEnumerator enumerator = g.get_Vertices().GetEnumerator();
            while (enumerator.MoveNext())
            {
                IVertex vertex = enumerator.get_Current();
                dictionary.set_Item(vertex, 0);
            }
            IEdgeEnumerator enumerator2 = g.get_Edges().GetEnumerator();
            while (enumerator2.MoveNext())
            {
                VertexIntDictionary dictionary2;
                IVertex vertex2;
                VertexIntDictionary dictionary3;
                IVertex vertex3;
                IEdge edge = enumerator2.get_Current();
                (dictionary2 = dictionary).set_Item(vertex2 = edge.get_Source(), dictionary2.get_Item(vertex2) + 1);
                (dictionary3 = dictionary).set_Item(vertex3 = edge.get_Target(), dictionary3.get_Item(vertex3) - 1);
            }
            VertexCollection vertexs = new VertexCollection();
            IDictionaryEnumerator enumerator3 = dictionary.GetEnumerator();
            while (enumerator3.MoveNext())
            {
                DictionaryEntry current = (DictionaryEntry) enumerator3.Current;
                if ((((int) current.Value) % 2) != 0)
                {
                    vertexs.Add((IVertex) current.Key);
                }
            }
            return vertexs;
        }

        public static IVertex Opposite(IEdge e, IVertex v)
        {
            if (e == null)
            {
                throw new ArgumentNullException("e");
            }
            if (e.get_Source() == v)
            {
                return e.get_Target();
            }
            if (e.get_Target() != v)
            {
                throw new VertexNotConnectedByEdgeException();
            }
            return e.get_Source();
        }

        public static IVertexEnumerable Sinks(IVertexListGraph g)
        {
            return new FilteredVertexEnumerable(g.get_Vertices(), new SinkVertexPredicate(g));
        }

        public static IVertexEnumerable Sinks(IVertexListGraph g, IVertex root)
        {
            if (g == null)
            {
                throw new ArgumentNullException("g");
            }
            if (root == null)
            {
                throw new ArgumentNullException("root");
            }
            DepthFirstSearchAlgorithm algorithm = new DepthFirstSearchAlgorithm(g);
            SinkRecorderVisitor vis = new SinkRecorderVisitor(g);
            algorithm.RegisterVertexColorizerHandlers(vis);
            algorithm.Initialize();
            algorithm.Visit(root, 0);
            return vis.Sinks;
        }

        public static IVertexEnumerable Sources(IBidirectionalVertexListGraph g)
        {
            return new FilteredVertexEnumerable(g.get_Vertices(), new SourceVertexPredicate(g));
        }

        public static int StrongComponents(IVertexListGraph g, VertexIntDictionary components)
        {
            StrongComponentsAlgorithm algorithm = new StrongComponentsAlgorithm(g, components);
            return algorithm.Compute();
        }

        public static void TopologicalSort(IVertexListGraph g, IList vertices)
        {
            if (g == null)
            {
                throw new ArgumentNullException("g");
            }
            if (vertices == null)
            {
                throw new ArgumentNullException("vertices");
            }
            vertices.Clear();
            new TopologicalSortAlgorithm(g, vertices).Compute();
        }
    }
}

