//Copyright (c) 2008 Nigel Thorne
using System;
using NDependencyInjection.interfaces;
using IServiceProvider = NDependencyInjection.interfaces.IServiceProvider;

namespace NDependencyInjection
{
    internal class ScopeQueryingProvider : IServiceProvider
    {
        private readonly IServiceLocator _scope;

        public ScopeQueryingProvider(IServiceLocator scope)
        {
            this._scope = scope;
        }

        public object GetService(Type serviceType, Type interfaceType, IServiceLocator context)
        {
            return _scope.GetService(interfaceType);
        }

        public void AddMapping(Type serviceType)
        {
            
        }
    }
}