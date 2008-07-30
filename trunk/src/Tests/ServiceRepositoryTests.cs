//Copyright (c) 2008 Nigel Thorne
using System;
using NDependencyInjection.Exceptions;
using NDependencyInjection.interfaces;
using NDependencyInjection.Tests.ExampleClasses;
using NMock2;
using NMockExtensions;
using NUnit.Framework;
using Assert=NUnit.Framework.Assert;
using IServiceProvider=NDependencyInjection.interfaces.IServiceProvider;


namespace NDependencyInjection.Tests
{
    [TestFixture]
    public class ServiceRepositoryTests : MockingTestFixture
    {
        private Scope repository;

        protected override void SetUp()
        {
            repository = new Scope();
        }

        [Test]
        public void CanGetInstance()
        {
            Assert.IsNotNull(repository);
            Assert.IsInstanceOfType(typeof (IScope), repository);
        }

        [Test, ExpectedException(typeof (ApplicationException))]
        public void Get_RethrowsUnwrappedReflectionExceptions()
        {
            IServiceProvider serviceProvider = new MockExceptionThrowingServiceProvider();
            repository.RegisterServiceProvider(typeof(IA),serviceProvider);
            repository.GetService(typeof (IA));
        }

        [Test, ExpectedException(typeof (InvalidOperationException))]
        public void RegisterService_ThrowsException_WhenSameTypeIsRegisteredTwice()
        {
            IServiceProvider mock = NewMock<IServiceProvider>();
            Stub.On(mock).Method("AddMapping");
            repository.RegisterServiceProvider(typeof(IMyTestClassA),mock);
            repository.RegisterServiceProvider(typeof(IMyTestClassA),NewMock<IServiceProvider>());
        }

        [Test, ExpectedException(typeof(UnknownTypeException))]
        public void ResolveService_ThrowsExpection_WhenTypeNotFound()
        {
            Assert.IsNull(repository.GetService(typeof (IMyTestClassA)));
        }

        [Test]
        public void ResolveService_TriggersTheServiceProvider_WhenAServiceProviderWasRegisteredForThatType()
        {
            IMyTestClassA a = NewMock<IMyTestClassA>();
            IServiceProvider serviceProvider = new SP(a);
            repository.RegisterServiceProvider(typeof(IMyTestClassA),serviceProvider);

            Assert.AreSame(a, repository.GetService(typeof (IMyTestClassA)));
            Assert.IsTrue(((SP) serviceProvider).gotCalled);
        }

        protected override void TearDown()
        {
        }

        private class MockExceptionThrowingServiceProvider : IServiceProvider
        {
            public object GetService(Type serviceType, Type interfaceType, IServiceLocator context)
            {
                throw new ApplicationException();
            }

            public void AddMapping(Type serviceType)
            {
            }
        }
    }
}