using System;
using NUnit.Framework;
using Rhino.Mocks;


namespace NDependencyInjection.Tests
{
    public abstract class RhinoMockingTestFixture
    {
        public MockRepository mocks;

        protected IDisposable Ordered
        {
            get { return mocks.Ordered(); }
        }

        [SetUp]
        public void MockSetUp()
        {
            mocks = new MockRepository();
            SetUp();
        }

        protected abstract void SetUp();

        [TearDown]
        public virtual void MockTearDown()
        {
            TearDown();
            VerifyExpectations();
        }

        protected virtual void TearDown()
        {
        }

        public T NewMock<T>()
        {
            return mocks.DynamicMock<T>();
        }

        public T NewStub<T>()
        {
            return mocks.Stub<T>();
        }

        public void VerifyExpectations()
        {
            mocks.VerifyAll();
        }

        protected static void IgnoreReturnValue(object ignored)
        {
        }

        protected void SetupComplete()
        {
            mocks.ReplayAll();
        }
    }
}