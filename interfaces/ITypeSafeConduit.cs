namespace NDependencyInjection.interfaces
{
    public interface ITypeSafeConduit<T>
    {
        T Proxy { get; }
        void SetTarget(T target);
    }
}