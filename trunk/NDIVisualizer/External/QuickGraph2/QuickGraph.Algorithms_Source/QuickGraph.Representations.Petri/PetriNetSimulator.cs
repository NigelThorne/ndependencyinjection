namespace QuickGraph.Representations.Petri
{
    using QuickGraph.Concepts.Petri;
    using QuickGraph.Concepts.Petri.Collections;
    using QuickGraph.Representations.Petri.Collections;
    using System;
    using System.Collections;

    public class PetriNetSimulator
    {
        private IPetriNet net;
        private Hashtable transitionBuffers = new Hashtable();
        private Hashtable transitionEnabled = new Hashtable();

        public PetriNetSimulator(IPetriNet net)
        {
            this.net = net;
        }

        public void Initialize()
        {
            this.transitionBuffers.Clear();
            this.transitionEnabled.Clear();
            foreach (ITransition transition in this.Net.get_Transitions())
            {
                this.transitionBuffers.Add(transition, new TokenCollection());
                this.transitionEnabled.Add(transition, false);
            }
        }

        public void SimulateStep()
        {
            foreach (IArc arc in this.net.get_Arcs())
            {
                if (arc.get_IsInputArc())
                {
                    ITokenCollection tokens = (ITokenCollection) this.transitionBuffers[arc.get_Transition()];
                    ITokenCollection tokens2 = arc.get_Annotation().Eval(arc.get_Place().get_Marking());
                    tokens.AddRange(tokens2);
                }
            }
            foreach (ITransition transition in this.Net.get_Transitions())
            {
                ITokenCollection tokens3 = (ITokenCollection) this.transitionBuffers[transition];
                this.transitionEnabled[transition] = transition.get_Condition().IsEnabled(tokens3);
            }
            foreach (IArc arc2 in this.Net.get_Arcs())
            {
                if ((bool) this.transitionEnabled[arc2.get_Transition()])
                {
                    if (arc2.get_IsInputArc())
                    {
                        ITokenCollection tokens4 = arc2.get_Annotation().Eval(arc2.get_Place().get_Marking());
                        arc2.get_Place().get_Marking().RemoveRange(tokens4);
                    }
                    else
                    {
                        ITokenCollection tokens5 = (ITokenCollection) this.transitionBuffers[arc2.get_Transition()];
                        ITokenCollection tokens6 = arc2.get_Annotation().Eval(tokens5);
                        arc2.get_Place().get_Marking().AddRange(tokens6);
                    }
                }
            }
            foreach (ITransition transition2 in this.Net.get_Transitions())
            {
                ((ITokenCollection) this.transitionBuffers[transition2]).Clear();
                this.transitionEnabled[transition2] = false;
            }
        }

        public IPetriNet Net
        {
            get
            {
                return this.net;
            }
        }
    }
}

