//Copyright (c) 2008 Nigel Thorne
using System;


namespace NDependencyInjection.interfaces
{
    public interface IServiceLocator 
    {
        object GetService(Type serviceType);
        bool HasService(Type serviceType);
    }
}