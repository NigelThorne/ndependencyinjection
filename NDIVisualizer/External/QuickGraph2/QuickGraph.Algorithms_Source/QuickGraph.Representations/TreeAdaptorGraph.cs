namespace QuickGraph.Representations
{
    using QuickGraph.Collections;
    using QuickGraph.Concepts;
    using QuickGraph.Concepts.Collections;
    using QuickGraph.Concepts.Traversals;
    using QuickGraph.Exceptions;
    using System;

    public class TreeAdaptorGraph : ITreeGraph
    {
        private IBidirectionalGraph wrapped;

        public TreeAdaptorGraph(IBidirectionalGraph wrapped)
        {
            if (wrapped == null)
            {
                throw new ArgumentNullException("wrapped");
            }
            this.wrapped = wrapped;
        }

        public IVertexEnumerable ChildVertices(IVertex v)
        {
            if (v == null)
            {
                throw new ArgumentNullException("v");
            }
            return new TargetVertexEnumerable(this.Wrapped.OutEdges(v));
        }

        public IVertex FirstChild(IVertex v)
        {
            if (v == null)
            {
                throw new ArgumentNullException("v");
            }
            return Traversal.FirstTargetVertex(this.Wrapped.OutEdges(v));
        }

        public bool HasChildVertices(IVertex v)
        {
            if (v == null)
            {
                throw new ArgumentNullException("v");
            }
            return this.Wrapped.OutEdgesEmpty(v);
        }

        public IVertex LastChild(IVertex v)
        {
            if (v == null)
            {
                throw new ArgumentNullException("v");
            }
            return Traversal.LastTargetVertex(this.Wrapped.OutEdges(v));
        }

        public IVertex ParentVertex(IVertex v)
        {
            if (v == null)
            {
                throw new ArgumentNullException("v");
            }
            if (this.Wrapped.InDegree(v) > 1)
            {
                throw new MultipleInEdgeException(v.get_ID().ToString());
            }
            return Traversal.FirstSourceVertex(this.Wrapped.InEdges(v));
        }

        protected IBidirectionalGraph Wrapped
        {
            get
            {
                return this.wrapped;
            }
        }
    }
}

