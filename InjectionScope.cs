using System;
using NDependencyInjection.interfaces;


namespace NDependencyInjection
{
    public class InjectionScope : IInjectionScope
    {
        private readonly IServiceLocator parentScope;
        private readonly IServiceRepository repository;

        public InjectionScope() : this(new NullServiceLocator())
        {
        }

        public InjectionScope(IServiceLocator parentScope)
        {
            this.parentScope = parentScope;
            repository = new ServiceRepository();
        }

        public ServiceType Get<ServiceType>()
        {
            if (repository.Has<ServiceType>())
            {
                return repository.Get<ServiceType>();
            }
            return parentScope.Get<ServiceType>();
        }

        public bool Has<T>()
        {
            if (repository.Has<T>()) return true;
            return parentScope.Has<T>();
        }

        public void Singleton<ServiceType>() where ServiceType : class
        {
            if (repository.Has<ServiceType>())
                throw new InvalidOperationException("{0} is already registered within this scope. You must set your class as a singleton before binding to it.");
            repository.RegisterService(Singleton(ResolvedType<ServiceType>()));
        }

        public void Bind<ServiceInterface, ServiceType>() where ServiceType : ServiceInterface
        {
            if (!repository.Has<ServiceType>())
                repository.RegisterService(ResolvedType<ServiceType>());
            repository.RegisterService(TypeMapping<ServiceInterface, ServiceType>());
        }
        
        public void BindSingleton<InterfaceType, ConcreteType>() where ConcreteType : class, InterfaceType
        {
            Singleton<ConcreteType>();
            Bind<InterfaceType,ConcreteType>();
        }

        public void SingletonInstance<InterfaceType>(InterfaceType instance) 
        {
            repository.RegisterService(Singleton(ConstantInstance(instance)));
        }

        private static FixedInstanceServiceProvider<InterfaceType> ConstantInstance<InterfaceType>(InterfaceType instance)
        {
            return new FixedInstanceServiceProvider<InterfaceType>(instance);
        }

        private static IServiceProvider<InterfaceType> Singleton<InterfaceType>(IServiceProvider<InterfaceType> provider)
        {
            return new SingletonServiceProviderDecorator<InterfaceType>(provider);
        }

        private IServiceProvider<InterfaceType> TypeMapping<InterfaceType, ConcreteType>() where ConcreteType : InterfaceType
        {
            return new TypeMappingServiceProvider<InterfaceType, ConcreteType>(this);
        }

        private ConstructorResolvingServiceProvider<ConcreteType> ResolvedType<ConcreteType>()
        {
            return new ConstructorResolvingServiceProvider<ConcreteType>(this);
        }
    }
}