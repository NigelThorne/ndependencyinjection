//Copyright (c) 2008 Nigel Thorne
using NDependencyInjection.interfaces;


namespace NDependencyInjection
{
    public class ProviderFactory : IProviderFactory
    {
        public IBroadcasterProvider CreateBroadcasterProvider<T>()
        {
            return new BroadcasterProvider<T>();
        }

        public IServiceProvider CreateConstructorCallingProvider<ConcreteType>(IServiceLocator locator)
        {
            return new FactoryServiceProvider<ConcreteType>(locator);
        }
    }
}