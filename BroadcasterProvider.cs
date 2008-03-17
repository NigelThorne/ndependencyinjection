using System;
using System.Collections.Generic;
using NDependencyInjection.Generics;
using NDependencyInjection.interfaces;
using IServiceProvider=NDependencyInjection.interfaces.IServiceProvider;


namespace NDependencyInjection
{
    public class BroadcasterProvider<EventsInterface> : IBroadcasterProvider
    {
        private readonly List<IServiceProvider> listeners;
        private ITypeSafeBroadcaster<EventsInterface> broadcaster;

        public BroadcasterProvider()
        {
            listeners = new List<IServiceProvider>();
        }

        public object GetService(Type serviceType, Type interfaceType)
        {
            if (!typeof (EventsInterface).IsAssignableFrom(interfaceType))
                throw new InvalidProgramException(
                    string.Format("Broadcaster supports type {0} not {1}", typeof (EventsInterface), interfaceType));

            if (broadcaster == null)
            {
                List<EventsInterface> list = new List<EventsInterface>();
                foreach (IServiceProvider provider in listeners)
                {
                    list.Add((EventsInterface) provider.GetService(serviceType, interfaceType));
                }
                broadcaster = new TypeSafeBroadcaster<EventsInterface>(list.ToArray());
            }
            return broadcaster.Listener;
        }

        public void AddListenerProvider(IServiceProvider listenerProvider)
        {
            if (broadcaster != null)
            {
                broadcaster.AddListeners((EventsInterface) listenerProvider.GetService(typeof(EventsInterface), typeof(EventsInterface)));
            }
            else
            {
                listeners.Add(listenerProvider);
            }
        }
    }
}