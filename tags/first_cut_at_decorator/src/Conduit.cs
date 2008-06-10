//Copyright (c) 2008 Nigel Thorne
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
                if (target == null) 
                    throw new NullReferenceException(string.Format("Target not set for {0} conduit, when method {1} was called from {2}", proxy.GetType(), info.TargetMethod, info.CallingMethod));
                return info.TargetMethod.Invoke(target, info.Arguments);
            }
            catch (TargetInvocationException ex)
            {
                throw ex.InnerException;
            }
        }
    }
}