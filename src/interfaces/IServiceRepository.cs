//Copyright (c) 2008 Nigel Thorne
namespace NDependencyInjection.interfaces
{
    public interface IServiceRepository : IServiceLocator
    {
        void RegisterServiceProvider<T>(IServiceProvider provider);
        void ReplaceServiceProvider<T1>(IServiceProvider provider);
    }
}