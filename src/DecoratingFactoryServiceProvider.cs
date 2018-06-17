using System;
using System.Collections.Generic;
using NDependencyInjection.interfaces;
using NDependencyInjection.Providers;
using IServiceProvider=NDependencyInjection.interfaces.IServiceProvider;

namespace NDependencyInjection
{
    public class DecoratingServiceProvider<I> : IServiceProvider
    {
        private readonly IServiceProvider _decoratedProvider;
        private readonly IServiceProvider _decoratorProvider;
        private readonly Dictionary<object, object> _cache = new Dictionary<object, object>();

        public DecoratingServiceProvider(IServiceProvider decoratedProvider, IServiceProvider decoratorProvider)
        {
            this._decoratedProvider = decoratedProvider;
            this._decoratorProvider = decoratorProvider;
        }

        public object GetService(Type serviceType, Type interfaceType, IServiceLocator context)
        {
            var decorated = FetchInstanceToDecorate(interfaceType, context);
            return FetchDecoratorInstance(serviceType, interfaceType, context, decorated);
        }

        public void AddMapping(Type serviceType)
        {
        }

        private object FetchInstanceToDecorate(Type interfaceType, IServiceLocator context)
        {
            IScope decoratedScope = new Scope(context);
            decoratedScope.RegisterServiceProvider(typeof(I), _decoratedProvider);
            return decoratedScope.GetService(interfaceType);
        }

        private object FetchDecoratorInstance(Type serviceType, Type interfaceType, IServiceLocator context, object decorated)
        {
            if (_cache.ContainsKey(decorated)) return _cache[decorated];
            _cache[decorated] = CreateDecoratorInstance(serviceType, interfaceType, context, decorated);
            return _cache[decorated];
        }

        private object CreateDecoratorInstance(Type serviceType, Type interfaceType, IServiceLocator context, object decorated)
        {
            IScope decoratorScope = new Scope(context);
            decoratorScope.RegisterServiceProvider(interfaceType, new InstanceServiceProvider(decorated));
            return _decoratorProvider.GetService(serviceType, interfaceType, decoratorScope);
        }
    }
}