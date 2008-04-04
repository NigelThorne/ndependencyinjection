//Copyright (c) 2008 Nigel Thorne
using System;
using NDependencyInjection.interfaces;
using IServiceProvider=NDependencyInjection.interfaces.IServiceProvider;


namespace NDependencyInjection
{
    public class ScopedServiceRepository : IServiceRepository
    {
        private readonly IServiceLocator parentScope;
        private readonly IServiceRepository repository;

        public ScopedServiceRepository(IServiceLocator parentScope) : this(new ServiceRepository(), parentScope)
        {
        }

        public ScopedServiceRepository(IServiceRepository repository, IServiceLocator parentScope)
        {
            this.repository = repository;
            this.parentScope = parentScope;
        }

        public void RegisterServiceProvider<T>(IServiceProvider provider)
        {
            repository.RegisterServiceProvider<T>(provider);
        }

        public bool HasService(Type serviceType)
        {
            if (repository.HasService(serviceType)) return true;
            return parentScope.HasService(serviceType);
        }

        public object GetService(Type serviceType, Type serviceInterface)
        {
            if (repository.HasService(serviceType))
            {
                return repository.GetService(serviceType, serviceInterface);
            }
            return parentScope.GetService(serviceType, serviceInterface);
        }
    }
}