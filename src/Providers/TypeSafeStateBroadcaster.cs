using LinFu.DynamicProxy;
using NDependencyInjection.Generics;
using NDependencyInjection.interfaces;


namespace NDependencyInjection.Providers
{
    public class TypeSafeStateBroadcaster<ListenerType> : IBroadcaster<ListenerType>, IInterceptor
    {
        private readonly TypeSafeBroadcaster<ListenerType> broadcaster;
        private InvocationInfo lastBroadcastInfo;
        private bool hasBroadcast;
        private readonly ListenerType listener;

        public TypeSafeStateBroadcaster()
        {
            broadcaster = new TypeSafeBroadcaster<ListenerType>();
            ProxyFactory factory = new ProxyFactory();
            listener = factory.CreateProxy<ListenerType>(this);
        }

        public ListenerType Listener
        {
            get { return listener; }
        }

        public void AddListeners(params ListenerType[] newListerners)
        {
            broadcaster.AddListeners(newListerners);

            if (hasBroadcast)
            {
                ContructingLockManager.AddIntercept((IInterceptor)broadcaster, lastBroadcastInfo);
            }
        }

        public void RemoveListeners(params ListenerType[] oldListeners)
        {
            broadcaster.RemoveListeners(oldListeners);
        }

        object IInterceptor.Intercept(InvocationInfo info)
        {
            hasBroadcast = true;
            lastBroadcastInfo = info;

            ((IInterceptor)broadcaster).Intercept(info);

            return null;
        }
    }
}