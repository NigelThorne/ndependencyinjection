//Copyright (c) 2008 Nigel Thorne
namespace NDependencyInjection.interfaces
{
    public interface IBroadcasterProvider : IServiceProvider
    {
        void AddListenerProvider(IServiceProvider listenerProvider);
    }
}