#region usings

using NDependencyInjection.Providers;
using NUnit.Framework;

#endregion

namespace NDependencyInjection.Tests
{
    [TestFixture]
    public class CompositeProviderTests
    {
        [SetUp]
        protected void SetUp ( )
        {
            provider = new CompositeProvider<object> ();
        }

        private CompositeProvider<object> provider;

        [Test]
        public void CanInstantiate ( )
        {
        }
    }
}