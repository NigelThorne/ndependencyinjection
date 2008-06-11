//Copyright (c) 2008 Nigel Thorne
using System;
using System.Collections.Generic;
using System.Reflection;
using LinFu.DynamicProxy;
using NDependencyInjection.interfaces;


namespace NDependencyInjection.Generics
{
    public class TypeSafeBroadcaster<ListenerType> : IBroadcaster<ListenerType>, IInterceptor
    {
        private readonly ListenerType listener;
        private readonly List<ListenerType> listeners = new List<ListenerType>();

        public TypeSafeBroadcaster()
        {
            ProxyFactory factory = new ProxyFactory();
            listener = factory.CreateProxy<ListenerType>(this);
        }

        object IInterceptor.Intercept(InvocationInfo info)
        {
            try
            {
                if (info.TargetMethod.ReturnType != typeof (void))
                    throw new InvalidOperationException("You can only broadcast void methods.");

                ListenerType[] copyOfListeners = listeners.ToArray();
                foreach (ListenerType childListeners in copyOfListeners)
                {
                    info.TargetMethod.Invoke(childListeners, info.Arguments);
                }
                return null;
            }
            catch (TargetInvocationException ex)
            {
                throw ex.InnerException;
            }
        }

        public void AddListeners(params ListenerType[] newListerners)
        {
            listeners.AddRange(newListerners);
        }

        public void RemoveListeners(params ListenerType[] oldListeners)
        {
            foreach (ListenerType item in oldListeners)
            {
                listeners.Remove(item);
            }
        }

        public ListenerType Listener
        {
            get { return listener; }
        }
    }
}