//Copyright (c) 2008 Nigel Thorne
namespace NDependencyInjection.interfaces
{
    public interface IScope : IServiceLocator
    {
        void RegisterServiceProvider<T1>(IServiceProvider provider);
        void RegisterServiceListener<T1>(IServiceProvider provider);
        void RegisterBroadcaster<EventsInterface>();
        IScope CreateInnerScope();
        void DecorateService<T1, T2>();
    }
}