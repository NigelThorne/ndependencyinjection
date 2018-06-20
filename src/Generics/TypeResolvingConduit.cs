//Copyright (c) 2008 Nigel Thorne

using System.Diagnostics;
using System.Reflection;
using LinFu.DynamicProxy;
using NDependencyInjection.interfaces;


namespace NDependencyInjection.Generics
{
    public class TypeResolvingConduit<InterfaceType> : IInterceptor
    {
        private readonly IServiceProvider provider;
        private readonly InterfaceType proxy;
        private bool targetSet;
        private InterfaceType target;
        private readonly IServiceLocator context;

        public TypeResolvingConduit(IServiceProvider provider, IServiceLocator context)
        {
            this.provider = provider;
            this.context = context;
            proxy = new ProxyFactory().CreateProxy<InterfaceType>(this);
        }

        public InterfaceType Proxy
        {
            get { return proxy; }
        }

        //[DebuggerStepThrough]
        object IInterceptor.Intercept(InvocationInfo info)
        {
            try
            {
                if(!targetSet)
                {
                    targetSet = true;
                    target = (InterfaceType) provider.GetService(typeof (InterfaceType), typeof (InterfaceType), context);                    
                }
                return info.TargetMethod.Invoke(target, info.Arguments);
            }
            catch (TargetInvocationException ex)
            {
                throw ex.InnerException;
            }
        }
    }
}