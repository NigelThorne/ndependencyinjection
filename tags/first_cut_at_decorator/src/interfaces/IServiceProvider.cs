//Copyright (c) 2008 Nigel Thorne
using System;


namespace NDependencyInjection.interfaces
{
    public interface IServiceProvider
    {
        object GetService(Type serviceType, Type interfaceType);
    }
}