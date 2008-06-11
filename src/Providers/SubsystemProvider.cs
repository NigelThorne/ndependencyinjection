//Copyright (c) 2008 Nigel Thorne
using System;
using NDependencyInjection.interfaces;
using IServiceProvider=NDependencyInjection.interfaces.IServiceProvider;


namespace NDependencyInjection.Providers
{
    internal class SubsystemProvider : IServiceProvider
    {
        private readonly IServiceLocator scope;

        public SubsystemProvider(IServiceLocator scope)
        {
            this.scope = scope;
        }

        public object GetService(Type serviceType, Type interfaceType)
        {
            return scope.GetService(interfaceType);
        }

        public void AddMapping(Type serviceType)
        {
            
        }
    }
}