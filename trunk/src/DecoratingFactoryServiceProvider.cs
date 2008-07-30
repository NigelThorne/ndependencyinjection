using System;
using NDependencyInjection.interfaces;
using NDependencyInjection.Providers;
using IServiceProvider=NDependencyInjection.interfaces.IServiceProvider;

namespace NDependencyInjection
{
    /// <summary>
    ///  TODO: Needs to create new instance only when the subsystem is creating a new instance.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class DecoratingServiceProvider<T, I> : IServiceProvider
    {
        private readonly IScope subsystem;
        private readonly FactoryServiceProvider<T> factory;
        private readonly IScope originalScope;

        public DecoratingServiceProvider(IScope subsystem, IServiceProvider serviceProvider)
        {
            factory = new FactoryServiceProvider<T>();
            this.subsystem = subsystem;
            originalScope = subsystem.CreateInnerScope();
            originalScope.RegisterServiceProvider(typeof(I), serviceProvider);
        }

        #region IServiceProvider Members

        public object GetService(Type serviceType, Type interfaceType, IServiceLocator context)
        {
            IScope newScope = originalScope.CreateInnerScope();
            newScope.RegisterServiceProvider(typeof(T), factory);

            return newScope.GetService(typeof(T));
        }

        public void AddMapping(Type serviceType)
        {
        }

        #endregion
    }
}