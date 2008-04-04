//Copyright (c) 2008 Nigel Thorne
using System;
using System.Collections.Generic;
using IServiceProvider=NDependencyInjection.interfaces.IServiceProvider;


namespace NDependencyInjection
{
    public class SingletonServiceProviderDecorator : IServiceProvider
    {
        private readonly IServiceProvider serviceProvider;
        private bool buildingInstance = false;
        private List<Conduit> conduits = new List<Conduit>();
        private object instance;

        public SingletonServiceProviderDecorator(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        public object GetService(Type service, Type interfaceType)
        {
            if (instance != null)
            {
                return instance;
            }

            if (buildingInstance)
            {
                return NewProxy(interfaceType);
            }

            buildingInstance = true;
            instance = serviceProvider.GetService(service, interfaceType);
            buildingInstance = false;
            ResolveProxies();
            return instance;
        }

        public object GetService(Type service)
        {
            return GetService(service, service);
        }

        private object NewProxy(Type interfaceType)
        {
            Conduit conduit = new Conduit(interfaceType);
            conduits.Add(conduit);
            return conduit.Proxy;
        }

        private void ResolveProxies()
        {
            foreach (Conduit conduit in conduits)
            {
                conduit.SetTarget(instance);
            }
        }
    }
}