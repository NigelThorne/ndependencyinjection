using System;
using NDependencyInjection.interfaces;
using NDependencyInjection.Tests.ExampleClasses;
using NUnit.Framework;


namespace NDependencyInjection.Tests
{
    [TestFixture]
    public class SystemDefinitionUATs
    {
        private ISystemDefinition definition;

        [SetUp]
        public void SetUp()
        {
            definition = new SystemDefinition();
        }

        [Test, ExpectedException(typeof(UnknownTypeException))]
        public void Get_ThrowsAnException_WhenTypeIsNotRegistered()
        {
            definition.Get<Object>();
        }

        [Test]
        public void Get_ReturnsAnInstanceOfType_WhenTypeIsRegistered()
        {
            definition.HasInstance(new Object()).Provides<Object>();
            Assert.IsNotNull(definition.Get<Object>());
        }

        [Test]
        public void Get_ReturnsASpecificInstance_WhenHasInstanceWasUsedToRegisterTheType()
        {
            object instance = new Object();
            definition.HasInstance(instance).Provides<Object>();
            Assert.AreSame(instance, definition.Get<Object>());
        }

        [Test]
        public void Get_ReturnsSameInstance_WhenHasInstanceProvidesMultipleTypes()
        {
            ClassA instance = new ClassA();
            definition.HasInstance(instance).Provides<Object>().Provides<ClassA>();
            Assert.AreSame(definition.Get<Object>(), definition.Get<ClassA>());            
        }

        [Test]
        public void Get_ReturnsSameInstance_WhenHasInstanceUsedToRegisterTheType()
        {
            definition.HasInstance(new Object()).Provides<Object>();
            Assert.AreSame(definition.Get<Object>(), definition.Get<Object>());            
        }

        [Test]
        public void Get_ReturnsNewInstance_WhenHasFactoryWasUsedToRegisterTheType()
        {
            definition.HasFactory<Object>().Provides<Object>();
            Assert.AreNotSame(definition.Get<Object>(), definition.Get<Object>());
        }

        [Test]
        public void Get_ReturnsNewInstance_WhenHasFactoryWasUsedToRegisterMultipleTypes()
        {
            definition.HasFactory<ClassA>().Provides<ClassA>().Provides<Object>();
            Assert.AreNotSame(definition.Get<Object>(), definition.Get<ClassA>());
        }

        [Test]
        public void Get_ReturnsSameInstance_WhenHasSingletonWasUsedToRegisterTheType()
        {
            definition.HasSingleton<ClassA>().Provides<ClassA>().Provides<Object>();
            Assert.AreSame(definition.Get<ClassA>(), definition.Get<Object>());
        }

        [Test, ExpectedException(typeof(InvalidOperationException))]
        public void Provides_ThrowsAnException_WhenTypeTypeGivenIsAlreadyKnown()
        {
            definition.HasSingleton<Object>().Provides<Object>();
            definition.HasInstance(new Object()).Provides<Object>();
        }

        [Test]
        public void Get_ReturnsInstanceFromOuterSystem_WhenCalledFromASubsystemThatDoesNotHaveAProviderForThatType()
        {
            object expected = new Object();
            object actual = null;
            definition.HasInstance(expected).Provides<Object>();
            definition.HasSubsystem(
                new DelegateExecutingBuilder(
                    delegate(ISystemDefinition scope)
                        {
                            actual = scope.Get<Object>();
                        }));
            Assert.AreSame(expected, actual);
        }
                
        [Test]
        public void Get_ReturnsInstanceFromInnerSystem_WhenCalledFromASubsystemThatDoesHaveAProviderForThatType()
        {
            object expected = new Object();
            object actual = null;

            definition.HasFactory<Object>().Provides<Object>();
            definition.HasSubsystem(
                new DelegateExecutingBuilder(
                    delegate(ISystemDefinition scope)
                        {
                            scope.HasInstance(expected).Provides<Object>();
                            actual = scope.Get<Object>();
                        }));
            Assert.AreSame(expected, actual);
        }
        
        [Test, ExpectedException(typeof(UnknownTypeException))]
        public void Get_ThrowsAnException_WhenCalledFromASubsystemThatDoesHaveAProviderForThatTypeAndItIsUnknownInTheParentScope()
        {
            definition.HasSubsystem(
                new DelegateExecutingBuilder(
                    delegate(ISystemDefinition scope)
                        {
                            scope.Get<Object>();
                        }));
        }

