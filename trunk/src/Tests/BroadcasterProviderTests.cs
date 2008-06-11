//Copyright (c) 2008 Nigel Thorne
using System;
using NDependencyInjection.interfaces;
using NDependencyInjection.Providers;
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