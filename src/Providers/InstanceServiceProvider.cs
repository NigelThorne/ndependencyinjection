#region usings

using System;
using NDependencyInjection.interfaces;
using IServiceProvider = NDependencyInjection.interfaces.IServiceProvider;

#endregion

namespace NDependencyInjection.Providers
{
    public class InstanceServiceProvider : IServiceProvider
    {
        private readonly object instance;

        public InstanceServiceProvider ( object instance )
        {
            this.instance = instance;
        }

        public object GetService ( Type serviceType, Type interfaceType, IServiceLocator context )
        {
            return instance;
        }

        public void AddMapping ( Type serviceType )
        {
        }
    }
}