//Copyright (c) 2008 Nigel Thorne
using System;
using NDependencyInjection.interfaces;
using NDependencyInjection.Tests.ExampleClasses;
using NMock2;
using NUnit.Framework;
using Varian.Tests.Utilities;


namespace NDependencyInjection.Tests
{
    [TestFixture]
    public class DependencyResolvingServiceProviderTests : MockingTestFixture
    {
        private DependencyResolvingServiceProvider<IC, ClassC> dependencyResolvingServiceProvider;
        private IServiceLocator serviceLocator;

        protected override void SetUp()
        {
            serviceLocator = NewMock<IServiceLocator>();
            dependencyResolvingServiceProvider = new DependencyResolvingServiceProvider<IC, ClassC>(serviceLocator);
        }

        [Test]
        public void GetService_CallsTheConstructorResolvingAllParameters()
        {
            IA ia = NewMock<IA>();
            IB ib = NewMock<IB>();

            Stub.On(serviceLocator).Method("Has").WithAnyArguments().Will(Return.Value(true));
            Stub.On(serviceLocator).Method("Get").With(typeof (IA)).Will(Return.Value(ia));
            Stub.On(serviceLocator).Method("Get").With(typeof (IB)).Will(Return.Value(ib));

            IC service = dependencyResolvingServiceProvider.GetService();

            Assert.AreEqual(ia, service.A);
            Assert.AreEqual(ib, service.B);
        }

        [Test, ExpectedException(typeof(ApplicationException))]
        public void GetService_ThrowsException_WhenNoCOnstructorIsPossible()
        {
            IA ia = NewMock<IA>();
            IB ib = NewMock<IB>();

            Stub.On(serviceLocator).Method("Has").With(typeof (IA)).Will(Return.Value(true));
            Stub.On(serviceLocator).Method("Get").With(typeof (IA)).Will(Return.Value(ia));
            Stub.On(serviceLocator).Method("Has").With(typeof (IB)).Will(Return.Value(false));

            IC service = dependencyResolvingServiceProvider.GetService();

            Assert.AreEqual(ia, service.A);
            Assert.AreEqual(ib, service.B);
        }
    }
}