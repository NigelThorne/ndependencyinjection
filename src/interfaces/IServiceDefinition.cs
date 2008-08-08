//Copyright (c) 2008 Nigel Thorne
namespace NDependencyInjection.interfaces
{
    public interface IServiceDefinition
    {
        IServiceDefinition Provides<Interface>();
        IServiceDefinition ListensTo<EventsListener>();
        IServiceDefinition Decorates<Interface>();
    }
}