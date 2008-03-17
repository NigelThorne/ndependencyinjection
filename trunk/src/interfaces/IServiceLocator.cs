using System;


namespace NDependencyInjection.interfaces
{
    public interface IServiceLocator : IServiceProvider
    {
        bool HasService(Type serviceType);
    }
}