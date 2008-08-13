//Copyright (c) 2008 Nigel Thorne
using System;


namespace NDependencyInjection.interfaces
{
    public interface IScope : IServiceLocator
    {
        void RegisterServiceProvider(Type serviceType, IServiceProvider provider);
        void RegisterServiceListener<T1>(IServiceProvider provider);
        void RegisterBroadcaster<EventsInterface>();
        IScope CreateInnerScope();
        void DecorateService<InterfaceType>(IServiceProvider decorator);
    }
}