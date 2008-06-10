//Copyright (c) 2008 Nigel Thorne
using System;
using NDependencyInjection.interfaces;
using IServiceProvider=NDependencyInjection.interfaces.IServiceProvider;


namespace NDependencyInjection
{
    internal class NullServiceLocator : IServiceLocator
    {
        public object GetService(Type serviceType, Type serviceInterface)
        {
            throw new UnknownTypeException(serviceType);
        }

        public bool HasService(Type serviceType)
        {
            return false;
        }

        public IServiceProvider GetServiceProvider<ServiceType>()
        {
            throw new UnknownTypeException(typeof(ServiceType));
        }
    }
}