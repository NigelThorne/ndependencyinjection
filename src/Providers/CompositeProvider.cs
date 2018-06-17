using System;
using System.Collections.Generic;
using NDependencyInjection.interfaces;
using IServiceProvider=NDependencyInjection.interfaces.IServiceProvider;


namespace NDependencyInjection.Providers
{
    public class CompositeProvider<TInterface> : IServiceProvider, IComposite<TInterface>
    {
        private readonly List<TInterface> _composite = new List<TInterface>();

        public object GetService(Type serviceType, Type interfaceType, IServiceLocator context)
        {
            if (!typeof(TInterface).IsAssignableFrom(interfaceType)
                && !typeof(IComposite<TInterface>).IsAssignableFrom(interfaceType)
                && !typeof(TInterface[]).IsAssignableFrom(interfaceType))
            {
                throw new InvalidProgramException(
                    $"Composite supports type {typeof(TInterface)} not {interfaceType}");
            }

            if (serviceType == typeof(IComposite<TInterface>))
            {
                return this;
            }
            return _composite.ToArray();
        }

        public void AddMapping(Type serviceType)
        {
        }

        void IComposite<TInterface>.Add(TInterface item)
        {
            _composite.Add(item);
        }

        void IComposite<TInterface>.Remove(TInterface item)
        {
            _composite.Remove(item);
        }
    }
}
