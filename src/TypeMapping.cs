//Copyright (c) 2008 Nigel Thorne
using System;
using NDependencyInjection.interfaces;
using IServiceProvider=NDependencyInjection.interfaces.IServiceProvider;


namespace NDependencyInjection
{
    public class TypeMapping<T> : IServiceProvider
    {
        private readonly IServiceLocator locator;

        public TypeMapping(IServiceLocator locator)
        {
            this.locator = locator;
        }

        public object GetService(Type serviceType, Type interfaceType)
        {
            return locator.GetService(typeof (T), interfaceType);
        }
    }
}