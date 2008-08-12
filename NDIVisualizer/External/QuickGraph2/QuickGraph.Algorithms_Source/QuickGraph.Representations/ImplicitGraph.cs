namespace QuickGraph.Representations
{
    using QuickGraph.Concepts;
    using QuickGraph.Concepts.Collections;
    using QuickGraph.Concepts.Traversals;
    using System;
    using System.Collections;

    public abstract class ImplicitGraph : IImplicitGraph, IGraph
    {
        private bool allowParallelEdges = true;

        protected ImplicitGraph()
        {
        }

        public bool ContainsEdgeEdges(IVertex v)
        {
            if (v == null)
            {
                throw new ArgumentNullException("v");
            }
            return (this.Transitions(v).Count > 0);
        }

        public int OutDegree(IVertex v)
        {
            if (v == null)
            {
                throw new ArgumentNullException("v");
            }
            return this.Transitions(v).Count;
        }

        public IEdgeEnumerable OutEdges(IVertex v)
        {
            if (v == null)
            {
                throw new ArgumentNullException("v");
            }
            return new ImplicitEdgeEnumerable(this, v);
        }

        public bool OutEdgesEmpty(IVertex v)
        {
            return (this.OutDegree(v) == 0);
        }

        public abstract TransitionDelegateCollection Transitions(IVertex v);

        public virtual bool AllowParallelEdges
        {
            get
            {
                return this.allowParallelEdges;
            }
        }

        public virtual bool IsDirected
        {
            get
            {
                return true;
            }
        }

        private class ImplicitEdgeEnumerable : IEdgeEnumerable, IEnumerable
        {
            private ImplicitGraph graph;
            private IVertex vertex;

            public ImplicitEdgeEnumerable(ImplicitGraph graph, IVertex vertex)
            {
                this.graph = graph;
                this.vertex = vertex;
            }

            public IEdgeEnumerator GetEnumerator()
            {
                return new ImplicitGraph.ImplicitEdgeEnumerator(this.graph, this.vertex);
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return this.GetEnumerator();
            }
        }

        private class ImplicitEdgeEnumerator : IEdgeEnumerator, IEnumerator
        {
            private IEdge current = null;
            private IEnumerator transitions = null;
            private IVertex vertex;

            public ImplicitEdgeEnumerator(ImplicitGraph graph, IVertex vertex)
            {
                this.vertex = vertex;
                this.transitions = graph.Transitions(vertex).GetEnumerator();
            }

            public bool MoveNext()
            {
                if (!this.transitions.MoveNext())
                {
                    this.current = null;
                    return false;
                }
                TransitionDelegate current = (TransitionDelegate) this.transitions.Current;
                this.current = (IEdge) current.DynamicInvoke(new object[] { this.vertex });
                return true;
            }

            public void Reset()
            {
                this.transitions.Reset();
                this.current = null;
            }

            public IEdge Current
            {
                get
                {
                    return this.current;
                }
            }

            object IEnumerator.Current
            {
                get
                {
                    return this.Current;
                }
            }
        }
    }
}

