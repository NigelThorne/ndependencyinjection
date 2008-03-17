using System;
using IServiceProvider=NDependencyInjection.interfaces.IServiceProvider;


namespace NDependencyInjection
{
    public class FixedInstanceServiceProvider : IServiceProvider
    {
        private readonly object instance;

        public FixedInstanceServiceProvider(object instance)
        {
            this.instance = instance;
        }

        public object GetService(Type serviceType, Type interfaceType)
        {
            return instance;
        }
    }
}