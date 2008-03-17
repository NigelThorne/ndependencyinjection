namespace NDependencyInjection.interfaces
{
    public interface ISystemComponent
    {
        ISystemComponent Provides<Interface>();
        ISystemComponent ListensTo<EventsListener>();
    }
}