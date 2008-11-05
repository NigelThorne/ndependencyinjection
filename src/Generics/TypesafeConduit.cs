//Copyright (c) 2008 Nigel Thorne
using NDependencyInjection.interfaces;


namespace NDependencyInjection.Generics
{
    public class TypeSafeConduit<T> : ITypeSafeConduit<T>
    {
        private readonly IConduit conduit;

        public TypeSafeConduit()
        {
//>>>            conduit = new Conduit(typeof (T));
            conduit = new BufferingConduit(typeof(T));
        }

        public T Proxy
        {
            get { return (T) conduit.Proxy; }
        }

        public void SetTarget(T target)
        {
            conduit.SetTarget(target);
        }
    }
}