using System;
using System.Collections.Generic;
using NDependencyInjection.interfaces;
using NDependencyInjection.Providers;
using IServiceProvider=NDependencyInjection.interfaces.IServiceProvider;

namespace NDependencyInjection
{
    public class DecoratingServiceProvider<I> : IServiceProvider
    {
        private readonly IServiceProvider decoratedProvider;
        private readonly IServiceProvider decoratorProvider;
        private readonly Dictionary<object, object> cache = new Dictionary<object, object>();

        public DecoratingServiceProvider(IServiceProvider decoratedProvider, IServiceProvider decoratorProvider)
        {
            this.decoratedProvider = decoratedProvider;
            this.decoratorProvider = decoratorProvider;
        }

        #region IServiceProvider Members

        public object GetService(Type serviceType, Type interfaceType, IServiceLocator context)
        {
            IScope scope = new Scope(context);
            scope.RegisterServiceProvider(typeof(I), decoratedProvider);
            object decorated = scope.GetService(interfaceType);

            if (cache.ContainsKey(decorated))
            {
                return cache[decorated];
            }

            IScope scope2 = new Scope(context);
            scope2.RegisterServiceProvider(interfaceType, new InstanceServiceProvider(decorated));

            object service = decoratorProvider.GetService(serviceType, interfaceType, scope2);
            cache[decorated] = service;
            return service;
        }

        public void AddMapping(Type serviceType)
        {
        }

        #endregion
    }
}