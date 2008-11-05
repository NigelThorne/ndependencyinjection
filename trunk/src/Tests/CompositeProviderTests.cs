using NDependencyInjection.Providers;
using NMockExtensions;
using NUnit.Framework;

namespace NDependencyInjection.Tests
{
    [TestFixture]
    public class CompositeProviderTests : MockingTestFixture
    {
        private CompositeProvider<object> provider;

        protected override void SetUp()
        {
            provider = new CompositeProvider<object>();
        }

        [Test]
        public void CanInstantiate()
        {
        }
    }
}
