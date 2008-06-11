//Copyright (c) 2008 Nigel Thorne
using System;
using System.Collections.Generic;
using NDependencyInjection.Generics;
using NDependencyInjection.interfaces;
using NDependencyInjection.Providers;
using IServiceProvider=NDependencyInjection.interfaces.IServiceProvider;

namespace NDependencyInjection
{
    /// <summary>
    /// </summary>
    public class ServiceRepository : IScope
    {
        private readonly Dictionary<Type, IServiceProvider> dictionary = new Dictionary<Type, IServiceProvider>();
        private readonly IServiceLocator parentScope;

        public ServiceRepository()
            : this(new NullServiceLocator())
        {
        }

        public ServiceRepository(IServiceLocator parentScope)
        {
            this.parentScope = parentScope;
        }

        public void RegisterServiceProvider<T>(IServiceProvider provider)
        {
            if (dictionary.ContainsKey(typeof (T)))
                throw new InvalidOperationException(String.Format("Type {0} is already registered", typeof (T)));

            dictionary[typeof (T)] = provider;
            provider.AddMapping(typeof (T));
        }

        public void RegisterServiceListener<EventsInterface>(IServiceProvider provider)
        {
            GetService<IBroadcaster<EventsInterface>>().AddListeners(new TypeResolvingConduit<EventsInterface>(provider).Proxy);
        }

        public void RegisterBroadcaster<EventsInterface>()
        {
            BroadcasterProvider<EventsInterface> provider = new BroadcasterProvider<EventsInterface>();
            RegisterServiceProvider<IBroadcaster<EventsInterface>>(provider);
            RegisterServiceProvider<EventsInterface>(provider);
        }

        public bool HasService(Type serviceType)
        {
            if (dictionary.ContainsKey(serviceType)) return true;
            return parentScope.HasService(serviceType);
        }

        public object GetService(Type serviceType)
        {
            if (dictionary.ContainsKey(serviceType))
            {
                return dictionary[serviceType].GetService(serviceType, serviceType);
            }
            return parentScope.GetService(serviceType);
        }

        public IServiceLocator Parent
        {
            get { return parentScope; }
        }

        public IScope CreateChildScope()
        {
            return new ServiceRepository(this);
        }

        private T GetService<T>()
        {
            return (T) GetService(typeof (T));
        }
    }
}