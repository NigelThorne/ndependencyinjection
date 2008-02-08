namespace NDependencyInjection.interfaces
{
    public interface IServiceRepository : IServiceLocator
    {
        void RegisterService<T>(IServiceProvider<T> provider);
    }
}