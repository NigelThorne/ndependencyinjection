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

        public void RegisterServiceProvider<T>(IServiceProvider provider)
        {
            if (dictionary.ContainsKey(typeof (T)))
                throw new InvalidOperationException(String.Format("Type {0} is already registered", typeof (T)));

            dictionary[typeof (T)] = provider;
        }

        public void ReplaceServiceProvider<T1>(IServiceProvider provider)
        {
            if (!dictionary.ContainsKey(typeof(T1)))
                throw new InvalidOperationException(String.Format("Type {0} not defined so you can't replace it", typeof(T1)));
            dictionary[typeof(T1)] = provider;
        }

        public object GetService(Type serviceType, Type serviceInterface)
        {
            if (!HasService(serviceType))
                throw new InvalidOperationException(String.Format("Type {0} is not registered", serviceType));

            return dictionary[serviceType].GetService(serviceType, serviceInterface);
        }

        public bool HasService(Type serviceType)
        {
            return dictionary.ContainsKey(serviceType);
        }

        public IServiceProvider GetServiceProvider<ServiceType>()
        {
            return dictionary[typeof (ServiceType)];
        }
    }
}