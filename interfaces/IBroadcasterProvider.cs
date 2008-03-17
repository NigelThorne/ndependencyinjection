namespace NDependencyInjection.interfaces
{
    public interface IBroadcasterProvider : IServiceProvider
    {
        void AddListenerProvider(IServiceProvider listenerProvider);
    }
}