//Copyright (c) 2008 Nigel Thorne
using System;
using System.Collections.Generic;
using NDependencyInjection.Exceptions;
using NDependencyInjection.interfaces;
using NDependencyInjection.Tests.ExampleClasses;
using NUnit.Framework;


namespace NDependencyInjection.Tests
{
    [TestFixture]
    public class SystemDefinitionUATs
    {
        [SetUp]
        public void SetUp()
        {
            definition = new SystemDefinition();
        }

        private ISystemDefinition definition;

        public interface IListener
        {
            void OnMessage(int m);
        }

        public class Reciever : IListener
        {
            public List<int> recieved = new List<int>();

            public void OnMessage(int m)
            {
                recieved.Add(m);
            }
        }

        public interface ISender
        {
            void SendMessage(int m);
        }

        public class Sender : ISender
        {
            private readonly IListener listener;

            public Sender(IListener listener)
            {
                this.listener = listener;
            }

            public void SendMessage(int m)
            {
                listener.OnMessage(m);
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

        [Test]
        public void Broadcasts_RegistersABroadcasterWithTheGivenInterface()
        {
            ISystemDefinition subsystem = new SystemDefinition();
            subsystem.Broadcasts<IListener>();

            Reciever reciever = new Reciever();
            subsystem.HasInstance(reciever).ListensTo<IListener>();
            subsystem.HasSingleton<Sender>().Provides<ISender>();

            ISender sender = subsystem.Get<ISender>();

            sender.SendMessage(2);
            sender.SendMessage(4);

            Assert.AreEqual(2, reciever.recieved[0]);
            Assert.AreEqual(4, reciever.recieved[1]);
        }

        [Test]
        public void Broadcasts_RegistersABroadcasterWithTheGivenInterface_AndCanBeListenedToWithinSubsystems()
        {
            ISystemDefinition subsystem = new SystemDefinition();
            subsystem.Broadcasts<IListener>();

            Reciever reciever = new Reciever();
            subsystem.HasSubsystem(
                new DelegateExecutingBuilder(
                    delegate(ISystemDefinition scope) { scope.HasInstance(reciever).ListensTo<IListener>(); }));
            subsystem.HasSingleton<Sender>().Provides<ISender>();

            ISender sender = subsystem.Get<ISender>();

            sender.SendMessage(2);
            sender.SendMessage(4);

            Assert.AreEqual(2, reciever.recieved[0]);
            Assert.AreEqual(4, reciever.recieved[1]);
        }

        [Test]
        public void Decorator_GetsInstanceOfParentForServiceItProvides()
        {
            DecoratorA instance = null;

            IA parent = new ClassA();
            definition.HasInstance(parent).Provides<IA>();
            definition.HasSubsystem(new DelegateExecutingBuilder(
                                        delegate(ISystemDefinition scope)
                                            {
                                                scope.HasSingleton<DecoratorA>().Provides<IA>().Provides<DecoratorA>();
                                                instance = scope.Get<DecoratorA>();
                                            }));

            Assert.AreEqual(parent,instance.Parent);
        }

        [Test, ExpectedException(typeof (UnknownTypeException))]
        public void Get_CannotAccessTypesRegisteredInASubsystem()
        {
            definition.HasSubsystem(
                new DelegateExecutingBuilder(
                    delegate(ISystemDefinition scope) { scope.HasInstance(new Object()).Provides<Object>(); }));
            definition.Get<Object>();
        }

        [Test]
        public void Get_ResolvedDependencies_WhenRequestedTypeDependsOnAnotherKnownType()
        {
            definition.HasSingleton<ClassA>().Provides<IA>();
            definition.HasSingleton<ClassB>().Provides<IB>();

            Assert.IsNotNull(definition.Get<IB>());
            Assert.AreSame(definition.Get<IA>(), definition.Get<IB>().A);
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

        [Test]
        public void Get_ReturnsABroadcaster_WhenMultipleListenersAreRegisteredForAType()
        {
            definition.Broadcasts<ITestListener>();
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
        public void Get_ReturnsACollectionConstructedFromTheProvidersOfThatType_WhenThereIsMoreThanOneProvider()
        {
            ISystemDefinition book = new SystemDefinition();
            book.HasCollection(new DelegateExecutingBuilder(Page1), new DelegateExecutingBuilder(Page2)).Provides
                <Page[]>();
            Assert.AreEqual(2, book.Get<Page[]>().Length);
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

        [Test]
        public void Get_ReturnsInstanceFromOuterSystem_WhenCalledFromASubsystemThatDoesNotHaveAProviderForThatType()
        {
            object expected = new Object();
            object actual = null;
            definition.HasInstance(expected).Provides<Object>();
            definition.HasSubsystem(
                new DelegateExecutingBuilder(
                    delegate(ISystemDefinition scope) { actual = scope.Get<Object>(); }));
            Assert.AreSame(expected, actual);
        }

        [Test]
        public void Get_ReturnsNewInstance_WhenHasFactoryWasUsedToRegisterMultipleTypes()
        {
            definition.HasFactory<ClassA>().Provides<ClassA>().Provides<Object>();
            Assert.AreNotSame(definition.Get<Object>(), definition.Get<ClassA>());
        }

        [Test]
        public void Get_ReturnsNewInstance_WhenHasFactoryWasUsedToRegisterTheType()
        {
            definition.HasFactory<Object>().Provides<Object>();
            Assert.AreNotSame(definition.Get<Object>(), definition.Get<Object>());
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
        public void Get_ReturnsSameInstance_WhenHasSingletonWasUsedToRegisterTheType()
        {
            definition.HasSingleton<ClassA>().Provides<ClassA>().Provides<Object>();
            Assert.AreSame(definition.Get<ClassA>(), definition.Get<Object>());
        }

        [Test, ExpectedException(typeof (UnknownTypeException))]
        public void
            Get_ThrowsAnException_WhenCalledFromASubsystemThatDoesHaveAProviderForThatTypeAndItIsUnknownInTheParentScope
            ()
        {
            definition.HasSubsystem(
                new DelegateExecutingBuilder(
                    delegate(ISystemDefinition scope) { scope.Get<Object>(); }));
        }

        [Test, ExpectedException(typeof (UnknownTypeException))]
        public void Get_ThrowsAnException_WhenTypeIsNotRegistered()
        {
            definition.Get<Object>();
        }

        [Test]
        public void ListensTo_DefinesASecondListener_WhenTheInterfaceIsListenedToAlready()
        {
            definition.Broadcasts<Object>();
            definition.HasFactory<Object>().ListensTo<Object>();
            definition.HasSingleton<Object>().ListensTo<Object>();
        }

        [Test, ExpectedException(typeof (UnknownTypeException))]
        public void ListensTo_ThrowsAnException_WhenNoBroadcasterIsRegistered()
        {
            definition.HasFactory<Object>().Provides<Object>();
            definition.HasSingleton<Object>().ListensTo<Object>();
        }

        [Test, ExpectedException(typeof (InvalidOperationException))]
        public void Provides_ThrowsAnException_WhenTypeTypeGivenIsAlreadyKnown()
        {
            definition.HasSingleton<Object>().Provides<Object>();
            definition.HasInstance(new Object()).Provides<Object>();
        }

        [Test]
        public void Decorate_SpecifiesADecoratorToUseForAType()
        {
            definition.HasSingleton<Add>().Provides<IDoSomething>();
            definition.Decorate<IDoSomething>().With<DoublingDecorator>();
            IDoSomething addThenDouble = definition.Get<IDoSomething>();
            Assert.AreEqual(60, addThenDouble.DoSomething(10, 20));
        }

        [Test]
        public void DecoratorsActLikeTheDecoratedClass_SingletonOrFactory()
        {
            definition.HasSingleton<CountCalls>().Provides<IDoSomething>();
            definition.Decorate<IDoSomething>().With<CountAndAdd>();

            Assert.AreEqual(0, definition.Get<IDoSomething>().DoSomething(0, 0));
            Assert.AreEqual(2, definition.Get<IDoSomething>().DoSomething(0, 0));
            Assert.AreEqual(4, definition.Get<IDoSomething>().DoSomething(0, 0));
        }

        [Test]
        public void Decoraters_ChainSoAllApply()
        {
            definition.HasSingleton<Add>().Provides<IDoSomething>();
            definition.Decorate<IDoSomething>().With<DoublingDecorator>();
            definition.Decorate<IDoSomething>().With<Increment>();
            IDoSomething addThenDouble = definition.Get<IDoSomething>();
            Assert.AreEqual(61, addThenDouble.DoSomething(10,20));
        }

        [Test]
        public void Decoraters_ChainSoAllApply_Event_same_twice()
        {
            definition.HasSingleton<Add>().Provides<IDoSomething>();
            definition.Decorate<IDoSomething>().With<DoublingDecorator>();
            definition.Decorate<IDoSomething>().With<DoublingDecorator>();
            IDoSomething addThenDouble = definition.Get<IDoSomething>();
            Assert.AreEqual(120, addThenDouble.DoSomething(10,20));
        }

        [Test]
        public void Service_Decorates_Works()
        {
            definition.HasSingleton<Add>().Provides<IDoSomething>();
            definition.HasSingleton<DoublingDecorator>().Decorates<IDoSomething>();
            IDoSomething addThenDouble = definition.Get<IDoSomething>();
            Assert.AreEqual(60, addThenDouble.DoSomething(10, 20));
        }

        [Test]
        public void Service_()
        {
            definition.HasSingleton<Add>().Provides<IDoSomething>();
            definition.HasSingleton<DoublingDecorator>().Decorates<IDoSomething>();
            definition.HasSingleton<Increment>().Decorates<IDoSomething>();
            IDoSomething addThenDouble = definition.Get<IDoSomething>();
            Assert.AreEqual(61, addThenDouble.DoSomething(10, 20));
        }

        class CountCalls : IDoSomething
        {
            private int count = 0;
            public int DoSomething(int x, int y)
            {
                return count++;
            }
        }

        class Add : IDoSomething
        {
            public int DoSomething(int x, int y)
            {
                return x + y;
            }
        }

        class CountAndAdd : IDoSomething
        {
            private readonly IDoSomething something;
            private int count = 0;
            public CountAndAdd(IDoSomething something)
            {
                this.something = something;
            }

            public int DoSomething(int x, int y)
            {
                return something.DoSomething(x, y) + count++;
            }
        }

        class DoublingDecorator : IDoSomething
        {
            private readonly IDoSomething something;

            public DoublingDecorator(IDoSomething something)
            {
                this.something = something;
            }

            public int DoSomething(int x, int y)
            {
                return something.DoSomething(x, y) * 2;
            }
        }

        class Increment : IDoSomething
        {
            private readonly IDoSomething something;

            public Increment(IDoSomething something)
            {
                this.something = something;
            }

            public int DoSomething(int x, int y)
            {
                return something.DoSomething(x, y) +1;
            }
        }
    }
}