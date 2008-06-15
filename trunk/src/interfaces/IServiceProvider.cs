//Copyright (c) 2008 Nigel Thorne
using System;


namespace NDependencyInjection.interfaces
{
    public interface IServiceProvider
    {
        object GetService(Type serviceType, Type interfaceType, IServiceLocator context);
        void AddMapping(Type serviceType);
    }
}