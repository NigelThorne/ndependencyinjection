using System;
using NMockExtensions;
using NUnit.Framework;
using Rhino.Mocks;


namespace NDependencyInjection.Tests
{
    public abstract class RhinoMockingTestFixture : MockingTestFixture
    {
        public MockRepository mocks;

        protected override IDisposable Ordered
        {
            get { return mocks.Ordered(); }
        }

        [SetUp]
        public override void MockSetUp()
        {
            mocks = new MockRepository();
            SetUp();
        }

        public override T NewMock<T>()
        {
            return mocks.DynamicMock<T>();
        }

        public T NewStub<T>()
        {
            return mocks.Stub<T>();
        }

        public override void VerifyExpectations()
        {
            mocks.VerifyAll();
        }

        protected void SetupComplete()
        {
            mocks.ReplayAll();
        }
    }
}