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
        private IServiceLocator context;

        protected override void SetUp()
        {
            context = NewMock<IServiceLocator>();

            serviceProvider = new FactoryServiceProvider<ClassC>();
        }

        [Test]
        public void GetService_CallsTheConstructorResolvingAllParameters_WhenThereIsOnlyOne()
        {
            IA ia = NewStub<IA>();
            IB ib = NewStub<IB>();

            SetupResult.For(context.HasService(typeof (IA))).Return(true);
            SetupResult.For(context.HasService(typeof (IB))).Return(true);
            SetupResult.For(context.GetService(typeof (IA))).Return(ia);
            SetupResult.For(context.GetService(typeof (IB))).Return(ib);
            SetupComplete();

            IC service = (IC) serviceProvider.GetService(typeof (IC), typeof (IC), context);

            Assert.AreEqual(ia, service.A);
            Assert.AreEqual(ib, service.B);
        }

        [Test]
        public void
            GetService_CallsTheConstructorThatIsAttributed_WhenThereIsMoreThanOneConstructorAndOnlyOneIsAttributed()
        {
            IServiceProvider provider =
                new FactoryServiceProvider<TwoConstructorsAttributes>();

            IA ia = NewStub<IA>();
            IB ib = NewStub<IB>();

            SetupResult.For(context.HasService(typeof (IA))).Return(true);
            SetupResult.For(context.GetService(typeof (IA))).Return(ia);
            SetupResult.For(context.HasService(typeof (IB))).Return(true);
            SetupResult.For(context.GetService(typeof (IB))).Return(ib);
            SetupComplete();

            IC service = (IC) provider.GetService(typeof (IC), typeof (IC), context);

            Assert.AreEqual(ia, service.A);
            Assert.AreEqual(ib, service.B);
        }

        [Test, ExpectedException(typeof (ApplicationException))]
        public void GetService_ThrowsException_WhenNotAllParametersAreResolvable()
        {
            SetupResult.For(context.HasService(typeof (IA))).Return(false);
            SetupResult.For(context.HasService(typeof (IB))).Return(true);
            SetupComplete();

            serviceProvider.GetService(typeof (IC), typeof (IC), context);
        }

        [Test, ExpectedException(typeof (ApplicationException))]
        public void GetService_ThrowsException_WhenThereAreMultipleConstructorsButNoneAreAttributed()
        {
            IServiceProvider provider =
                new FactoryServiceProvider<TwoConstructorsNoAttributes>();
            IA ia = NewStub<IA>();
            IB ib = NewStub<IB>();

            SetupResult.For(context.HasService(typeof (IA))).Return(true);
            SetupResult.For(context.GetService(typeof (IA))).Return(ia);
            SetupResult.For(context.HasService(typeof (IB))).Return(true);
            SetupResult.For(context.GetService(typeof (IB))).Return(ib);
            SetupComplete();
            provider.GetService(typeof (IC), typeof (IC), context);
        }

        [Test, ExpectedException(typeof (ApplicationException))]
        public void GetService_ThrowsException_WhenThereAreMultipleConstructorsThatAreAttributed()
        {
            IServiceProvider provider =
                new FactoryServiceProvider<TwoConstructorsTwoAttributes>();

            SetupComplete();
            provider.GetService(typeof (IC), typeof (IC), context);
        }
    }
}