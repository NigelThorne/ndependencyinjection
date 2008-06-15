//Copyright (c) 2008 Nigel Thorne
using System;
using NDependencyInjection.Generics;
using NDependencyInjection.interfaces;
using IServiceProvider=NDependencyInjection.interfaces.IServiceProvider;


namespace NDependencyInjection.Providers
{
    public class BroadcasterProvider<EventsInterface> : IServiceProvider
    {
        private IBroadcaster<EventsInterface> broadcaster;

        public object GetService(Type serviceType, Type interfaceType, IServiceLocator context)
        {
            if (!typeof (EventsInterface).IsAssignableFrom(interfaceType) 
                && !typeof (IBroadcaster<EventsInterface>).IsAssignableFrom(interfaceType))
                throw new InvalidProgramException(
                    string.Format("Broadcaster supports type {0} not {1}", typeof (EventsInterface), interfaceType));

            if (broadcaster == null)
            {
                broadcaster = new TypeSafeBroadcaster<EventsInterface>();
            }

            if (serviceType == typeof(IBroadcaster<EventsInterface>))
            {
                return broadcaster;
            }
            return broadcaster.Listener;
        }

        public void AddMapping(Type serviceType)
        {
            
        }
    }
}