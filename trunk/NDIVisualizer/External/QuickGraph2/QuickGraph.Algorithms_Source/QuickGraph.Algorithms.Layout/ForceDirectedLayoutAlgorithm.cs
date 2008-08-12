namespace QuickGraph.Algorithms.Layout
{
    using QuickGraph.Collections;
    using QuickGraph.Concepts;
    using QuickGraph.Concepts.Algorithms;
    using QuickGraph.Concepts.Collections;
    using QuickGraph.Concepts.Traversals;
    using System;
    using System.Collections;
    using System.Drawing;
    using System.Runtime.CompilerServices;

    public class ForceDirectedLayoutAlgorithm : LayoutAlgorithm, IForceDirectedLayoutAlgorithm, IIteratedLayoutAlgorithm, ILayoutAlgorithm, IAlgorithm
    {
        private int currentIteration;
        private float heat;
        private float heatDecayRate;
        private int maxIteration;
        private float maxMovement;
        private IPotential potential;
        private VertexPointFDictionary potentials;
        private ILayoutAlgorithm preLayoutAlgorithm;
        private object syncRoot;

        public event EventHandler PostIteration;

        public event EventHandler PreIteration;

        public ForceDirectedLayoutAlgorithm(IVertexAndEdgeListGraph visitedGraph) : base(visitedGraph)
        {
            this.preLayoutAlgorithm = null;
            this.potential = null;
            this.currentIteration = 0;
            this.maxIteration = 500;
            this.potentials = new VertexPointFDictionary();
            this.maxMovement = 50f;
            this.heat = 1f;
            this.heatDecayRate = 0.99f;
            this.syncRoot = null;
            this.preLayoutAlgorithm = new RandomLayoutAlgorithm(visitedGraph, base.Positions);
            this.potential = new DirectedForcePotential(this);
            this.potentials = new VertexPointFDictionary();
        }

        public ForceDirectedLayoutAlgorithm(IVertexAndEdgeListGraph visitedGraph, IPotential potential, ILayoutAlgorithm preLayoutAlgorithm) : base(visitedGraph)
        {
            this.preLayoutAlgorithm = null;
            this.potential = null;
            this.currentIteration = 0;
            this.maxIteration = 500;
            this.potentials = new VertexPointFDictionary();
            this.maxMovement = 50f;
            this.heat = 1f;
            this.heatDecayRate = 0.99f;
            this.syncRoot = null;
            this.preLayoutAlgorithm = preLayoutAlgorithm;
            this.potential = potential;
            this.potentials = new VertexPointFDictionary();
        }

        public override void Compute()
        {
            this.OnPreCompute();
            this.PreLayoutAlgorithm.Compute();
            this.heat = 1f;
            this.currentIteration = 0;
            while ((this.currentIteration < this.MaxIteration) && !base.ComputationAbortion)
            {
                lock (this.SyncRoot)
                {
                    this.Iterate();
                }
                this.heat *= this.heatDecayRate;
                this.currentIteration++;
            }
            this.OnPostCompute();
        }

        public virtual void Iterate()
        {
            this.OnPreIteration();
            IVertexEnumerator enumerator = base.VisitedGraph.get_Vertices().GetEnumerator();
            while (enumerator.MoveNext())
            {
                IVertex vertex = enumerator.get_Current();
                PointF tf2 = new PointF();
                this.potentials.set_Item(vertex, tf2);
            }
            this.Potential.Compute(this.potentials);
            IDictionaryEnumerator enumerator2 = this.potentials.GetEnumerator();
            while (enumerator2.MoveNext())
            {
                DictionaryEntry current = (DictionaryEntry) enumerator2.Current;
                IVertex key = (IVertex) current.Key;
                PointF tf = (PointF) current.Value;
                base.Positions.set_Item(key, PointMath.ScaleSaturate(base.Positions.get_Item(key), this.potentials.get_Item(key), this.heat, this.maxMovement));
            }
            this.OnPostIteration();
        }

        protected virtual void OnPostIteration()
        {
            if (this.PostIteration != null)
            {
                this.PostIteration(this, EventArgs.Empty);
            }
        }

        protected virtual void OnPreIteration()
        {
            if (this.PreIteration != null)
            {
                this.PreIteration(this, EventArgs.Empty);
            }
        }

        public override void UpdateEdgeLength()
        {
            base.UpdateEdgeLength();
            this.PreLayoutAlgorithm.UpdateEdgeLength();
            this.maxMovement = base.EdgeLength / 2f;
        }

        public int CurrentIteration
        {
            get
            {
                return this.currentIteration;
            }
        }

        public int MaxIteration
        {
            get
            {
                return this.maxIteration;
            }
            set
            {
                this.maxIteration = value;
            }
        }

        public IPotential Potential
        {
            get
            {
                return this.potential;
            }
            set
            {
                this.potential = value;
            }
        }

        public ILayoutAlgorithm PreLayoutAlgorithm
        {
            get
            {
                return this.preLayoutAlgorithm;
            }
            set
            {
                this.preLayoutAlgorithm = value;
            }
        }

        public object SyncRoot
        {
            get
            {
                return this.syncRoot;
            }
            set
            {
                this.syncRoot = value;
            }
        }
    }
}

