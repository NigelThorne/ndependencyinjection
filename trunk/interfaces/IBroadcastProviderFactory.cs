namespace NDependencyInjection.interfaces
{
    public interface IProviderFactory
    {
        IBroadcasterProvider CreateBroadcasterProvider<T>();
        IServiceProvider CreateConstructorCallingProvider<ConcreteType>(IServiceLocator locator);
    }
}