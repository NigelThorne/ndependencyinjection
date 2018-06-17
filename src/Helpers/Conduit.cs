//Copyright (c) 2008 Nigel Thorne
using System;
using System.Diagnostics;
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

        [DebuggerStepThrough]
        object IInterceptor.Intercept(InvocationInfo info)
        {
            try
            {
                if (target == null) 
                    throw new NullReferenceException(
                        $"Target not set for {proxy.GetType()} conduit, when method {info.TargetMethod} was called from {info.CallingMethod}");
                return info.TargetMethod.Invoke(target, info.Arguments);
            }
            catch (TargetInvocationException ex)
            {
                throw ex.InnerException;
            }
        }
    }
}