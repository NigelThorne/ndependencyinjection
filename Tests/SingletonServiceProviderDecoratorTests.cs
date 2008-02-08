using System;
using System.Reflection;
using NDependencyInjection.interfaces;
using NDependencyInjection.Tests.ExampleClasses;
using NMock2;
using NUnit.Framework;
using Varian.NMockExtensions;
using Varian.Tests.Utilities;
using Assert=NUnit.Framework.Assert;


namespace NDependencyInjection.Tests
{
    [TestFixture]
    public class SingletonServiceProviderDecoratorTests : MockingTestFixture
    {
        private SingletonServiceProviderDecorator<IA> conduitGeneratingServiceProviderDecorator;
        private IServiceProvider<IA> serviceProvider;

        protected override void SetUp()
        {
            serviceProvider = NewMock<IServiceProvider<IA>>();
            conduitGeneratingServiceProviderDecorator = new SingletonServiceProviderDecorator<IA>(serviceProvider);
        }

        [Test]
        public void GetService_ReturnsAConduit_WhenAnotherCallIsInProgress()
        {
            IA realService = NewMock<IA>();
            IA conduit = null;
            Expect.Once.On(serviceProvider).Method("GetService").Will(
                Execute.Delegate(delegate { conduit = conduitGeneratingServiceProviderDecorator.GetService(); }),
                Return.Value(realService));

            IA service = conduitGeneratingServiceProviderDecorator.GetService();

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

            IA service = conduitGeneratingServiceProviderDecorator.GetService();

            Expect.Once.On(realService).Method("DoSomething").With(1, 2).Will(Return.Value(3));
            Assert.AreEqual(3, service.DoSomething(1, 2));
        }

        [Test]
        public void GetService_ReturnsTheSameInstance_TheSecondTime()
        {
            IA realService = NewMock<IA>();
            Expect.Once.On(serviceProvider).Method("GetService").Will(
                Return.Value(realService));

            IA service1 = conduitGeneratingServiceProviderDecorator.GetService();
            IA service2 = conduitGeneratingServiceProviderDecorator.GetService();

            Expect.Once.On(realService).Method("DoSomething").With(1, 2).Will(Return.Value(3));
            Assert.AreEqual(3, service2.DoSomething(1, 2));
        }

        [Test, ExpectedException(typeof (TargetException))]
        public void UsingTheConduitBeforeItIsInitialIzed_ThrowsAnException()
        {
            IA realService = NewMock<IA>();
            Expect.Once.On(serviceProvider).Method("GetService").Will(
                Execute.Delegate(delegate { conduitGeneratingServiceProviderDecorator.GetService().DoSomething(4, 6); }),
                Return.Value(realService));

            conduitGeneratingServiceProviderDecorator.GetService();
        }
    }
}