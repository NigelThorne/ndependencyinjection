using System;
using NDependencyInjection.Tests.ExampleClasses;
using NUnit.Framework;
using Varian.Tests.Utilities;


namespace NDependencyInjection.Tests
{
    [TestFixture]
    public class InjectionScopeTests : MockingTestFixture
    {
        private InjectionScope injectionScope;

        protected override void SetUp()
        {
            injectionScope = new InjectionScope();
        }

        [Test]
        public void Get_ReturnsANewInstanceOfConcreteType_EachTimeGetIsCalledOnceTypeIsRegistered()
        {
            injectionScope.Bind<IA, ClassA>();
            IA a1 = injectionScope.Get<IA>();
            IA a2 = injectionScope.Get<IA>();
            Assert.IsNotNull(a1);
            Assert.IsNotNull(a2);
            Assert.AreNotSame(a1, a2);
        }

        [Test]
        public void Get_ReturnsTheSameInstance_WhenTypeBoundWithBindSingleton()
        {
            injectionScope.BindSingleton<IA, ClassA>();
            IA a1 = injectionScope.Get<IA>();
            IA a2 = injectionScope.Get<IA>();
            Assert.IsNotNull(a1);
            Assert.IsNotNull(a2);
            Assert.AreSame(a1, a2);
        }

        [Test]
        public void Get_ReturnsTheRegisteredInstance_WhenTypeBoundWithBindToSingletonInstance()
        {
            object instance = new object();
            injectionScope.SingletonInstance(instance);
            Assert.AreSame(instance,injectionScope.Get<object>());
            Assert.AreSame(instance,injectionScope.Get<object>());
        }

        [Test]
        public void Get_ResolvesCircularDependencies()
        {
            injectionScope.BindSingleton<IB, NeedsA>();
            injectionScope.BindSingleton<IA, NeedsB>();
            IB b = injectionScope.Get<IB>();
            Assert.IsNotNull(b);
            Assert.IsNotNull(b.A);
        }

        [Test]
        public void Get_ResolvesDependencies_WhenInnerScopeTypeNeedsTypesFromOuterScope()
        {
            injectionScope.BindSingleton<IA, ClassA>();
            
            InjectionScope childScope = new InjectionScope(injectionScope); 
            childScope.BindSingleton<IB, NeedsA>();

            IB b = childScope.Get<IB>();
            Assert.IsNotNull(b);
            Assert.IsNotNull(b.A);
        }
        
        [Test, ExpectedException(typeof(ApplicationException))]
        public void Get_ThrowsException_WhenInnerScopeTypeNeedsTypesFromOuterScopeAndTheOuterScopeDoesntHaveAllItsRequiredTypes()
        {
            injectionScope.BindSingleton<IA, NeedsB>();
            
            InjectionScope childScope = new InjectionScope(injectionScope); 
            childScope.BindSingleton<IB, NeedsA>();

            childScope.Get<IB>();
        }

        [Test]
        public void Get_CallOutToParentScopeForTypesItDoesNotHave()
        {
            InjectionScope innerScope = new InjectionScope(injectionScope);
            injectionScope.Bind<IA, ClassA>();
            IA a = innerScope.Get<IA>();
            Assert.IsNotNull(a);
        }

        [Test, ExpectedException(typeof (UnknownTypeException))]
        public void Get_ThrowsException_WhenTypeIsNotKnown()
        {
            injectionScope.Get<IA>();
        }

        [Test, ExpectedException(typeof(InvalidOperationException))]
        public void Singleton_ThrowsExceptoin_WhenTypeIsAlreadyDefined()
        {
            injectionScope.Bind<IA,ClassA>();    
            injectionScope.Singleton<ClassA>();    
        }
        
        [Test]
        public void Singleton_MakesABoundTypeActLikeOne()
        {
            injectionScope.Singleton<ClassA>();
            injectionScope.Bind<IA, ClassA>();

            IA a1 = injectionScope.Get<IA>();
            IA a2 = injectionScope.Get<IA>();
            Assert.IsNotNull(a1);
            Assert.IsNotNull(a2);
            Assert.AreSame(a1, a2);
    
        }

        [Test]
        public void Singleton_MakesATypeActLikeOne()
        {
            injectionScope.Singleton<ClassA>();

            ClassA a1 = injectionScope.Get<ClassA>();
            ClassA a2 = injectionScope.Get<ClassA>();
            Assert.IsNotNull(a1);
            Assert.IsNotNull(a2);
            Assert.AreSame(a1, a2);
        }

        [Test]
        public void Has_ReturnsTrue_WhenTypeIsKnown()
        {
            injectionScope.Bind<IA, ClassA>();
            Assert.IsTrue(injectionScope.Has<IA>());
        }

        [Test]
        public void Has_ReturnsFalse_WhenTypeIsKnown()
        {
            Assert.IsFalse(injectionScope.Has<IA>());
        }

        [Test]
        public void Has_ReturnsTrue_WhenTypeIsKnownIsParentScope()
        {
            injectionScope.Bind<IA, ClassA>();

            InjectionScope innerScope = new InjectionScope(injectionScope);
            Assert.IsTrue(innerScope.Has<IA>());
        }
    }
}