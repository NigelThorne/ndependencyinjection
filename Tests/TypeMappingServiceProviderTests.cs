using NDependencyInjection.interfaces;
using NDependencyInjection.Tests.ExampleClasses;
using NUnit.Framework;
using Rhino.Mocks;


namespace NDependencyInjection.Tests
{   
    [TestFixture]
    public class TypeMappingServiceProviderTests : RhinoMockingTestFixture
    {
        private TypeMappingServiceProvider<IA, ClassA> typeMappingServiceProvider;
        private IServiceLocator serviceLocator;

        protected override void SetUp()
        {
            serviceLocator = NewStub<IServiceLocator>();
            typeMappingServiceProvider = new TypeMappingServiceProvider<IA, ClassA>(serviceLocator);
        }

        [Test]
        public void GetService_ForwardsCallToWrappedServiceLocator()
        {
            ClassA classA = NewStub<ClassA>();
            SetupResult.For(serviceLocator.Get<ClassA>()).Return(classA);
            SetupComplete();

            Assert.AreSame(classA, typeMappingServiceProvider.GetService());
        }
    }
}