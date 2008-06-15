//Copyright (c) 2008 Nigel Thorne
using System;
using NDependencyInjection.interfaces;
using IServiceProvider=NDependencyInjection.interfaces.IServiceProvider;


namespace NDependencyInjection.Tests.ExampleClasses
{
    public class SP : IServiceProvider
    {
        private readonly IMyTestClassA service;
        public bool gotCalled = false;

        public SP(IMyTestClassA service)
        {
            this.service = service;
        }

        public object GetService(Type serviceType, Type interfaceType, IServiceLocator context)
        {
            gotCalled = true;
            return service;
        }

        public void AddMapping(Type serviceType)
        {
            
        }
    }
}