//Copyright (c) 2008 Nigel Thorne
using System;
using NDependencyInjection.interfaces;


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
    }
}