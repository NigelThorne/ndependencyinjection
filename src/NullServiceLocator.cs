//Copyright (c) 2008 Nigel Thorne
using System;
using NDependencyInjection.interfaces;
using IServiceProvider=NDependencyInjection.interfaces.IServiceProvider;


namespace NDependencyInjection
{
    internal class NullServiceLocator : IServiceLocator
    {
        public object GetService(Type serviceType)
        {
            throw new UnknownTypeException(serviceType);
        }

        public bool HasService(Type serviceType)
        {
            return false;
        }
    }
}