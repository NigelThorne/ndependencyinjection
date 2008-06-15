//Copyright (c) 2008 Nigel Thorne
using System;
using System.Collections;
using NDependencyInjection.interfaces;
using IServiceProvider=NDependencyInjection.interfaces.IServiceProvider;


namespace NDependencyInjection.Providers
{
    internal class CollectionProvider : IServiceProvider
    {
        private readonly IServiceLocator[] subsystems;

        public CollectionProvider(IServiceLocator[] subsystems)
        {
            this.subsystems = subsystems;
        }

        public object GetService(Type serviceType, Type interfaceType, IServiceLocator context)
        {
            ArrayList list = new ArrayList();
            foreach (IServiceLocator subsystem in subsystems)
            {
                list.Add(subsystem.GetService(interfaceType.GetElementType()));
            }
            return list.ToArray(interfaceType.GetElementType());
        }

        public void AddMapping(Type serviceType)
        {
            
        }
    }
}