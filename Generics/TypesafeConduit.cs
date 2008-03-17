using NDependencyInjection.interfaces;


namespace NDependencyInjection.Generics
{
    public class TypeSafeConduit<T> : ITypeSafeConduit<T>
    {
        private readonly Conduit conduit;

        public TypeSafeConduit()
        {
            conduit = new Conduit(typeof (T));
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