//Copyright (c) 2008 Nigel Thorne
namespace NDependencyInjection.interfaces
{
    public interface IConduit
    {
        object Proxy { get; }
        void SetTarget(object target);
    }
}