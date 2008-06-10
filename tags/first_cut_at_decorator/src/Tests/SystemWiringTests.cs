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
        private IServiceRepository serviceRepository;
        private ISystemWiring wiring;

        protected override void SetUp()
        {
            serviceLocator = NewMock<IServiceLocator>();
            serviceRepository = NewMock<IServiceRepository>();


            providerFactory = NewStub<IProviderFactory>();
            broadcastProvider = NewMock<IBroadcasterProvider>();
            SetupResult.On(providerFactory).Call(providerFactory.CreateBroadcasterProvider<IA>()).Return(
                broadcastProvider);
            serviceProvider = NewMock<IServiceProvider>();
            SetupResult.On(providerFactory).Call(providerFactory.CreateConstructorCallingProvider<IA>(serviceRepository)).Return(serviceProvider);

            wiring = new SystemWiring(serviceRepository);
        }

    }

    public interface IOpticsEvents
    {
        void OnOpticsReady();
    }
}