using System;
using NDependencyInjection.interfaces;
using NDependencyInjection.Tests.ExampleClasses;
using NMockExtensions;
using NUnit.Framework;
using Assert=NUnit.Framework.Assert;
using IServiceProvider=NDependencyInjection.interfaces.IServiceProvider;


namespace NDependencyInjection.Tests
{
    [TestFixture]
    public class ServiceRepositoryTests : MockingTestFixture
    {
        private ServiceRepository repository;

        protected override void SetUp()
        {
            repository = new ServiceRepository();
        }

        [Test]
        public void CanGetInstance()
        {
            Assert.IsNotNull(repository);
            Assert.IsInstanceOfType(typeof (IServiceRepository), repository);
        }

        [Test, ExpectedException(typeof (ApplicationException))]
        public void Get_RethrowsUnwrappedReflectionExceptions()
        {
            IServiceProvider serviceProvider = new MockExceptionThrowingServiceProvider();
            repository.RegisterServiceProvider<IA>(serviceProvider);
            repository.GetService(typeof (IA), typeof (IA));
        }

        [Test, ExpectedException(typeof (InvalidOperationException))]
        public void RegisterService_ReplacesOldObject_WhenSameTypeIsAddedTwice()
        {
            repository.RegisterServiceProvider<IMyTestClassA>(NewMock<IServiceProvider>());
            repository.RegisterServiceProvider<IMyTestClassA>(NewMock<IServiceProvider>());
        }

        [Test, ExpectedException(typeof (InvalidOperationException))]
        public void ResolveService_ThrowsExpection_WhenTypeNotFound()
        {
            Assert.IsNull(repository.GetService(typeof (IMyTestClassA), typeof (IMyTestClassA)));
        }

        [Test]
        public void ResolveService_TriggersTheServiceProvider_WhenAServiceProviderWasRegisteredForThatType()
        {
            IMyTestClassA a = NewMock<IMyTestClassA>();
            IServiceProvider serviceProvider = new SP(a);
            repository.RegisterServiceProvider<IMyTestClassA>(serviceProvider);

            Assert.AreSame(a, repository.GetService(typeof (IMyTestClassA), typeof (IMyTestClassA)));
            Assert.IsTrue(((SP) serviceProvider).gotCalled);
        }

        protected override void TearDown()
        {
        }

        private class MockExceptionThrowingServiceProvider : IServiceProvider
        {
            public object GetService(Type serviceType, Type interfaceType)
            {
                throw new ApplicationException();
            }
        }
    }
}