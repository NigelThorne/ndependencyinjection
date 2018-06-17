//Copyright (c) 2008 Nigel Thorne
using NDependencyInjection.interfaces;
using NDependencyInjection.Providers;
using NUnit.Framework;
using Assert = NUnit.Framework.Assert;


namespace NDependencyInjection.Tests
{
    [TestFixture]
    public class FixedInstanceServiceProviderTests : MockingTestFixture
    {
        private InstanceServiceProvider _instanceServiceProvider;
        private object _instance;
        private IServiceLocator _context;

        protected override void SetUp()
        {
            _instance = new object();
            _context = NewMock<IServiceLocator>();
            _instanceServiceProvider = new InstanceServiceProvider(_instance);
        }

        [Test]
        public void GetService_ReturnsTheInstaceThatWasPassedIn()
        {
            Assert.AreSame(_instance, _instanceServiceProvider.GetService(typeof (object), typeof (object), _context));
        }
    }
}