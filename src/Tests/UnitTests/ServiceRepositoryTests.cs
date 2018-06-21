#region usings

using System;
using NDependencyInjection.Exceptions;
using NDependencyInjection.interfaces;
using NDependencyInjection.Tests.TestClasses;
using NUnit.Framework;
using IServiceProvider = NDependencyInjection.interfaces.IServiceProvider;

#endregion

namespace NDependencyInjection.Tests
{
    [TestFixture]
    public class ServiceRepositoryTests : MockingTestFixture
    {
        protected override void SetUp ( )
        {
            _repository = new Scope ();
        }

        protected override void TearDown ( )
        {
        }

        private Scope _repository;

        private class MockExceptionThrowingServiceProvider : IServiceProvider
        {
            public object GetService ( Type serviceType, Type interfaceType, IServiceLocator context )
            {
                throw new ApplicationException ();
            }

            public void AddMapping ( Type serviceType )
            {
            }
        }

        [Test]
        public void CanGetInstance ( )
        {
            Assert.IsNotNull ( _repository as IScope );
        }

        [Test]
        public void Get_RethrowsUnwrappedReflectionExceptions ( )
        {
            IServiceProvider serviceProvider = new MockExceptionThrowingServiceProvider ();
            _repository.RegisterServiceProvider ( typeof (IA), serviceProvider );
            Assert.Throws<ApplicationException> ( ( ) => _repository.GetService ( typeof (IA) ) );
        }

        [Test]
        public void RegisterService_ThrowsException_WhenSameTypeIsRegisteredTwice ( )
        {
            var mock = NewMock<IServiceProvider> ();
            _repository.RegisterServiceProvider ( typeof (IMyTestClassA), mock );
            Assert.Throws<InvalidOperationException> ( ( ) =>
                _repository.RegisterServiceProvider ( typeof (IMyTestClassA), NewMock<IServiceProvider> () ) );
        }

        [Test]
        public void ResolveService_ThrowsExpection_WhenTypeNotFound ( )
        {
            Assert.Throws<UnknownTypeException> ( ( ) => _repository.GetService ( typeof (IMyTestClassA) ) );
        }

        [Test]
        public void ResolveService_TriggersTheServiceProvider_WhenAServiceProviderWasRegisteredForThatType ( )
        {
            var a = NewMock<IMyTestClassA> ();
            IServiceProvider serviceProvider = new TestSP ( a );
            _repository.RegisterServiceProvider ( typeof (IMyTestClassA), serviceProvider );

            Assert.AreSame ( a, _repository.GetService ( typeof (IMyTestClassA) ) );
            Assert.IsTrue ( ( (TestSP) serviceProvider ).gotCalled );
        }
    }
}