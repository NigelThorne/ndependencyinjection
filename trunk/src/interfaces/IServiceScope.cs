//Copyright (c) 2008 Nigel Thorne
namespace NDependencyInjection.interfaces
{
    public interface IServiceScope : IServiceLocator
    {
        void RegisterServiceProvider<T1>(IServiceProvider provider);
        void RegisterServiceListener<T1>(IServiceProvider provider);
        void RegisterBroadcaster<EventsInterface>();
        IServiceScope CreateChildScope();
    }
}