namespace QuickGraph.Representations.Petri.Collections
{
    using QuickGraph.Concepts.Petri.Collections;
    using System;
    using System.Collections;

    public class TokenCollection : CollectionBase, ITokenCollection, ICollection, IEnumerable
    {
        public void Add(object token)
        {
            base.List.Add(token);
        }

        public void AddRange(ITokenCollection tokens)
        {
            if (tokens == null)
            {
                throw new ArgumentNullException("tokens");
            }
            foreach (object obj2 in tokens)
            {
                base.List.Add(obj2);
            }
        }

        public bool Contains(object token)
        {
            return base.List.Contains(token);
        }

        public void Remove(object token)
        {
            base.List.Remove(token);
        }

        public void RemoveAll(object token)
        {
            while (base.List.Contains(token))
            {
                base.List.Remove(token);
            }
        }

        public void RemoveRange(ITokenCollection tokens)
        {
            if (tokens == null)
            {
                throw new ArgumentNullException("tokens");
            }
            foreach (object obj2 in tokens)
            {
                base.List.Remove(obj2);
            }
        }
    }
}

