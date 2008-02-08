namespace NDependencyInjection.interfaces
{
    public interface IInjectionScope: IServiceLocator
    {
        void Bind<InterfaceType, ConcreteType>() where ConcreteType : InterfaceType;
        void Singleton<ConcreteType>() where ConcreteType : class;
        void SingletonInstance<InterfaceType>(InterfaceType instance);
    }
}