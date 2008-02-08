namespace NDependencyInjection.interfaces
{
    public interface IServiceLocator
    {
        T Get<T>();
        bool Has<T>();
    }
}