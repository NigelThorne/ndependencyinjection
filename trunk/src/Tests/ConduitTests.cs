//Copyright (c) 2008 Nigel Thorne
using System;
using NDependencyInjection.Generics;
using NDependencyInjection.interfaces;
using NDependencyInjection.Tests.ExampleClasses;
using NMock2;
using NMockExtensions;
using NUnit.Framework;
using Assert=NUnit.Framework.Assert;


namespace NDependencyInjection.Tests
{
    [TestFixture]
    public class ConduitTests : MockingTestFixture
    {
        private IA proxy;
        private IA target;

        protected override void SetUp()
        {
            ITypeSafeConduit<IA> typeSafeConduit = new TypeSafeConduit<IA>();

            proxy = typeSafeConduit.Proxy;
            Assert.IsNotNull(proxy);

            target = NewMock<IA>();
            typeSafeConduit.SetTarget(target);
        }

        [Test, ExpectedException(typeof (InvalidOperationException))]
        public void Exceptions_AreRethrown()
        {
            Expect.Once.On(target).Method("DoSomething").With(1, 2).Will(
                Throw.Exception(new InvalidOperationException()));
            Assert.AreEqual(3, proxy.DoSomething(1, 2));
        }

        [Test]
        public void ExplicitMethods_ArePassedThrough()
        {
            X x = new X();
            TypeSafeConduit<IX> conduit = new TypeSafeConduit<IX>();
            conduit.SetTarget(x);
            conduit.Proxy.Boing();
            Assert.IsTrue(x.wasCalled);
        }

        [Test]
        public void GetProperties_ArePassedThrough()
        {
            Expect.Once.On(target).GetProperty("Property").Will(Return.Value(4));
            Assert.AreEqual(4, proxy.Property);
        }

        [Test]
        public void Methods_ArePassedThrough()
        {
            Expect.Once.On(target).Method("DoSomething").With(1, 2).Will(Return.Value(3));
            Assert.AreEqual(3, proxy.DoSomething(1, 2));
        }

        [Test]
        public void MethodsOnTemplatedType_ArePassedThrough()
        {
            IA ia = NewMock<IA>();
            IX<IA> mockIX = new MockXIA(ia);

            ITypeSafeConduit<IX<IA>> conduit2 = new TypeSafeConduit<IX<IA>>();
            conduit2.SetTarget(mockIX);

            Assert.AreSame(ia, conduit2.Proxy.DoSomething());
        }

        private class MockXIA : IX<IA>
        {
            private readonly IA result;

            public MockXIA(IA result)
            {
                this.result = result;
            }

            public IA DoSomething()
            {
                return result;
            }
        }

        [Test]
        public void MethodsOnTemplatedType_ArePassedThrough2()
        {
            ITypeSafeConduit<IX2> conduit2 = new TypeSafeConduit<IX2>();
            IA ia = NewMock<IA>();
            IX2 mockX = new X2(ia);

            conduit2.SetTarget(mockX);

            Assert.AreSame(ia, conduit2.Proxy.DoSomething<IA>());
        }

        [Test]
        public void SetProperties_ArePassedThrough()
        {
            Expect.Once.On(target).SetProperty("Property").To(5);
            proxy.Property = 5;
        }

        public class X : IX
        {
            public bool wasCalled = false;

            void IY.Boing()
            {
                wasCalled = true;
            }
        }

        public class X2 : IX2
        {
            private readonly object obj;

            public X2(object obj)
            {
                this.obj = obj;
            }

            public T DoSomething<T>()
            {
                return (T) obj;
            }
        }

        public interface IY
        {
            void Boing();
        }

        public interface IX : IY
        {
        }
    }
}