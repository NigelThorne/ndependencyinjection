//Copyright (c) 2008 Nigel Thorne
namespace NDependencyInjection.interfaces
{
    public interface ISystemComponent
    {
        ISystemComponent Provides<Interface>();
        ISystemComponent ListensTo<EventsListener>();
    }
}