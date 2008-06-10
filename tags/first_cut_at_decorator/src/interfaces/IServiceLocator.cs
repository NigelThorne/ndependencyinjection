//Copyright (c) 2008 Nigel Thorne
using System;


namespace NDependencyInjection.interfaces
{
    public interface IServiceLocator : IServiceProvider
    {
        bool HasService(Type serviceType);
        IServiceProvider GetServiceProvider<T1>();
    }
}