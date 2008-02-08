using System;
using LinFu.DynamicProxy;
using LinFu.Reflection;
using NDependencyInjection.interfaces;
using NDependencyInjection.Tests.ExampleClasses;
using NMock2;
using NUnit.Framework;
using Varian.Tests.Utilities;


namespace NDependencyInjection.Tests
{
    [TestFixture]
    public class ConduitTests : MockingTestFixture
    {
        private IA proxy;
        private IA target;

        protected override void SetUp()
        {
            IConduit<IA> conduit = new Conduit<IA>();

            proxy = conduit.Proxy;
            Assert.IsNotNull(proxy);

            target = NewMock<IA>();
            conduit.SetTarget(target);
        }

        [Test]
        public void Methods_ArePassedThrough()
        {
            Expect.Once.On(target).Method("DoSomething").With(1, 2).Will(Return.Value(3));
            Assert.AreEqual(3, proxy.DoSomething(1, 2));
        }

        [Test]
        public void GetProperties_ArePassedThrough()
        {
            Expect.Once.On(target).GetProperty("Property").Will(Return.Value(4));
            Assert.AreEqual(4, proxy.Property);
        }

        [Test]
        public void SetProperties_ArePassedThrough()
        {
            Expect.Once.On(target).SetProperty("Property").To(5);
            proxy.Property = 5;
        }

        [Test, ExpectedException(typeof(InvalidOperationException))]
        public void Exceptions_AreRethrown()
        {
            Expect.Once.On(target).Method("DoSomething").With(1, 2).Will(Throw.Exception(new InvalidOperationException()));
            Assert.AreEqual(3, proxy.DoSomething(1, 2));
        }

        [Test, Ignore("Can't use for templated types yet")]
        public void MethodsOnTemplatedType_ArePassedThrough()
        {
            IConduit<X<IA>> conduit2 = new Conduit<X<IA>>();
            X<IA> mockX = NewMock<X<IA>>();

            conduit2.SetTarget(mockX);

            IA ia = NewMock<IA>();
            Expect.Once.On(mockX).Method("DoSomething").WithNoArguments().Will(Return.Value(ia));
            
            Assert.AreSame(ia, conduit2.Proxy.DoSomething());
        }

        [Test]
        public void VirtualPropetiesOnConcreteClasses_AreForwarded()
        {
            IA ia = NewMock<IA>();
            Conduit<ClassB> conduit = new Conduit<ClassB>();
            ClassB proxyB = conduit.Proxy;
            conduit.SetTarget(new ClassB(ia));
            Assert.AreSame(ia, proxyB.A);
        }

        [Test, Ignore("Non virtual properties/methods are not forwarded yet")]
        public void NonVirtualPropetiesOnConcreteClasses_AreForwarded()
        {
            ProxyFactory factory = new ProxyFactory(); 
            IA ia = NewMock<IA>();
            IInterceptor interceptor = NewMock<IInterceptor>();
            ClassB proxyB = factory.CreateProxy<ClassB>(interceptor);
            Expect.Once.On(interceptor).Method("Intercept").WithAnyArguments().Will(Return.Value(ia));
            IA a = proxyB.A;
        }
        
        [Test, Ignore("Non virtual properties are not forwarded yet")]
        public void NonVirtualPropetiesOnConcreteClasses_AreForwardedDDD()
        {
            DynamicObject obj = new DynamicObject();
//            IProxy p = obj as IProxy;
//            IInterceptor interceptor = NewMock<IInterceptor>();
//            p.Interceptor = interceptor;
            IA ia = NewMock<IA>();
            obj.MixWith(new ClassB(ia));

            ClassB proxyB = obj.CreateDuck<ClassB>();
            Assert.AreEqual(ia, proxyB.A);
        }

        [Test]
        public void method_action_condition()
        {
            GreeterMixin first = new GreeterMixin("Hello, World!");
            GreeterMixin second = new GreeterMixin("Hello, CodeProject!");

            DynamicObject dynamic = new DynamicObject();
            dynamic.MixWith(first);
            dynamic.MixWith(second);

            // This will display "Hello, World!"

            dynamic.Methods["Greet"]();

            GreeterMixin proxyB = dynamic.CreateDuck<GreeterMixin>();
            proxyB.Greet();

        }
    }
    public class GreeterMixin
    {
        private string _message;
        public GreeterMixin(string message)
        {
            _message = message;
        }
        public void Greet()
        {
            Console.WriteLine(_message);
        }
    }

}