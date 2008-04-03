using System;
using NMock2;
using NMockExtensions;
using NUnit.Framework;
using IServiceProvider=NDependencyInjection.interfaces.IServiceProvider;


namespace NDependencyInjection.Tests
{
    public interface IXListener
    {
        void OnEvent();
    }

    [TestFixture]
    public class BroadcasterProviderTests : MockingTestFixture
    {
        private BroadcasterProvider<IXListener> broadcasterProvider;

        protected override void SetUp()
        {
            broadcasterProvider = new BroadcasterProvider<IXListener>();
        }

        [Test]
        public void GetService_ReturnsABroadcasterWithTwoListeners_WhenTwoServiceProvidersAreRegistered()
        {
            IXListener l1 = NewMock<IXListener>();
            IServiceProvider provider1 = NewMock<IServiceProvider>();
            Stub.On(provider1).Method("GetService").With(typeof (IXListener), typeof (IXListener)).Will(Return.Value(l1));

            IXListener l2 = NewMock<IXListener>();
            IServiceProvider provider2 = NewMock<IServiceProvider>();
            Stub.On(provider2).Method("GetService").With(typeof (IXListener), typeof (IXListener)).Will(Return.Value(l2));

            broadcasterProvider.AddListenerProvider(provider1);
            broadcasterProvider.AddListenerProvider(provider2);

            Expect.Once.On(l1).Method("OnEvent");
            Expect.Once.On(l2).Method("OnEvent");
            ((IXListener)broadcasterProvider.GetService(typeof (IXListener), typeof (IXListener))).OnEvent();
        }

        [Test]
        public void AddListenerProvider_IncludesTheListenersInTheExistingBroadcastersListeners()
        {
            IXListener l1 = NewMock<IXListener>();
            IServiceProvider provider1 = NewMock<IServiceProvider>();
            Stub.On(provider1).Method("GetService").With(typeof (IXListener), typeof (IXListener)).Will(Return.Value(l1));

            IXListener l2 = NewMock<IXListener>();
            IServiceProvider provider2 = NewMock<IServiceProvider>();
            Stub.On(provider2).Method("GetService").With(typeof (IXListener), typeof (IXListener)).Will(Return.Value(l2));

            IXListener listener = (IXListener)broadcasterProvider.GetService(typeof(IXListener), typeof(IXListener));

            broadcasterProvider.AddListenerProvider(provider1);
            broadcasterProvider.AddListenerProvider(provider2);

            Expect.Once.On(l1).Method("OnEvent");
            Expect.Once.On(l2).Method("OnEvent");
            listener.OnEvent();
        }

        [Test]
        public void GetService_ReturnsAnEmptyBroadcaster_WhenNoListenersAreRegistered()
        {
            ((IXListener)broadcasterProvider.GetService(typeof(IXListener), typeof(IXListener))).OnEvent();
        }

        [Test, ExpectedException(typeof (InvalidProgramException))]
        public void GetService_ThrowsException_WhenAskedForIncorrectType()
        {
            broadcasterProvider.GetService(typeof (int), typeof (object));
        }
    }
}