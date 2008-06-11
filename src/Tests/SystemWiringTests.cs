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
        private IBroadcasterProvider broadcastProvider;
        private IProviderFactory providerFactory;
        private IServiceLocator serviceLocator;
        private IServiceProvider serviceProvider;
        private IServiceScope serviceScope;
        private IServiceScope scope;

        protected override void SetUp()
        {
            serviceLocator = NewMock<IServiceLocator>();
            serviceScope = NewMock<IServiceScope>();


            providerFactory = NewStub<IProviderFactory>();
            broadcastProvider = NewMock<IBroadcasterProvider>();
            SetupResult.On(providerFactory).Call(providerFactory.CreateBroadcasterProvider<IA>()).Return(
                broadcastProvider);
            serviceProvider = NewMock<IServiceProvider>();
            SetupResult.On(providerFactory).Call(providerFactory.CreateConstructorCallingProvider<IA>(serviceScope)).Return(serviceProvider);

            scope = new ServiceRepository(serviceScope);
        }

    }

    public interface IOpticsEvents
    {
        void OnOpticsReady();
    }
}