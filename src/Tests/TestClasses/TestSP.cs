#region usings

using System;
using NDependencyInjection.interfaces;
using IServiceProvider = NDependencyInjection.interfaces.IServiceProvider;

#endregion

namespace NDependencyInjection.Tests.TestClasses
{
    public class TestSP : IServiceProvider
    {
        private readonly IMyTestClassA service;
        public bool gotCalled = false;

        public TestSP ( IMyTestClassA service )
        {
            this.service = service;
        }

        public object GetService ( Type serviceType, Type interfaceType, IServiceLocator context )
        {
            gotCalled = true;
            return service;
        }

        public void AddMapping ( Type serviceType )
        {
        }
    }
}