        [Test, ExpectedException(typeof(UnknownTypeException))]
        public void Get_CannotAccessTypesRegisteredInASubsystem()
        {
            definition.HasSubsystem(
                new DelegateExecutingBuilder(
                    delegate(ISystemDefinition scope)
                        {
                            scope.HasInstance(new Object()).Provides<Object>();
                        }));
            definition.Get<Object>();
        }

        [Test]
        public void Get_ResolvedDependencies_WhenRequestedTypeDependsOnAnotherKnownType()
        {
            definition.HasSingleton<ClassA>().Provides<IA>();
            definition.HasSingleton<ClassB>().Provides<IB>();

            Assert.IsNotNull(definition.Get<IB>());
            Assert.AreSame(definition.Get<IA>(),definition.Get<IB>().A);
        }

        [Test]
        public void Get_ResolvesCircularDependencies_WhenKnownTypesDependOnEachOther()
        {
            definition.HasSingleton<NeedsA>().Provides<IB>();
            definition.HasSingleton<NeedsB>().Provides<IA>();
            IB b = definition.Get<IB>();
            Assert.IsNotNull(b);
            Assert.IsNotNull(b.A);            
        }

        [Test]
        public void Get_ResolvesDependencies_WhenInnerScopeTypeNeedsTypesFromOuterScope()
        {
            definition.HasSingleton<ClassA>().Provides<IA>();
            IB actual = null;
            definition.HasSubsystem(
                new DelegateExecutingBuilder(
                    delegate(ISystemDefinition scope)
                        {
                            scope.HasSingleton<NeedsA>().Provides<IB>();
                            actual = scope.Get<IB>();
                        }));
            Assert.IsNotNull(actual);
            Assert.IsNotNull(actual.A);
        }

        [Test, ExpectedException(typeof(InvalidOperationException))]
        public void ListensTo_ThrowsAnException_WhenTheInterfaceIsProvidedAlready()
        {
            definition.HasFactory<Object>().Provides<Object>();
            definition.HasSingleton<Object>().ListensTo<Object>();
        }

        [Test]
        public void ListensTo_DefinesAListenerForAGivenInterface_()
        {
            TestListener realListener = new TestListener();
            definition.HasInstance(realListener).ListensTo<ITestListener>();
            ITestListener listener = definition.Get<ITestListener>();

            listener.OnEvent();

            Assert.AreEqual("Called", realListener.log);
        }

        [Test]
        public void Get_ReturnsABroadcaster_WhenMultipleListenersAreRegisteredForAType()
        {
            TestListener realListener1 = new TestListener();
            TestListener realListener2 = new TestListener();
            definition.HasInstance(realListener1).ListensTo<ITestListener>();
            definition.HasInstance(realListener2).ListensTo<ITestListener>();
            ITestListener listener = definition.Get<ITestListener>();

            listener.OnEvent();

            Assert.AreEqual("Called", realListener1.log);
            Assert.AreEqual("Called", realListener2.log);
        }
        
        [Test]
        public void ListensTo_DefinesASecondListener_WhenTheInterfaceIsListenedToAlready()
        {
            definition.HasFactory<Object>().ListensTo<Object>();
            definition.HasSingleton<Object>().ListensTo<Object>();
        }

        [Test]
        public void Get_ReturnsACollectionConstructedFromTheProvidersOfThatType_WhenThereIsMoreThanOneProvider()
        {
            ISystemDefinition book = new SystemDefinition();
            book.HasCollection(new DelegateExecutingBuilder(Page1), new DelegateExecutingBuilder(Page2)).Provides
                <Page[]>();
            Assert.AreEqual(2, book.Get<Page[]>().Length);
        }

        public class DelegateExecutingBuilder : ISubsystemBuilder
        {
            private readonly CreateSubsystem method;

            public DelegateExecutingBuilder(CreateSubsystem method)
            {
                this.method = method;
            }

            public void Build(ISystemDefinition system)
            {
                method(system);
            }
        }

        private class Page
        {
        }

        private static void Page2(ISystemDefinition scope)
        {
            scope.HasInstance(new Page()).Provides<Page>();
        }

        private static void Page1(ISystemDefinition scope)
        {
            scope.HasSingleton<Page>().Provides<Page>();
        }
    }
}