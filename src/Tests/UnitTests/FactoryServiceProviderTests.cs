#region usings

using System;
using NDependencyInjection.interfaces;
using NDependencyInjection.Providers;
using NDependencyInjection.Tests.TestClasses;
using NUnit.Framework;
using IServiceProvider = NDependencyInjection.interfaces.IServiceProvider;

#endregion

namespace NDependencyInjection.Tests
{
    [TestFixture]
    public class FactoryServiceProviderTests : MockingTestFixture
    {
        protected override void SetUp ( )
        {
            context = NewMock<IServiceLocator> ();

            serviceProvider = new FactoryServiceProvider<ClassC> ();
        }

        private IServiceProvider serviceProvider;
        private IServiceLocator context;

        [Test]
        public void AddMapping_DoesNothing_WhenTheMappingRefersToSupportedService ( )
        {
            serviceProvider.AddMapping ( typeof (object) );
        }

        [Test]
        public void AddMapping_ThrowsAnException_WhenTheMappingRefersToAServiceYouDontSupport ( )
        {
            IServiceProvider provider =
                new FactoryServiceProvider<object> ();

            Assert.Throws<InvalidWiringException> ( ( ) => provider.AddMapping ( typeof (IA) ) );
        }

        [Test]
        public void GetService_CallsTheConstructorResolvingAllParameters_WhenThereIsOnlyOne ( )
        {
            var ia = NewStub<IA> ();
            var ib = NewStub<IB> ();

            MockOf ( context ).Setup ( c => c.HasService ( typeof (IA) ) ).Returns ( true );
            MockOf ( context ).Setup ( c => c.HasService ( typeof (IB) ) ).Returns ( true );
            MockOf ( context ).Setup ( c => c.GetService ( typeof (IA) ) ).Returns ( ia );
            MockOf ( context ).Setup ( c => c.GetService ( typeof (IB) ) ).Returns ( ib );

            var service = (IC) serviceProvider.GetService ( typeof (IC), typeof (IC), context );

            Assert.AreEqual ( ia, service.A );
            Assert.AreEqual ( ib, service.B );
        }

        [Test]
        public void
            GetService_CallsTheConstructorThatIsAttributed_WhenThereIsMoreThanOneConstructorAndOnlyOneIsAttributed ( )
        {
            IServiceProvider provider =
                new FactoryServiceProvider<TwoConstructorsAttributes> ();

            var ia = NewStub<IA> ();
            var ib = NewStub<IB> ();

            MockOf ( context ).Setup ( c => c.HasService ( typeof (IA) ) ).Returns ( true );
            MockOf ( context ).Setup ( c => c.GetService ( typeof (IA) ) ).Returns ( ia );
            MockOf ( context ).Setup ( c => c.HasService ( typeof (IB) ) ).Returns ( true );
            MockOf ( context ).Setup ( c => c.GetService ( typeof (IB) ) ).Returns ( ib );

            var service = (IC) provider.GetService ( typeof (IC), typeof (IC), context );

            Assert.AreEqual ( ia, service.A );
            Assert.AreEqual ( ib, service.B );
        }

        [Test]
        public void GetService_ThrowsException_WhenNotAllParametersAreResolvable ( )
        {
            MockOf ( context ).Setup ( c => c.HasService ( typeof (IA) ) ).Returns ( false );
            MockOf ( context ).Setup ( c => c.HasService ( typeof (IB) ) ).Returns ( true );

            Assert.Throws<ApplicationException> ( ( ) =>
                serviceProvider.GetService ( typeof (IC), typeof (IC), context ) );
        }

        [Test]
        public void GetService_ThrowsException_WhenThereAreMultipleConstructorsButNoneAreAttributed ( )
        {
            IServiceProvider provider =
                new FactoryServiceProvider<TwoConstructorsNoAttributes> ();
            var ia = NewStub<IA> ();
            var ib = NewStub<IB> ();

            MockOf ( context ).Setup ( c => c.HasService ( typeof (IA) ) ).Returns ( true );
            MockOf ( context ).Setup ( c => c.GetService ( typeof (IA) ) ).Returns ( ia );
            MockOf ( context ).Setup ( c => c.HasService ( typeof (IB) ) ).Returns ( true );
            MockOf ( context ).Setup ( c => c.GetService ( typeof (IB) ) ).Returns ( ib );

            Assert.Throws<ApplicationException> ( ( ) => provider.GetService ( typeof (IC), typeof (IC), context ) );
        }

        [Test]
        public void GetService_ThrowsException_WhenThereAreMultipleConstructorsThatAreAttributed ( )
        {
            IServiceProvider provider =
                new FactoryServiceProvider<TwoConstructorsTwoAttributes> ();

            Assert.Throws<ApplicationException> ( ( ) => provider.GetService ( typeof (IC), typeof (IC), context ) );
        }
    }
}