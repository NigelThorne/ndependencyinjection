using NDependencyInjection.interfaces;
using NDependencyInjection.Tests.ExampleClasses;
using NUnit.Framework;
using Rhino.Mocks;


namespace NDependencyInjection.Tests
{
    public delegate void CreateSubsystem(ISystemDefinition scope);

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

//        [Test]
//        public void RegisterListener_RegistersABroadcastProviderWithTheServiceRepository()
//        {
//            IServiceProvider provider = NewMock<IServiceProvider>();
//            Expect.Call(delegate{serviceRepository.RegisterServiceProvider<IA>(broadcastProvider);});
//            Expect.Call(delegate{broadcastProvider.AddListenerProvider(provider);});
//
//            SetupComplete();
//
////            wiring.RegisterListener<IA, ClassA>();
//            IA a1 = wiring.Get<IA>();
//        }

//
//        // Calling twice registers one service 
//
//        // GET<IAListener>().SomeMethod()... should call SomeMethod on both Listeners.
    }

    public interface IOpticsEvents
    {
        void OnOpticsReady();
    }
}