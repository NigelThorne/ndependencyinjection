//Copyright (c) 2008 Nigel Thorne
namespace NDependencyInjection.interfaces
{
    public interface IServiceDefinition
    {
        IServiceDefinition Provides<Interface>();
        IServiceDefinition ListensFor<EventsListener>();
        IServiceDefinition Decorates<Interface>();
        IServiceDefinition AddsToComposite<T>();
    }
}