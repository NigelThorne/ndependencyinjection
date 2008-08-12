namespace QuickGraph.Representations.Petri
{
    using QuickGraph;
    using QuickGraph.Concepts;
    using QuickGraph.Concepts.Petri;
    using System;

    public class Transition : Vertex, ITransition, IPetriVertex, IVertex, IComparable
    {
        private IConditionExpression condition;
        private string name;

        public Transition(int id, string name) : base(id)
        {
            this.condition = new AllwaysTrueConditionExpression();
            this.name = name;
        }

        public override string ToString()
        {
            return string.Format("T({0})", this.name);
        }

        public IConditionExpression Condition
        {
            get
            {
                return this.condition;
            }
            set
            {
                this.condition = value;
            }
        }

        public string Name
        {
            get
            {
                return this.name;
            }
        }
    }
}

