using NDependencyInjection.interfaces;


namespace NDependencyInjection
{
    public class SingletonServiceProviderDecorator<ServiceType> : IServiceProvider<ServiceType>
    {
        private readonly IServiceProvider<ServiceType> serviceProvider;
        private bool hasInstance = false;
        private ServiceType instance;

        public SingletonServiceProviderDecorator(IServiceProvider<ServiceType> serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        public ServiceType GetService()
        {
            if (!hasInstance)
            {
                Conduit<ServiceType> conduit = new Conduit<ServiceType>();
                instance = conduit.Proxy;
                hasInstance = true;
                instance = serviceProvider.GetService();
                conduit.SetTarget(instance);
            }
            return instance;
        }
    }
}