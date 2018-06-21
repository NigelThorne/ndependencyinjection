#region usings

using Moq;
using NUnit.Framework;

#endregion

namespace NDependencyInjection.Tests
{
    public class MockingTestFixture
    {
        [SetUp]
        protected virtual void SetUp ( )
        {
        }

        [TearDown]
        protected virtual void TearDown ( )
        {
        }

        protected T NewMock<T> ( ) where T : class
        {
            return new Mock<T> ().Object;
        }

        protected T NewStub<T> ( ) where T : class
        {
            return new Mock<T> ().Object;
        }

        protected Mock<T> MockOf<T> ( T obj ) where T : class
        {
            return Mock.Get ( obj );
        }
    }
}