#region usings

using System;
using Moq;
using NDependencyInjection.Generics;
using NDependencyInjection.interfaces;
using NDependencyInjection.Tests.TestClasses;
using NUnit.Framework;

#endregion

namespace NDependencyInjection.Tests
{
    [TestFixture]
    public class ConduitTests : MockingTestFixture
    {
        protected override void SetUp ( )
        {
            ITypeSafeConduit<IA> typeSafeConduit = new TypeSafeConduit<IA> ();

            proxy = typeSafeConduit.Proxy;
            Assert.IsNotNull ( proxy );

            target = NewMock<IA> ();
            typeSafeConduit.SetTarget ( target );
        }

        private IA proxy;
        private IA target;

        private class MockXIA : IX<IA>
        {
            private readonly IA result;

            public MockXIA ( IA result )
            {
                this.result = result;
            }

            public IA DoSomething ( )
            {
                return result;
            }
        }

        public class X : IX
        {
            public bool wasCalled = false;

            void IY.Boing ( )
            {
                wasCalled = true;
            }
        }

        public class X2 : IX2
        {
            private readonly object obj;

            public X2 ( object obj )
            {
                this.obj = obj;
            }

            public T DoSomething<T> ( )
            {
                return (T) obj;
            }
        }

        public interface IY
        {
            void Boing ( );
        }

        public interface IX : IY
        {
        }


        [Test]
        public void Exceptions_AreRethrown ( )
        {
            Mock.Get ( target ).Setup ( t => t.DoSomething ( 1, 2 ) ).Throws ( new InvalidOperationException () );
            Assert.Throws<InvalidOperationException> ( ( ) => proxy.DoSomething ( 1, 2 ) );
        }

        [Test]
        public void ExplicitMethods_ArePassedThrough ( )
        {
            var x = new X ();
            var conduit = new TypeSafeConduit<IX> ();
            conduit.SetTarget ( x );
            conduit.Proxy.Boing ();
            Assert.IsTrue ( x.wasCalled );
        }

        [Test]
        public void GetProperties_ArePassedThrough ( )
        {
            Mock.Get ( target ).Setup ( t => t.Property ).Returns ( 4 );
            Assert.AreEqual ( 4, proxy.Property );
        }

        [Test]
        public void Methods_ArePassedThrough ( )
        {
            Mock.Get ( target ).Setup ( t => t.DoSomething ( 1, 2 ) ).Returns ( 3 );
            Assert.AreEqual ( 3, proxy.DoSomething ( 1, 2 ) );
        }

        [Test]
        public void MethodsOnTemplatedType_ArePassedThrough ( )
        {
            var ia = NewMock<IA> ();
            IX<IA> mockIX = new MockXIA ( ia );

            ITypeSafeConduit<IX<IA>> conduit2 = new TypeSafeConduit<IX<IA>> ();
            conduit2.SetTarget ( mockIX );

            Assert.AreSame ( ia, conduit2.Proxy.DoSomething () );
        }

        //[Test, Ignore("Known not to work... that's life!")]
        //public void MethodsOnTemplatedType_ArePassedThrough2()
        //{
        //    ITypeSafeConduit<IX2> conduit2 = new TypeSafeConduit<IX2>();
        //    IA ia = NewMock<IA>();
        //    IX2 mockX = new X2(ia);

        //    conduit2.SetTarget(mockX);

        //    Assert.AreSame(ia, conduit2.Proxy.DoSomething<IA>());
        //}

        [Test]
        public void SetProperties_ArePassedThrough ( )
        {
            Mock.Get ( target ).SetupProperty ( t => t.Property );
            proxy.Property = 5;
            Mock.Get ( target ).VerifySet ( t => t.Property = 5, Times.Once );
        }
    }
}