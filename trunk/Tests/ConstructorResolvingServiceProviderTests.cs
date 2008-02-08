using System;
using NDependencyInjection.interfaces;
using NDependencyInjection.Tests.ExampleClasses;
using NUnit.Framework;
using Rhino.Mocks;


namespace NDependencyInjection.Tests
{
    [TestFixture]
    public class ConstructorResolvingServiceProviderTests : RhinoMockingTestFixture
    {
        private IServiceLocator serviceLocator;
        private ConstructorResolvingServiceProvider<ClassC> resolvingServiceProvider;

        protected override void SetUp()
        {
            serviceLocator = NewMock<IServiceLocator>();
            resolvingServiceProvider = new ConstructorResolvingServiceProvider<ClassC>(serviceLocator);
        }

        [Test]
        public void GetService_CallsTheConstructorResolvingAllParameters_WhenThereIsOnlyOne()
        {
            IA ia = NewStub<IA>();
            IB ib = NewStub<IB>();

            SetupResult.For(serviceLocator.Has<IA>()).Return(true);
            SetupResult.For(serviceLocator.Has<IB>()).Return(true);
            SetupResult.For(serviceLocator.Get<IA>()).Return(ia);
            SetupResult.For(serviceLocator.Get<IB>()).Return(ib);
            SetupComplete();

            IC service = resolvingServiceProvider.GetService();

            Assert.AreEqual(ia, service.A);
            Assert.AreEqual(ib, service.B);
        }

        [Test]
        public void
            GetService_CallsTheConstructorThatIsAttributed_WhenThereIsMoreThanOneConstructorAndOnlyOneIsAttributed()
        {
            ConstructorResolvingServiceProvider<TwoConstructorsAttributes> provider =
                new ConstructorResolvingServiceProvider<TwoConstructorsAttributes>(serviceLocator);

            IA ia = NewStub<IA>();
            IB ib = NewStub<IB>();

            SetupResult.For(serviceLocator.Has<IA>()).Return(true);
            SetupResult.For(serviceLocator.Get<IA>()).Return(ia);
            SetupResult.For(serviceLocator.Has<IB>()).Return(true);
            SetupResult.For(serviceLocator.Get<IB>()).Return(ib);
            SetupComplete();

            IC service = provider.GetService();

            Assert.AreEqual(ia, service.A);
            Assert.AreEqual(ib, service.B);
        }

        [Test, ExpectedException(typeof (ApplicationException))]
        public void GetService_ThrowsException_WhenNotAllParametersAreResolvable()
        {
            SetupResult.For(serviceLocator.Has<IA>()).Return(false);
            SetupResult.For(serviceLocator.Has<IB>()).Return(true);
            SetupComplete();

            resolvingServiceProvider.GetService();
        }

        [Test, ExpectedException(typeof (ApplicationException))]
        public void GetService_ThrowsException_WhenThereAreMultipleConstructorsButNoneAreAttributed()
        {
            ConstructorResolvingServiceProvider<TwoConstructorsNoAttributes> provider =
                new ConstructorResolvingServiceProvider<TwoConstructorsNoAttributes>(serviceLocator);
            IA ia = NewStub<IA>();
            IB ib = NewStub<IB>();

            SetupResult.For(serviceLocator.Has<IA>()).Return(true);
            SetupResult.For(serviceLocator.Get<IA>()).Return(ia);
            SetupResult.For(serviceLocator.Has<IB>()).Return(true);
            SetupResult.For(serviceLocator.Get<IB>()).Return(ib);
            SetupComplete();
            provider.GetService();
        }

        [Test, ExpectedException(typeof (ApplicationException))]
        public void GetService_ThrowsException_WhenThereAreMultipleConstructorsThatAreAttributed()
        {
            ConstructorResolvingServiceProvider<TwoConstructorsTwoAttributes> provider =
                new ConstructorResolvingServiceProvider<TwoConstructorsTwoAttributes>(serviceLocator);

            SetupComplete();
            provider.GetService();
        }
    }
}