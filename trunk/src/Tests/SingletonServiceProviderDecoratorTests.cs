using System;
using System.Reflection;
using NDependencyInjection.Tests.ExampleClasses;
using NMock2;
using NMockExtensions;
using NUnit.Framework;
using Assert=NUnit.Framework.Assert;
using IServiceProvider=NDependencyInjection.interfaces.IServiceProvider;


namespace NDependencyInjection.Tests
{
    [TestFixture]
    public class SingletonServiceProviderDecoratorTests : MockingTestFixture
    {
        private SingletonServiceProviderDecorator conduitGeneratingServiceProviderDecorator;
        private IServiceProvider serviceProvider;

        protected override void SetUp()
        {
            serviceProvider = NewMock<IServiceProvider>();
            conduitGeneratingServiceProviderDecorator = new SingletonServiceProviderDecorator(serviceProvider);
        }

        [Test]
        public void GetService_ReturnsAConduit_WhenAnotherCallIsInProgress()
        {
            IA realService = NewMock<IA>();
            IA conduit = null;
            Expect.Once.On(serviceProvider).Method("GetService").Will(
                Execute.Delegate(
                    delegate { conduit = (IA) conduitGeneratingServiceProviderDecorator.GetService(typeof (IA), typeof (IA)); }),
                Return.Value(realService));

            IA service = (IA) conduitGeneratingServiceProviderDecorator.GetService(typeof (IA));

            Expect.Once.On(realService).Method("DoSomething").With(1, 2).Will(Return.Value(3));
            Assert.AreEqual(3, service.DoSomething(1, 2));

            Expect.Once.On(realService).Method("DoSomething").With(4, 6).Will(Return.Value(9));
            Assert.AreEqual(9, conduit.DoSomething(4, 6));
        }

        [Test]
        public void GetService_ReturnsTheInstance()
        {
            IA realService = NewMock<IA>();
            Expect.Once.On(serviceProvider).Method("GetService").Will(
                Return.Value(realService));

            IA service = (IA) conduitGeneratingServiceProviderDecorator.GetService(typeof (IA));

            Expect.Once.On(realService).Method("DoSomething").With(1, 2).Will(Return.Value(3));
            Assert.AreEqual(3, service.DoSomething(1, 2));
        }

        [Test]
        public void GetService_ReturnsTheSameInstance_TheSecondTime()
        {
            IA realService = NewMock<IA>();
            Expect.Once.On(serviceProvider).Method("GetService").Will(
                Return.Value(realService));

            IA service1 = (IA) conduitGeneratingServiceProviderDecorator.GetService(typeof (IA));
            IA service2 = (IA) conduitGeneratingServiceProviderDecorator.GetService(typeof (IA));

            Expect.Once.On(realService).Method("DoSomething").With(1, 2).Will(Return.Value(3));
            Assert.AreEqual(3, service2.DoSomething(1, 2));
        }

        [Test, ExpectedException(typeof(NullReferenceException))]
        public void UsingTheConduitBeforeItIsInitialIzed_ThrowsAnException()
        {
            IA realService = NewMock<IA>();
            Expect.Once.On(serviceProvider).Method("GetService").Will(
                Execute.Delegate(
                    delegate { ((IA) conduitGeneratingServiceProviderDecorator.GetService(typeof (IA))).DoSomething(4, 6); }),
                Return.Value(realService));

            conduitGeneratingServiceProviderDecorator.GetService(typeof (IA), typeof (IA));
        }
    }
}