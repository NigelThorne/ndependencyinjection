#region usings

using System;
using System.Reflection;
using LinFu.DynamicProxy;
using NDependencyInjection.interfaces;

#endregion

namespace NDependencyInjection
{
    public class Conduit : IInterceptor, IConduit
    {
        private object target;

        public Conduit ( Type serviceType )
        {
            var factory = new ProxyFactory ();
            Proxy = factory.CreateProxy ( serviceType, this );
        }

        public object Proxy { get; }

        public void SetTarget ( object target )
        {
            this.target = target;
        }

        //[DebuggerStepThrough]
        object IInterceptor.Intercept ( InvocationInfo info )
        {
            try
            {
                if ( target == null )
                    throw new NullReferenceException (
                        $"Target not set for {Proxy.GetType ()} conduit, when method {info.TargetMethod} was called from {info.CallingMethod}" );
                return info.TargetMethod.Invoke ( target, info.Arguments );
            }
            catch ( TargetInvocationException ex )
            {
                throw ex.InnerException;
            }
        }
    }
}