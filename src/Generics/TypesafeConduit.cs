#region usings

using NDependencyInjection.interfaces;

#endregion

namespace NDependencyInjection.Generics
{
    public class TypeSafeConduit<T> : ITypeSafeConduit<T>
    {
        private readonly IConduit conduit;

        public TypeSafeConduit ( )
        {
            conduit = new Conduit ( typeof (T) );
        }

        public T Proxy => (T) conduit.Proxy;

        public void SetTarget ( T target )
        {
            conduit.SetTarget ( target );
        }
    }
}