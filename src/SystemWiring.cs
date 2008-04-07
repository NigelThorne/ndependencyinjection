//Copyright (c) 2008 Nigel Thorne
using System;
using System.Collections.Generic;
using NDependencyInjection.Generics;
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

        public IServiceProvider GetServiceProvider<T1>()
        {
            return repository.GetServiceProvider<T1>();
        }

        public object GetService(Type serviceType, Type interfaceType)
        {
            return repository.GetService(serviceType, interfaceType);
        }

        public ISystemWiring CreateSubsystem()
        {
            return new SystemWiring(new ScopedServiceRepository(this));
        }

        public void DecorateService<InterfaceType, DecoratingType>()
        {
            ISystemWiring subsystem = CreateSubsystem();
            subsystem.RegisterServiceProvider<InterfaceType>(repository.GetServiceProvider<InterfaceType>());
            repository.ReplaceServiceProvider<InterfaceType>(new DependencyResolvingServiceProvider<DecoratingType>(subsystem));
        }

        private T GetService<T>()
        {
            return (T) repository.GetService(typeof (T), typeof (T));
        }
    }

}
