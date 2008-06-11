//Copyright (c) 2008 Nigel Thorne
using NDependencyInjection.interfaces;
using NDependencyInjection.Tests.ExampleClasses;
using NUnit.Framework;
using Rhino.Mocks;


namespace NDependencyInjection.Tests
{

    [TestFixture]
    public class SystemWiringTests : RhinoMockingTestFixture
    {
        private IServiceLocator serviceLocator;
        private IServiceProvider serviceProvider;
        private IScope serviceScope;
        private IScope scope;

        protected override void SetUp()
        {
            serviceLocator = NewMock<IServiceLocator>();
            serviceScope = NewMock<IScope>();


            serviceProvider = NewMock<IServiceProvider>();

            scope = new ServiceRepository(serviceScope);
        }

    }

    public interface IOpticsEvents
    {
        void OnOpticsReady();
    }
}