using NDependencyInjection.Providers;
using NUnit.Framework;

namespace NDependencyInjection.Tests
{
    [TestFixture]
    public class CompositeProviderTests 
    {
        private CompositeProvider<object> provider;

        [SetUp] protected void SetUp()
        {
            provider = new CompositeProvider<object>();
        }

        [Test]
        public void CanInstantiate()
        {
        }
    }
}
