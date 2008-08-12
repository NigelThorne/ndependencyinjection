namespace QuickGraph.Algorithms.Layout
{
    using QuickGraph.Collections;
    using QuickGraph.Concepts.Algorithms;
    using QuickGraph.Concepts.Collections;
    using QuickGraph.Concepts.Traversals;
    using System;
    using System.Drawing;
    using System.Runtime.CompilerServices;

    public abstract class LayoutAlgorithm : ILayoutAlgorithm, IAlgorithm
    {
        private bool computationAbortion;
        private float edgeLength;
        private Size layoutSize;
        private IVertexPointFDictionary positions;
        private IVertexAndEdgeListGraph visitedGraph;

        public event EventHandler PostCompute;

        public event EventHandler PreCompute;

        public LayoutAlgorithm(IVertexAndEdgeListGraph visitedGraph)
        {
            this.edgeLength = 20f;
            this.layoutSize = new Size(800, 600);
            this.positions = new VertexPointFDictionary();
            this.computationAbortion = false;
            this.visitedGraph = visitedGraph;
            this.positions = new VertexPointFDictionary();
        }

        public LayoutAlgorithm(IVertexAndEdgeListGraph visitedGraph, IVertexPointFDictionary positions)
        {
            this.edgeLength = 20f;
            this.layoutSize = new Size(800, 600);
            this.positions = new VertexPointFDictionary();
            this.computationAbortion = false;
            this.visitedGraph = visitedGraph;
            this.positions = positions;
        }

        public abstract void Compute();
        protected virtual void OnPostCompute()
        {
            if (this.PostCompute != null)
            {
                this.PostCompute(this, EventArgs.Empty);
            }
        }

        protected virtual void OnPreCompute()
        {
            if (this.PreCompute != null)
            {
                this.PreCompute(this, EventArgs.Empty);
            }
            this.computationAbortion = false;
        }

        object IAlgorithm.get_VisitedGraph()
        {
            return this.VisitedGraph;
        }

        public virtual void RequestComputationAbortion()
        {
            this.computationAbortion = true;
        }

        public virtual void UpdateEdgeLength()
        {
            this.edgeLength = 0.5f * ((float) Math.Sqrt((double) ((this.LayoutSize.Width * this.LayoutSize.Height) / ((float) this.VisitedGraph.get_VerticesCount()))));
        }

        protected bool ComputationAbortion
        {
            get
            {
                return this.computationAbortion;
            }
        }

        public float EdgeLength
        {
            get
            {
                return this.edgeLength;
            }
            set
            {
                this.edgeLength = value;
            }
        }

        public Size LayoutSize
        {
            get
            {
                return this.layoutSize;
            }
            set
            {
                this.layoutSize = value;
            }
        }

        public IVertexPointFDictionary Positions
        {
            get
            {
                return this.positions;
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

