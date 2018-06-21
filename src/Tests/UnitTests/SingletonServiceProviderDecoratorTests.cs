#region usings

using System;
using Moq;
using NDependencyInjection.interfaces;
using NDependencyInjection.Providers;
using NDependencyInjection.Tests.TestClasses;
using NUnit.Framework;
using IServiceProvider = NDependencyInjection.interfaces.IServiceProvider;

#endregion

namespace NDependencyInjection.Tests
{
    [TestFixture]
    public class SingletonServiceProviderDecoratorTests : MockingTestFixture
    {
        protected override void SetUp ( )
        {
            context = NewMock<IServiceLocator> ();
            serviceProvider = NewMock<IServiceProvider> ();
            conduitGeneratingServiceProviderDecorator = new SingletonServiceProviderDecorator ( serviceProvider );
        }

        private SingletonServiceProviderDecorator conduitGeneratingServiceProviderDecorator;
        private IServiceProvider serviceProvider;
        private IServiceLocator context;

        [Test]
        public void GetService_ReturnsAConduit_WhenAnotherCallIsInProgress ( )
        {
            var realService = NewMock<IA> ();
            IA conduit = null;
            MockOf ( serviceProvider ).Setup ( s =>
                    s.GetService ( It.IsAny<Type> (), It.IsAny<Type> (), It.IsAny<IServiceLocator> () ) )
                .Callback<Type, Type, IServiceLocator> ( ( t1, t2, sl ) =>
                {
                    conduit = (IA) conduitGeneratingServiceProviderDecorator.GetService ( typeof (IA), typeof (IA),
                        context );
                } ).Returns ( realService );

            var service =
                (IA) conduitGeneratingServiceProviderDecorator.GetService ( typeof (IA), typeof (IA), context );

            MockOf ( realService ).Setup ( t => t.DoSomething ( 1, 2 ) ).Returns ( 3 );
            Assert.AreEqual ( 3, service.DoSomething ( 1, 2 ) );

            MockOf ( realService ).Setup ( t => t.DoSomething ( 4, 6 ) ).Returns ( 9 );
            Assert.AreEqual ( 9, conduit.DoSomething ( 4, 6 ) );
        }

        [Test]
        public void GetService_ReturnsTheInstance ( )
        {
            var realService = NewMock<IA> ();

            MockOf ( serviceProvider ).Setup ( s =>
                    s.GetService ( It.IsAny<Type> (), It.IsAny<Type> (), It.IsAny<IServiceLocator> () ) )
                .Returns ( realService );

            var service =
                (IA) conduitGeneratingServiceProviderDecorator.GetService ( typeof (IA), typeof (IA), context );

            MockOf ( realService ).Setup ( t => t.DoSomething ( 1, 2 ) ).Returns ( 3 );
            Assert.AreEqual ( 3, service.DoSomething ( 1, 2 ) );
        }

        [Test]
        public void GetService_ReturnsTheSameInstance_TheSecondTime ( )
        {
            var realService = NewMock<IA> ();
            MockOf ( serviceProvider ).Setup ( s =>
                    s.GetService ( It.IsAny<Type> (), It.IsAny<Type> (), It.IsAny<IServiceLocator> () ) )
                .Returns ( realService );

            var service1 =
                (IA) conduitGeneratingServiceProviderDecorator.GetService ( typeof (IA), typeof (IA), context );
            var service2 =
                (IA) conduitGeneratingServiceProviderDecorator.GetService ( typeof (IA), typeof (IA), context );

            MockOf ( realService ).Setup ( t => t.DoSomething ( 1, 2 ) ).Returns ( 3 );
            Assert.AreEqual ( 3, service2.DoSomething ( 1, 2 ) );
        }

        [Test]
        public void UsingTheConduitBeforeItIsInitialIzed_ThrowsAnException ( )
        {
            var realService = NewMock<IA> ();

            MockOf ( serviceProvider ).Setup ( s =>
                    s.GetService ( It.IsAny<Type> (), It.IsAny<Type> (), It.IsAny<IServiceLocator> () ) )
                .Callback<Type, Type, IServiceLocator> ( ( t1, t2, sl ) =>
                {
                    ( (IA) conduitGeneratingServiceProviderDecorator.GetService ( typeof (IA), typeof (IA),
                        context ) ).DoSomething ( 4, 6 );
                } ).Returns ( realService );


            Assert.Throws<NullReferenceException> ( ( ) =>
                conduitGeneratingServiceProviderDecorator.GetService ( typeof (IA), typeof (IA), context ) );
        }
    }
}