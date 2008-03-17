using System;
using NDependencyInjection.Generics;
using NDependencyInjection.interfaces;
using NDependencyInjection.Tests.ExampleClasses;
using NMock2;
using NUnit.Framework;
using Varian.Tests.Utilities;


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

        [Test, Ignore("Can't use for templated types yet")]
        public void MethodsOnTemplatedType_ArePassedThrough()
        {
            ITypeSafeConduit<X<IA>> conduit2 = new TypeSafeConduit<X<IA>>();
            X<IA> mockX = NewMock<X<IA>>();

            conduit2.SetTarget(mockX);

            IA ia = NewMock<IA>();
            Expect.Once.On(mockX).Method("DoSomething").WithNoArguments().Will(Return.Value(ia));

            Assert.AreSame(ia, conduit2.Proxy.DoSomething());
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

        public interface IY
        {
            void Boing();
        }

        public interface IX : IY
        {
        }
    }
}