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

        // SMELL! ignores context
        public object GetService(Type serviceType, Type interfaceType, IServiceLocator context)
        {
            return scope.GetService(interfaceType);
        }

        // SMELL: not used
        public void AddMapping(Type serviceType)
        {
        }
    }
}