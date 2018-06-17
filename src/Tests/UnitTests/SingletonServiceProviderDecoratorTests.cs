//Copyright (c) 2008 Nigel Thorne
using System;
using Moq;
using NDependencyInjection.interfaces;
using NDependencyInjection.Providers;
using NDependencyInjection.Tests.TestClasses;
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
        private IServiceLocator context;

        protected override void SetUp()
        {
            context = NewMock<IServiceLocator>();
            serviceProvider = NewMock<IServiceProvider>();
            conduitGeneratingServiceProviderDecorator = new SingletonServiceProviderDecorator(serviceProvider);
        }

        [Test]
        public void GetService_ReturnsAConduit_WhenAnotherCallIsInProgress()
        {
            IA realService = NewMock<IA>();
            IA conduit = null;
            MockOf(serviceProvider).Setup(s => s.GetService(It.IsAny<Type>(),It.IsAny<Type>(),It.IsAny<IServiceLocator>()))
                .Callback<Type, Type, IServiceLocator >((t1,t2,sl) =>
            {
                conduit = (IA) conduitGeneratingServiceProviderDecorator.GetService(typeof(IA), typeof(IA),context);
            }).Returns(realService);

            IA service = (IA) conduitGeneratingServiceProviderDecorator.GetService(typeof (IA), typeof (IA), context);

            MockOf(realService).Setup(t => t.DoSomething(1, 2)).Returns(3);
            Assert.AreEqual(3, service.DoSomething(1, 2));

            MockOf(realService).Setup(t => t.DoSomething(4, 6)).Returns(9);
            Assert.AreEqual(9, conduit.DoSomething(4, 6));
        }

        [Test]
        public void GetService_ReturnsTheInstance()
        {
            IA realService = NewMock<IA>();

            MockOf(serviceProvider).Setup(s => s.GetService(It.IsAny<Type>(), It.IsAny<Type>(), It.IsAny<IServiceLocator>()))
                .Returns(realService);

            IA service = (IA) conduitGeneratingServiceProviderDecorator.GetService(typeof (IA), typeof (IA), context);

            MockOf(realService).Setup(t => t.DoSomething(1, 2)).Returns(3);
            Assert.AreEqual(3, service.DoSomething(1, 2));
        }

        [Test]
        public void GetService_ReturnsTheSameInstance_TheSecondTime()
        {
            IA realService = NewMock<IA>();
            MockOf(serviceProvider).Setup(s => s.GetService(It.IsAny<Type>(), It.IsAny<Type>(), It.IsAny<IServiceLocator>()))
                .Returns(realService);

            IA service1 = (IA) conduitGeneratingServiceProviderDecorator.GetService(typeof (IA), typeof (IA), context);
            IA service2 = (IA) conduitGeneratingServiceProviderDecorator.GetService(typeof (IA), typeof (IA), context);

            MockOf(realService).Setup(t => t.DoSomething(1, 2)).Returns(3);
            Assert.AreEqual(3, service2.DoSomething(1, 2));
        }

        [Test]
        public void UsingTheConduitBeforeItIsInitialIzed_ThrowsAnException()
        {
            IA realService = NewMock<IA>();

            MockOf(serviceProvider).Setup(s => s.GetService(It.IsAny<Type>(), It.IsAny<Type>(), It.IsAny<IServiceLocator>()))
                .Callback<Type, Type, IServiceLocator>((t1, t2, sl) =>
                {
                    ((IA)conduitGeneratingServiceProviderDecorator.GetService(typeof(IA), typeof(IA), context)).DoSomething(4,6);
                }).Returns(realService);



            Assert.Throws<NullReferenceException>(() => conduitGeneratingServiceProviderDecorator.GetService(typeof (IA), typeof (IA), context));
        }
    }
}