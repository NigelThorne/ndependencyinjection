namespace NDependencyInjection.interfaces
{
    public interface ISystemWiring : IServiceLocator
    {
        void RegisterServiceProvider<T1>(IServiceProvider provider);
        void RegisterServiceListener<T1>(IServiceProvider provider);
        void RegisterBroadcaster<EventsInterface>();
        ISystemWiring CreateSubsystem();
    }
}