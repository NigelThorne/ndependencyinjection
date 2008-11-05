using System;
using System.Reflection;
using LinFu.DynamicProxy;
using NDependencyInjection.interfaces;


namespace NDependencyInjection
{
    public class BufferingConduit : IInterceptor, IConduit
    {
        private readonly object proxy;
        private object target;
        private InvocationInfo bufferedInvocation;

        public BufferingConduit(Type serviceType)
        {
            ProxyFactory factory = new ProxyFactory();
            proxy = factory.CreateProxy(serviceType, this);
        }

        public object Proxy
        {
            get { return proxy; }
        }

        public void SetTarget(object newTarget)
        {
            target = newTarget;

            if (bufferedInvocation != null)
            {
                ((IInterceptor) this).Intercept(bufferedInvocation);
                bufferedInvocation = null;
            }
        }

        object IInterceptor.Intercept(InvocationInfo info)
        {
            try
            {
                object returnValue;
                if (target == null && info.TargetMethod.ReturnType == typeof(void))
                {
                    bufferedInvocation = info;
                    returnValue = null;
                }
                else
                {
                    returnValue = info.TargetMethod.Invoke(target, info.Arguments);
                }
                return returnValue;
            }
            catch (TargetInvocationException ex)
            {
                throw ex.InnerException;
            }
        }
    }
}