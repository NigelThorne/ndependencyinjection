using System;
using System.Collections.Generic;
using NDependencyInjection.interfaces;
using IServiceProvider=NDependencyInjection.interfaces.IServiceProvider;


namespace NDependencyInjection
{
    public class SystemWiring : ISystemWiring
    {
        private readonly Dictionary<Type, IBroadcasterProvider> broadcasters =
            new Dictionary<Type, IBroadcasterProvider>();

        private readonly IServiceRepository repository;

        public SystemWiring()
            : this(new ScopedServiceRepository(new NullServiceLocator()))
        {
        }

        [Obsolete("Use the CreateSubsystem syntax")]
        public SystemWiring(IServiceLocator parent)
            : this(new ScopedServiceRepository(parent))
        {
        }

        public SystemWiring(IServiceRepository repository)
        {
            this.repository = repository;
        }

        public void RegisterServiceProvider<T1>(IServiceProvider provider)
        {
            repository.RegisterServiceProvider<T1>(provider);
        }

        public void RegisterServiceListener<EventsInterface>(IServiceProvider provider)
        {
            if (!broadcasters.ContainsKey(typeof (EventsInterface)))
            {
                broadcasters[typeof (EventsInterface)] =
                    new BroadcasterProvider<EventsInterface>();
                repository.RegisterServiceProvider<EventsInterface>(broadcasters[typeof (EventsInterface)]);
            }

            broadcasters[typeof (EventsInterface)].AddListenerProvider(provider);
        }

        public bool HasService(Type serviceType)
        {
            return repository.HasService(serviceType);
        }

        public object GetService(Type serviceType, Type interfaceType)
        {
            return repository.GetService(serviceType, interfaceType);
        }

        public ISystemWiring CreateSubsystem()
        {
            return new SystemWiring(new ScopedServiceRepository(this));
        }
    }
}