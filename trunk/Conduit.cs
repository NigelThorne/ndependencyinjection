using System;
using System.Reflection;
using LinFu.DynamicProxy;
using NDependencyInjection.interfaces;


namespace NDependencyInjection
{
    public class Conduit : IInterceptor, IConduit
    {
        private readonly object proxy;
        private object target;

        public Conduit(Type serviceType)
        {
            ProxyFactory factory = new ProxyFactory();
            proxy = factory.CreateProxy(serviceType, this);
        }

        public object Proxy
        {
            get { return proxy; }
        }

        public void SetTarget(object target)
        {
            this.target = target;
        }

        object IInterceptor.Intercept(InvocationInfo info)
        {
            try
            {
                return info.TargetMethod.Invoke(target, info.Arguments);
            }
            catch (TargetInvocationException ex)
            {
                throw ex.InnerException;
            }
        }
    }
}