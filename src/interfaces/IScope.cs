#region usings

using System;

#endregion

namespace NDependencyInjection.interfaces
{
    public interface IScope : IServiceLocator
    {
        void RegisterServiceProvider ( Type serviceType, IServiceProvider provider );
        void RegisterServiceListener<T1> ( IServiceProvider provider );
        void RegisterBroadcaster<EventsInterface> ( );
        IScope CreateInnerScope ( );
        void DecorateService<InterfaceType> ( IServiceProvider decorator );
        void RegisterCompositeItem<T> ( IServiceProvider provider );
    }
}