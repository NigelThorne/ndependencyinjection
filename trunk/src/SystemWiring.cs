//Copyright (c) 2008 Nigel Thorne
using System;
using NDependencyInjection.Generics;
using NDependencyInjection.interfaces;
using NDependencyInjection.Providers;
using IServiceProvider=NDependencyInjection.interfaces.IServiceProvider;


namespace NDependencyInjection
{
    public class SystemWiring : ISystemWiring
    {
        private readonly IServiceRepository repository;

        public SystemWiring()
            : this(new NullServiceLocator())
        {
        }

        // Should this be removed?
        public SystemWiring(IServiceLocator parent)
            : this(new ServiceRepository(parent))
        {
        }

        public SystemWiring(IServiceRepository repository)
        {
            this.repository = repository;
        }

        public void RegisterServiceProvider<T1>(IServiceProvider provider)
        {
            repository.RegisterServiceProvider<T1>(provider);
            provider.AddMapping(typeof(T1));
        }

        public void RegisterServiceListener<EventsInterface>(IServiceProvider provider)
        {
            TypeResolvingConduit<EventsInterface> resolvingConduit = new TypeResolvingConduit<EventsInterface>(provider);
            GetService<IBroadcaster<EventsInterface>>().AddListeners(resolvingConduit.Proxy);
        }

        public void RegisterBroadcaster<EventsInterface>()
        {
            BroadcasterProvider<EventsInterface> provider = new BroadcasterProvider<EventsInterface>();
            repository.RegisterServiceProvider<IBroadcaster<EventsInterface>>(provider);
            repository.RegisterServiceProvider<EventsInterface>(provider);
        }

        public bool HasService(Type serviceType)
        {
            return repository.HasService(serviceType);
        }

        public IServiceLocator Parent
        {
            get { return repository.Parent; }
        }

        public object GetService(Type serviceType)
        {
            return repository.GetService(serviceType);
        }

        public ISystemWiring CreateSubsystem()
        {
            return new SystemWiring(new ServiceRepository(this));
        }

        private T GetService<T>()
        {
            return (T) GetService(typeof (T));
        }
    }

}
