//Copyright (c) 2008 Nigel Thorne
using System;
using System.Collections.Generic;
using NDependencyInjection.interfaces;
using IServiceProvider=NDependencyInjection.interfaces.IServiceProvider;


namespace NDependencyInjection.Providers
{
    public class SingletonServiceProviderDecorator : IServiceProvider
    {
        private readonly IServiceProvider serviceProvider;
        private bool buildingInstance = false;
        private readonly List<IConduit> conduits = new List<IConduit>();
        private object instance;

        public SingletonServiceProviderDecorator(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        public object GetService(Type service, Type interfaceType, IServiceLocator context)
        {
            if (instance != null)
            {
                return instance;
            }

            if (buildingInstance)
            {
                return NewProxy(interfaceType);
            }

            ContructingLockManager.Lock();

            buildingInstance = true;
            instance = serviceProvider.GetService(service, interfaceType, context);
            buildingInstance = false;

            ResolveProxies();

            ContructingLockManager.Release();

            return instance;
        }

        public void AddMapping(Type serviceType)
        {
            serviceProvider.AddMapping(serviceType);
        }

        public IConduit GetStateListenerConduit(Type serviceType)
        {
            IConduit conduit = new BufferingConduit(serviceType);
            conduits.Add(conduit);
            return conduit;
        }

        private object NewProxy(Type interfaceType)
        {
            IConduit conduit = new BufferingConduit(interfaceType);
//>>>            IConduit conduit = new Conduit(interfaceType);
            conduits.Add(conduit);
            return conduit.Proxy;
        }

        private void ResolveProxies()
        {
            foreach (IConduit conduit in conduits)
            {
                conduit.SetTarget(instance);
            }
        }
    }
}