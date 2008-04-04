//Copyright (c) 2008 Nigel Thorne
namespace NDependencyInjection.interfaces
{
    public interface ITypeSafeConduit<T>
    {
        T Proxy { get; }
        void SetTarget(T target);
    }
}