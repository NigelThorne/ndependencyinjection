namespace QuickGraph.Representations
{
    using System;
    using System.Collections;

    public class TransitionDelegateCollection : CollectionBase
    {
        public void Add(TransitionDelegate transition)
        {
            if (transition == null)
            {
                throw new ArgumentNullException("transition");
            }
            base.List.Add(transition);
        }

        public bool Contains(TransitionDelegate transition)
        {
            if (transition == null)
            {
                throw new ArgumentNullException("transition");
            }
            return base.List.Contains(transition);
        }

        public void Remove(TransitionDelegate transition)
        {
            if (transition == null)
            {
                throw new ArgumentNullException("transition");
            }
            base.List.Remove(transition);
        }
    }
}

