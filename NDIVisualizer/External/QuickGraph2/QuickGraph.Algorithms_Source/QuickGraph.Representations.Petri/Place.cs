namespace QuickGraph.Representations.Petri
{
    using QuickGraph;
    using QuickGraph.Concepts;
    using QuickGraph.Concepts.Petri;
    using QuickGraph.Concepts.Petri.Collections;
    using QuickGraph.Representations.Petri.Collections;
    using System;
    using System.IO;

    public class Place : Vertex, IPlace, IPetriVertex, IVertex, IComparable
    {
        private TokenCollection marking;
        private string name;

        public Place(int id, string name) : base(id)
        {
            this.marking = new TokenCollection();
            this.name = name;
        }

        public override string ToString()
        {
            return string.Format("P({0}|{1})", this.name, this.marking.Count);
        }

        public string ToStringWithMarking()
        {
            StringWriter writer = new StringWriter();
            writer.WriteLine(this.ToString());
            foreach (object obj2 in this.marking)
            {
                writer.WriteLine("\t{0}", obj2.GetType().Name);
            }
            return writer.ToString();
        }

        public ITokenCollection Marking
        {
            get
            {
                return this.marking;
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

