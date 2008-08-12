namespace QuickGraph.Representations.Petri
{
    using QuickGraph;
    using QuickGraph.Concepts;
    using QuickGraph.Concepts.Petri;
    using System;

    public class Arc : Edge, IArc, IEdge, IComparable
    {
        private IExpression annotation;
        private bool isInputArc;
        private IPlace place;
        private ITransition transition;

        public Arc(int id, IPlace place, ITransition transition) : base(id, place, transition)
        {
            this.annotation = new IdentityExpression();
            this.place = place;
            this.transition = transition;
            this.isInputArc = true;
        }

        public Arc(int id, ITransition transition, IPlace place) : base(id, transition, place)
        {
            this.annotation = new IdentityExpression();
            this.place = place;
            this.transition = transition;
            this.isInputArc = false;
        }

        public override string ToString()
        {
            if (this.IsInputArc)
            {
                return string.Format("{0} -> {1}", this.place, this.transition);
            }
            return string.Format("{0} -> {1}", this.transition, this.place);
        }

        public IExpression Annotation
        {
            get
            {
                return this.annotation;
            }
            set
            {
                this.annotation = value;
            }
        }

        public bool IsInputArc
        {
            get
            {
                return this.isInputArc;
            }
        }

        public IPlace Place
        {
            get
            {
                return this.place;
            }
        }

        public ITransition Transition
        {
            get
            {
                return this.transition;
            }
        }
    }
}

