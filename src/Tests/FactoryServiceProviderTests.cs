//Copyright (c) 2008 Nigel Thorne
using System;
using NDependencyInjection.interfaces;
using NDependencyInjection.Providers;
using NDependencyInjection.Tests.ExampleClasses;
using NUnit.Framework;
using Rhino.Mocks;
using IServiceProvider=NDependencyInjection.interfaces.IServiceProvider;


namespace NDependencyInjection.Tests
{
    [TestFixture]
    public class FactoryServiceProviderTests : RhinoMockingTestFixture
    {
        private IServiceProvider serviceProvider;
        private IServiceLocator serviceLocator;

        protected override void SetUp()
        {
            serviceLocator = NewMock<IServiceLocator>();

            serviceProvider = new DecoratingServiceProvider<ClassC>(serviceLocator);
        }

        [Test]
        public void GetService_CallsTheConstructorResolvingAllParameters_WhenThereIsOnlyOne()
        {
            IA ia = NewStub<IA>();
            IB ib = NewStub<IB>();

            SetupResult.For(serviceLocator.HasService(typeof (IA))).Return(true);
            SetupResult.For(serviceLocator.HasService(typeof (IB))).Return(true);
            SetupResult.For(serviceLocator.GetService(typeof (IA))).Return(ia);
            SetupResult.For(serviceLocator.GetService(typeof (IB))).Return(ib);
            SetupComplete();

            IC service = (IC) serviceProvider.GetService(typeof (IC), typeof (IC));

            Assert.AreEqual(ia, service.A);
            Assert.AreEqual(ib, service.B);
        }

        [Test]
        public void
            GetService_CallsTheConstructorThatIsAttributed_WhenThereIsMoreThanOneConstructorAndOnlyOneIsAttributed()
        {
            IServiceProvider provider =
                new DecoratingServiceProvider<TwoConstructorsAttributes>(serviceLocator);

            IA ia = NewStub<IA>();
            IB ib = NewStub<IB>();

            SetupResult.For(serviceLocator.HasService(typeof (IA))).Return(true);
            SetupResult.For(serviceLocator.GetService(typeof (IA))).Return(ia);
            SetupResult.For(serviceLocator.HasService(typeof (IB))).Return(true);
            SetupResult.For(serviceLocator.GetService(typeof (IB))).Return(ib);
            SetupComplete();

            IC service = (IC) provider.GetService(typeof (IC), typeof (IC));

            Assert.AreEqual(ia, service.A);
            Assert.AreEqual(ib, service.B);
        }

        [Test, ExpectedException(typeof (ApplicationException))]
        public void GetService_ThrowsException_WhenNotAllParametersAreResolvable()
        {
            SetupResult.For(serviceLocator.HasService(typeof (IA))).Return(false);
            SetupResult.For(serviceLocator.HasService(typeof (IB))).Return(true);
            SetupComplete();

            serviceProvider.GetService(typeof (IC), typeof (IC));
        }

        [Test, ExpectedException(typeof (ApplicationException))]
        public void GetService_ThrowsException_WhenThereAreMultipleConstructorsButNoneAreAttributed()
        {
            IServiceProvider provider =
                new DecoratingServiceProvider<TwoConstructorsNoAttributes>(serviceLocator);
            IA ia = NewStub<IA>();
            IB ib = NewStub<IB>();

            SetupResult.For(serviceLocator.HasService(typeof (IA))).Return(true);
            SetupResult.For(serviceLocator.GetService(typeof (IA))).Return(ia);
            SetupResult.For(serviceLocator.HasService(typeof (IB))).Return(true);
            SetupResult.For(serviceLocator.GetService(typeof (IB))).Return(ib);
            SetupComplete();
            provider.GetService(typeof (IC), typeof (IC));
        }

        [Test, ExpectedException(typeof (ApplicationException))]
        public void GetService_ThrowsException_WhenThereAreMultipleConstructorsThatAreAttributed()
        {
            IServiceProvider provider =
                new DecoratingServiceProvider<TwoConstructorsTwoAttributes>(serviceLocator);

            SetupComplete();
            provider.GetService(typeof (IC), typeof (IC));
        }
    }
}