//Copyright (c) 2008 Nigel Thorne
using System;
using System.Collections.Generic;
using NDependencyInjection.interfaces;
using IServiceProvider=NDependencyInjection.interfaces.IServiceProvider;


namespace NDependencyInjection
{
    public class ServiceRepository : IServiceRepository
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
        }

        public object GetService(Type serviceType)
        {
            if (dictionary.ContainsKey(serviceType))
            {
                return dictionary[serviceType].GetService(serviceType,serviceType);
            }
            return parentScope.GetService(serviceType);
        }

        public bool HasService(Type serviceType)
        {
            if ( dictionary.ContainsKey(serviceType)) return true;
            return parentScope.HasService(serviceType);
        }
    }
}