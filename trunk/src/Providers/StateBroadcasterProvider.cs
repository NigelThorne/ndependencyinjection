using System;
using NDependencyInjection.interfaces;
using IServiceProvider=NDependencyInjection.interfaces.IServiceProvider;


namespace NDependencyInjection.Providers
{
    public class StateBroadcasterProvider<EventsInterface> : IServiceProvider
    {
        private readonly IServiceProvider provider;

        public StateBroadcasterProvider()
        {
            provider = new BroadcasterProvider<EventsInterface, TypeSafeStateBroadcaster<EventsInterface>>();
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