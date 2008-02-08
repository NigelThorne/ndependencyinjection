using System;
using NDependencyInjection.interfaces;
using NDependencyInjection.Tests.ExampleClasses;
using NUnit.Framework;
using Varian.Tests.Utilities;


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

        [Test, ExpectedException(typeof (InvalidOperationException))]
        public void RegisterService_ReplacesOldObject_WhenSameTypeIsAddedTwice()
        {
            repository.RegisterService(NewMock<IServiceProvider<IMyTestClassA>>());
            repository.RegisterService(NewMock<IServiceProvider<IMyTestClassA>>());
        }

        [Test, ExpectedException(typeof (InvalidOperationException))]
        public void ResolveService_ThrowsExpection_WhenTypeNotFound()
        {
            Assert.IsNull(repository.Get<IMyTestClassA>());
        }

        [Test]
        public void ResolveService_TriggersTheServiceProvider_WhenAServiceProviderWasRegisteredForThatType()
        {
            IMyTestClassA a = NewMock<IMyTestClassA>();
            IServiceProvider<IMyTestClassA> serviceProvider = new SP(a);
            repository.RegisterService(serviceProvider);

            Assert.AreSame(a, repository.Get<IMyTestClassA>());
            Assert.IsTrue(((SP) serviceProvider).gotCalled);
        }

        [Test, ExpectedException(typeof(ApplicationException))]
        public void Get_RethrowsUnwrappedReflectionExceptions()
        {
            IServiceProvider<IA> serviceProvider = new MockExceptionThrowingServiceProvider();
            repository.RegisterService(serviceProvider);
            repository.Get<IA>();
        }

        protected override void TearDown()
        {
        }

        class MockExceptionThrowingServiceProvider: IServiceProvider<IA>
        {
            public IA GetService()
            {
                throw new ApplicationException();
            }
        }
    }
}