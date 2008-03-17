namespace NDependencyInjection.interfaces
{
    public interface IConduit
    {
        object Proxy { get; }
        void SetTarget(object target);
    }
}