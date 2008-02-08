using System.Reflection;
using LinFu.DynamicProxy;
using NDependencyInjection.interfaces;


namespace NDependencyInjection
{
    public class Conduit<T> :IInterceptor ,IConduit<T>
    {
        private readonly T proxy;
        private T target;

        public Conduit()
        {
            ProxyFactory factory  = new ProxyFactory();
            proxy = factory.CreateProxy<T>(this);
        }

        public T Proxy
        {
            get { return proxy; }
        }

        public void SetTarget(T target)
        {
            this.target = target;
        }

        object IInterceptor.Intercept(InvocationInfo info)
        {
            try
            {
                return info.TargetMethod.Invoke(target, info.Arguments);
            }
            catch(TargetInvocationException ex)
            {
                throw ex.InnerException;
            }
        }
    }
}