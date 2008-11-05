using System;
using NDependencyInjection.Generics;
using NDependencyInjection.interfaces;
using IServiceProvider=NDependencyInjection.interfaces.IServiceProvider;


namespace NDependencyInjection.Providers
{
    public class MesssageBroadcasterProvider<EventsInterface> : IServiceProvider
    {
        private readonly IServiceProvider provider;

        public MesssageBroadcasterProvider()
        {
            provider = new BroadcasterProvider<EventsInterface, TypeSafeBroadcaster<EventsInterface>>();
        }

        public object GetService(Type serviceType, Type interfaceType, IServiceLocator context)
        {
            return provider.GetService(serviceType, interfaceType, context);
        }

        public void AddMapping(Type serviceType)
        {
            provider.AddMapping(serviceType);
        }
    }
}