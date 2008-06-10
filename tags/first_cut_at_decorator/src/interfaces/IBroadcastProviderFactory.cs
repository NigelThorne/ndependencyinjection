//Copyright (c) 2008 Nigel Thorne
namespace NDependencyInjection.interfaces
{
    public interface IProviderFactory
    {
        IBroadcasterProvider CreateBroadcasterProvider<T>();
        IServiceProvider CreateConstructorCallingProvider<ConcreteType>(IServiceLocator locator);
    }
}