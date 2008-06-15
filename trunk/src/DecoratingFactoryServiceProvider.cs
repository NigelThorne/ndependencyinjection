using System;
using NDependencyInjection.interfaces;
using IServiceProvider=NDependencyInjection.interfaces.IServiceProvider;

namespace NDependencyInjection
{
    /// <summary>
    ///  TODO: Needs to create new instance only when the subsystem is creating a new instance.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class DecoratingServiceProvider<T> : IServiceProvider
    {
        //private IServiceProvider factory;
        //public DecoratingFactoryServiceProvider(IServiceProvider decorated, IServiceProvider decorating)
        //{
        //    factory = new FactoryServiceProvider<T>();
        //}

        #region IServiceProvider Members

        public object GetService(Type serviceType, Type interfaceType, IServiceLocator context)
        {
            throw new NotImplementedException();
        }

        public void AddMapping(Type serviceType)
        {
        }

        #endregion
    }
}