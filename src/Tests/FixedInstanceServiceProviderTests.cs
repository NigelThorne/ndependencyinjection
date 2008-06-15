//Copyright (c) 2008 Nigel Thorne
using NDependencyInjection.interfaces;
using NDependencyInjection.Providers;
using NMockExtensions;
using NUnit.Framework;
using Assert=NMockExtensions.Assert;


namespace NDependencyInjection.Tests
{
    [TestFixture]
    public class FixedInstanceServiceProviderTests : MockingTestFixture
    {
        private InstanceServiceProvider instanceServiceProvider;
        private object instance;
        private IServiceLocator context;

        protected override void SetUp()
        {
            instance = new object();
            context = NewMock<IServiceLocator>();
            instanceServiceProvider = new InstanceServiceProvider(instance);
        }

        [Test]
        public void GetService_ReturnsTheInstaceThatWasPassedIn()
        {
            Assert.AreSame(instance, instanceServiceProvider.GetService(typeof (object), typeof (object), context));
        }
    }
}