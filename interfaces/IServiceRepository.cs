namespace NDependencyInjection.interfaces
{
    public interface IServiceRepository : IServiceLocator
    {
        void RegisterServiceProvider<T>(IServiceProvider provider);
    }
}