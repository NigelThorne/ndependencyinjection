using System;
using System.Collections.Generic;
using NDependencyInjection.interfaces;
using IServiceProvider=NDependencyInjection.interfaces.IServiceProvider;


namespace NDependencyInjection.Providers
{
    public class CompositeProvider<Interface> : IServiceProvider, IComposite<Interface>
    {
        private readonly List<Interface> composite = new List<Interface>();

        public object GetService(Type serviceType, Type interfaceType, IServiceLocator context)
        {
            if (!typeof(Interface).IsAssignableFrom(interfaceType)
                && !typeof(IComposite<Interface>).IsAssignableFrom(interfaceType)
                && !typeof(Interface[]).IsAssignableFrom(interfaceType))
            {
                throw new InvalidProgramException(
                    string.Format("Composite supports type {0} not {1}", typeof(Interface), interfaceType));
            }

            if (serviceType == typeof(IComposite<Interface>))
            {
                return this;
            }
            return composite.ToArray();
        }

        public void AddMapping(Type serviceType)
        {
        }

        void IComposite<Interface>.Add(Interface item)
        {
            composite.Add(item);
        }

        void IComposite<Interface>.Remove(Interface item)
        {
            composite.Remove(item);
        }
    }
}
