namespace QuickGraph.Representations.Petri
{
    using QuickGraph.Concepts.Petri;
    using System;
    using System.Collections;
    using System.IO;

    public class PetriNet : IMutablePetriNet, IPetriNet
    {
        private ArrayList arcs = new ArrayList();
        private PetriGraph graph = new PetriGraph(true);
        private ArrayList places = new ArrayList();
        private ArrayList transitions = new ArrayList();
        private int version = 0;

        public IArc AddArc(IPlace place, ITransition transition)
        {
            IArc arc = new Arc(this.version++, place, transition);
            this.arcs.Add(arc);
            this.graph.AddEdge(arc);
            return arc;
        }

        public IArc AddArc(ITransition transition, IPlace place)
        {
            IArc arc = new Arc(this.version++, transition, place);
            this.arcs.Add(arc);
            this.graph.AddEdge(transition, place);
            return arc;
        }

        public IPlace AddPlace(string name)
        {
            IPlace place = new Place(this.version++, name);
            this.places.Add(place);
            this.graph.AddVertex(place);
            return place;
        }

        public ITransition AddTransition(string name)
        {
            ITransition transition = new Transition(this.version++, name);
            this.transitions.Add(transition);
            this.graph.AddVertex(transition);
            return transition;
        }

        public override string ToString()
        {
            StringWriter writer = new StringWriter();
            writer.WriteLine("-----------------------------------------------");
            writer.WriteLine("Places ({0})", this.places.Count);
            foreach (Place place in this.places)
            {
                writer.WriteLine("\t{0}", place.ToStringWithMarking());
            }
            writer.WriteLine("Transitions ({0})", this.transitions.Count);
            foreach (ITransition transition in this.transitions)
            {
                writer.WriteLine("\t{0}", transition);
            }
            writer.WriteLine("Arcs");
            foreach (IArc arc in this.arcs)
            {
                writer.WriteLine("\t{0}", arc);
            }
            return writer.ToString();
        }

        public IList Arcs
        {
            get
            {
                return this.arcs;
            }
        }

        public PetriGraph Graph
        {
            get
            {
                return this.graph;
            }
        }

        public IList Places
        {
            get
            {
                return this.places;
            }
        }

        public IList Transitions
        {
            get
            {
                return this.transitions;
            }
        }
    }
}

