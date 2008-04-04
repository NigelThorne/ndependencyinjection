//Copyright (c) 2008 Nigel Thorne
using System.Reflection;
using LinFu.DynamicProxy;
using NDependencyInjection.interfaces;


namespace NDependencyInjection.Generics
{
    public class TypeResolvingConduit<InterfaceType> : IInterceptor
    {
        private readonly IServiceProvider provider;
        private readonly InterfaceType proxy;
        private bool targetSet = false;
        private InterfaceType target = default(InterfaceType);

        public TypeResolvingConduit(IServiceProvider provider)
        {
            this.provider = provider;
            proxy = new ProxyFactory().CreateProxy<InterfaceType>(this);
        }

        public InterfaceType Proxy
        {
            get { return proxy; }
        }

        object IInterceptor.Intercept(InvocationInfo info)
        {
            try
            {
                if(!targetSet)
                {
                    targetSet = true;
                    target = (InterfaceType) provider.GetService(typeof (InterfaceType), typeof (InterfaceType));                    
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