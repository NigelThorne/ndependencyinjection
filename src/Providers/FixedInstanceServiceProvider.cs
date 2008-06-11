//Copyright (c) 2008 Nigel Thorne
using System;
using IServiceProvider=NDependencyInjection.interfaces.IServiceProvider;


namespace NDependencyInjection.Providers
{
    public class FixedInstanceServiceProvider : IServiceProvider
    {
        private readonly object instance;

        public FixedInstanceServiceProvider(object instance)
        {
            this.instance = instance;
        }

        public object GetService(Type serviceType, Type interfaceType)
        {
            return instance;
        }

        public void AddMapping(Type serviceType)
        {
            
        }
    }
}