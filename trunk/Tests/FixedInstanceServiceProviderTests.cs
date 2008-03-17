using NUnit.Framework;
using Varian.Tests.Utilities;


namespace NDependencyInjection.Tests
{
    [TestFixture]
    public class FixedInstanceServiceProviderTests : MockingTestFixture
    {
        private FixedInstanceServiceProvider fixedInstanceServiceProvider;
        private object instance;

        protected override void SetUp()
        {
            instance = new object();
            fixedInstanceServiceProvider = new FixedInstanceServiceProvider(instance);
        }

        [Test]
        public void GetService_ReturnsTheInstaceThatWasPassedIn()
        {
            Assert.AreSame(instance, fixedInstanceServiceProvider.GetService(typeof (object), typeof (object)));
        }
    }
}