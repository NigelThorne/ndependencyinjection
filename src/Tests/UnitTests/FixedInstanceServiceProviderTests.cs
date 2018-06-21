#region usings

using NDependencyInjection.interfaces;
using NDependencyInjection.Providers;
using NUnit.Framework;

#endregion

namespace NDependencyInjection.Tests
{
    [TestFixture]
    public class FixedInstanceServiceProviderTests : MockingTestFixture
    {
        protected override void SetUp ( )
        {
            _instance = new object ();
            _context = NewMock<IServiceLocator> ();
            _instanceServiceProvider = new InstanceServiceProvider ( _instance );
        }

        private InstanceServiceProvider _instanceServiceProvider;
        private object _instance;
        private IServiceLocator _context;

        [Test]
        public void GetService_ReturnsTheInstaceThatWasPassedIn ( )
        {
            Assert.AreSame ( _instance,
                _instanceServiceProvider.GetService ( typeof (object), typeof (object), _context ) );
        }
    }
}