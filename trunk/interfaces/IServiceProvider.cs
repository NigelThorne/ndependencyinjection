namespace NDependencyInjection.interfaces
{
    public interface IServiceProvider<ServiceType>
    {
        ServiceType GetService();
    }
}