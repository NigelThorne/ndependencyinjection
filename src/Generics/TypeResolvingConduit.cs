#region usings

using System.Reflection;
using LinFu.DynamicProxy;
using NDependencyInjection.interfaces;

#endregion

namespace NDependencyInjection.Generics
{
    public class TypeResolvingConduit<InterfaceType> : IInterceptor
    {
        private readonly IServiceLocator context;
        private readonly IServiceProvider provider;
        private InterfaceType target;
        private bool targetSet;

        public TypeResolvingConduit ( IServiceProvider provider, IServiceLocator context )
        {
            this.provider = provider;
            this.context = context;
            Proxy = new ProxyFactory ().CreateProxy<InterfaceType> ( this );
        }

        public InterfaceType Proxy { get; }

        //[DebuggerStepThrough]
        object IInterceptor.Intercept ( InvocationInfo info )
        {
            try
            {
                if ( !targetSet )
                {
                    targetSet = true;
                    target = (InterfaceType) provider.GetService ( typeof (InterfaceType), typeof (InterfaceType),
                        context );
                }

                return info.TargetMethod.Invoke ( target, info.Arguments );
            }
            catch ( TargetInvocationException ex )
            {
                throw ex.InnerException;
            }
        }
    }
}