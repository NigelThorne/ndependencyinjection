//Copyright (c) 2008 Nigel Thorne
using System;
using NDependencyInjection.Generics;
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
        private BroadcasterProvider<IXListener, TypeSafeBroadcaster<IXListener>> broadcasterProvider;
        private IServiceLocator context;

        protected override void SetUp()
        {
            broadcasterProvider = new BroadcasterProvider<IXListener, TypeSafeBroadcaster<IXListener>>();
            context = NewMock<IServiceLocator>();
        }

        [Test]
        public void GetService_ReturnsAnEmptyBroadcaster_WhenNoListenersAreRegistered()
        {
            ((IXListener)broadcasterProvider.GetService(typeof(IXListener), typeof(IXListener), context)).OnEvent();
        }

        [Test, ExpectedException(typeof (InvalidProgramException))]
        public void GetService_ThrowsException_WhenAskedForIncorrectType()
        {
            broadcasterProvider.GetService(typeof (int), typeof (object), context);
        }
    }
}