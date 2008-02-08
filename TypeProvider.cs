using NDependencyInjection.interfaces;


namespace NDependencyInjection
{
    public class TypeProvider<ServiceType, ConcreteType> : IServiceProvider<ServiceType> where ConcreteType : ServiceType
    {
        public ServiceType GetService()
        {
            return default(ConcreteType);
        }
    }
}