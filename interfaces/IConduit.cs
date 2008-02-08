namespace NDependencyInjection.interfaces
{
    public interface IConduit<T>
    {
        T Proxy{ get;}
        void SetTarget(T target);
    }
}