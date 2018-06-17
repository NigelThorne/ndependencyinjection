using Moq;
using NUnit.Framework;

namespace NDependencyInjection.Tests
{
    public class MockingTestFixture
    {
        [SetUp]
        protected virtual void SetUp()
        {

        }

        [TearDown]
        protected virtual void TearDown()
        {

        }

        protected T NewMock<T>() where T : class
        {
            return new Moq.Mock<T>().Object;
        }

        protected T NewStub<T>() where T : class
        {
            return new Moq.Mock<T>().Object;
        }

        protected Mock<T> MockOf<T>(T obj) where T : class
        {
            return Mock.Get(obj);
        }

    }
}