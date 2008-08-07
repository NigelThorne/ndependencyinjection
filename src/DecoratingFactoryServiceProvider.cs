using System;
using System.Collections.Generic;
using NDependencyInjection.interfaces;
using NDependencyInjection.Providers;
using IServiceProvider=NDependencyInjection.interfaces.IServiceProvider;

namespace NDependencyInjection
{
    /// <summary>
    ///  TODO: Needs to create new instance only when the subsystem is creating a new instance.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="I"></typeparam>
    public class DecoratingServiceProvider<T, I> : IServiceProvider
    {
        private readonly FactoryServiceProvider<T> factory;
        private readonly IScope originalScope;
        private readonly Dictionary<object, object> cache = new Dictionary<object, object>();

        public DecoratingServiceProvider(IScope subsystem, IServiceProvider serviceProvider)
        {
            factory = new FactoryServiceProvider<T>();
            originalScope = subsystem.CreateInnerScope();
            originalScope.RegisterServiceProvider(typeof(I), serviceProvider);
        }

        #region IServiceProvider Members

        public object GetService(Type serviceType, Type interfaceType, IServiceLocator context)
        {
            object service = originalScope.GetService(typeof (I));

            if (cache.ContainsKey(service)) return cache[service];
            
            IScope newScope = originalScope.CreateInnerScope();
            newScope.RegisterServiceProvider(typeof (I), new InstanceServiceProvider(service));
            newScope.RegisterServiceProvider(typeof (T), factory);
            object decorator = newScope.GetService(typeof (T));
            cache[service] = decorator;
            return decorator;
        }

        public void AddMapping(Type serviceType)
        {
        }

        #endregion
    }
}