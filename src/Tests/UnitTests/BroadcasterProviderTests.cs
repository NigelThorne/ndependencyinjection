#region usings

using System;
using Moq;
using NDependencyInjection.interfaces;
using NDependencyInjection.Providers;
using NUnit.Framework;

#endregion

namespace NDependencyInjection.Tests
{
    public interface IXListener
    {
        void OnEvent ( );
    }

    [TestFixture]
    public class BroadcasterProviderTests
    {
        [SetUp]
        protected void SetUp ( )
        {
            broadcasterProvider = new BroadcasterProvider<IXListener> ();
            context = new Mock<IServiceLocator> ().Object;
        }

        private BroadcasterProvider<IXListener> broadcasterProvider;
        private IServiceLocator context;

        [Test]
        public void GetService_ReturnsAnEmptyBroadcaster_WhenNoListenersAreRegistered ( )
        {
            ( (IXListener) broadcasterProvider.GetService ( typeof (IXListener), typeof (IXListener), context ) )
                .OnEvent ();
        }

        [Test]
        public void GetService_ThrowsException_WhenAskedForIncorrectType ( )
        {
            Assert.Throws<InvalidProgramException> ( ( ) =>
                broadcasterProvider.GetService ( typeof (int), typeof (object), context ) );
        }
    }
}