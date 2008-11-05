//Copyright (c) 2008 Nigel Thorne
using System;


namespace NDependencyInjection.interfaces
{
    public interface IScope : IServiceLocator
    {
        void RegisterServiceProvider(Type serviceType, IServiceProvider provider);
        void RegisterServiceListener<T1>(IServiceProvider provider);
        void RegisterServiceStateListener<T>(IServiceProvider provider);
        void RegisterBroadcaster<EventsInterface>();
        void RegisterStateBroadcaster<EventsInterface>();
        IScope CreateInnerScope();
        void DecorateService<InterfaceType>(IServiceProvider decorator);
        void RegisterCompositeItem<T>(IServiceProvider provider);
    }
}