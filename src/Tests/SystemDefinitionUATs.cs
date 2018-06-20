//Copyright (c) 2008 Nigel Thorne


using System;
using System.Collections.Generic;
using NDependencyInjection.DSL;
using NDependencyInjection.Exceptions;
using NDependencyInjection.interfaces;
using NDependencyInjection.Tests.TestClasses;
using NUnit.Framework;

namespace NDependencyInjection.Tests
{
    [TestFixture]
    public class SystemDefinitionUATs
    {
        [SetUp]
        public void SetUp()
        {
            _definition = new SystemDefinition();
        }

        private ISystemDefinition _definition;

        public interface IMessageReciever
        {
            void OnMessage(int m);
        }

        public interface IMessageSender
        {
            void SendMessage(int m);
        }

        public class Reciever : IMessageReciever
        {
            public readonly IList<int> recieved = new List<int>();

            public void OnMessage(int m)
            {
                recieved.Add(m);
            }
        }

        private class MessageSender : IMessageSender
        {
            private readonly IMessageReciever _messageReciever;

            public MessageSender(IMessageReciever messageReciever)
            {
                this._messageReciever = messageReciever;
            }

            public void SendMessage(int m)
            {
                _messageReciever.OnMessage(m);
            }
        }

        private class Page
        {
        }

        private class CountCalls : IDoSomething
        {
            private int count;

            #region IDoSomething Members

            public int DoSomething(int x, int y)
            {
                return count++;
            }

            #endregion
        }

        private class AddSomething : IDoSomething
        {
            #region IDoSomething Members

            public int DoSomething(int x, int y)
            {
                return x + y;
            }

            #endregion
        }

        private class CountAndAdd : IDoSomething
        {
            private readonly IDoSomething something;
            private int count;

            public CountAndAdd(IDoSomething something)
            {
                this.something = something;
            }

            #region IDoSomething Members

            public int DoSomething(int x, int y)
            {
                return something.DoSomething(x, y) + count++;
            }

            #endregion
        }

        private class DoublingDecorator : IDoSomething
        {
            private readonly IDoSomething something;

            public DoublingDecorator(IDoSomething something)
            {
                this.something = something;
            }

            #region IDoSomething Members

            public int DoSomething(int x, int y)
            {
                return something.DoSomething(x, y) * 2;
            }

            #endregion
        }

        private class IncrementDecorator : IDoSomething
        {
            private readonly IDoSomething something;

            public IncrementDecorator(IDoSomething something)
            {
                this.something = something;
            }

            #region IDoSomething Members

            public int DoSomething(int x, int y)
            {
                return something.DoSomething(x, y) + 1;
            }

            #endregion
        }

        private static void Page1(ISystemDefinition scope)
        {
            scope.HasSingleton<Page>().Provides<Page>();
        }

        private static void Page2(ISystemDefinition scope)
        {
            scope.HasInstance(new Page()).Provides<Page>();
        }

        [Test]
        public void Broadcasts_RegistersABroadcasterWithTheGivenInterface()
        {
            ISystemDefinition subsystem = new SystemDefinition();
            subsystem.RelaysCallsTo<IMessageReciever>();

            var reciever = new Reciever();
            subsystem.HasInstance(reciever).HandlesCallsTo<IMessageReciever>();
            subsystem.HasSingleton<MessageSender>().Provides<IMessageSender>();

            var sender = subsystem.Get<IMessageSender>();

            sender.SendMessage(2);
            sender.SendMessage(4);

            Assert.AreEqual(2, reciever.recieved[0]);
            Assert.AreEqual(4, reciever.recieved[1]);
        }

        [Test]
        public void Broadcasts_RegistersABroadcasterWithTheGivenInterface_AndCanBeListenedToWithinSubsystems()
        {
            ISystemDefinition subsystem = new SystemDefinition();
            subsystem.RelaysCallsTo<IMessageReciever>();

            var reciever = new Reciever();
            subsystem.HasSubsystem( scope => scope.HasInstance(reciever).HandlesCallsTo<IMessageReciever>());
            subsystem.HasSingleton<MessageSender>().Provides<IMessageSender>();

            var sender = subsystem.Get<IMessageSender>();

            sender.SendMessage(2);
            sender.SendMessage(4);

            Assert.AreEqual(2, reciever.recieved[0]);
            Assert.AreEqual(4, reciever.recieved[1]);
        }

        [Test]
        public void Decorate_SpecifiesADecoratorToUseForAType()
        {
            _definition.HasSingleton<AddSomething>().Provides<IDoSomething>();
            _definition.Decorate<IDoSomething>().With<DoublingDecorator>();
            var addThenDouble = _definition.Get<IDoSomething>();
            Assert.AreEqual(60, addThenDouble.DoSomething(10, 20));
        }

        [Test]
        public void Decoraters_ChainSoAllApply()
        {
            _definition.HasSingleton<AddSomething>().Provides<IDoSomething>();
            _definition.Decorate<IDoSomething>().With<DoublingDecorator>();
            _definition.Decorate<IDoSomething>().With<IncrementDecorator>();
            var addThenDouble = _definition.Get<IDoSomething>();
            Assert.AreEqual(61, addThenDouble.DoSomething(10, 20));
        }

        [Test]
        public void Decoraters_ChainSoAllApply_Event_same_twice()
        {
            _definition.HasSingleton<AddSomething>().Provides<IDoSomething>();
            _definition.Decorate<IDoSomething>().With<DoublingDecorator>();
            _definition.Decorate<IDoSomething>().With<DoublingDecorator>();
            var addThenDouble = _definition.Get<IDoSomething>();
            Assert.AreEqual(120, addThenDouble.DoSomething(10, 20));
        }

        [Test]
        public void Decorator_GetsInstanceOfParentForServiceItProvides()
        {
            DecoratorA instance = null;

            IA parent = new ClassA();
            _definition.HasInstance(parent).Provides<IA>();
            _definition.HasSubsystem(
                scope =>
                {
                    scope.HasSingleton<DecoratorA>().Provides<IA>().Provides<DecoratorA>();
                    instance = scope.Get<DecoratorA>();
                });

            Assert.AreEqual(parent, instance.Parent);
        }

        [Test]
        public void DecoratorsActLikeTheDecoratedClass_SingletonOrFactory()
        {
            _definition.HasSingleton<CountCalls>().Provides<IDoSomething>();
            _definition.Decorate<IDoSomething>().With<CountAndAdd>();

            Assert.AreEqual(0, _definition.Get<IDoSomething>().DoSomething(0, 0));
            // both counters increment, so they are both singletons
            Assert.AreEqual(2, _definition.Get<IDoSomething>().DoSomething(0, 0));
            // both counters increment, so they are both singletons
            Assert.AreEqual(4, _definition.Get<IDoSomething>().DoSomething(0, 0)); 
        }

        [Test]
        public void DecoratorsActLikeTheDecoratedClass_SingletonOrFactory2()
        {
            _definition.HasFactory<CountCalls>().Provides<IDoSomething>();
            _definition.Decorate<IDoSomething>().With<CountAndAdd>();

            Assert.AreEqual(0, _definition.Get<IDoSomething>().DoSomething(0, 0));
            // both counters start again so they are new instances
            Assert.AreEqual(0, _definition.Get<IDoSomething>().DoSomething(0, 0));
        }
        [Test]
        public void Get_CannotAccessTypesRegisteredInASubsystem()
        {
            _definition.HasSubsystem(scope => scope.HasInstance(new object()).Provides<object>());

            Assert.Throws<UnknownTypeException>(() => _definition.Get<object>());
        }

        [Test]
        public void Get_ResolvedDependencies_WhenRequestedTypeDependsOnAnotherKnownType()
        {
            _definition.HasSingleton<ClassA>().Provides<IA>();
            _definition.HasSingleton<ClassB>().Provides<IB>();

            Assert.IsNotNull(_definition.Get<IB>());
            Assert.AreSame(_definition.Get<IA>(), _definition.Get<IB>().A);
        }

        [Test]
        public void Get_ResolvesCircularDependencies_WhenKnownTypesDependOnEachOther()
        {
            _definition.HasSingleton<NeedsA>().Provides<IB>();
            _definition.HasSingleton<NeedsB>().Provides<IA>();
            var b = _definition.Get<IB>();
            Assert.IsNotNull(b);
            Assert.IsNotNull(b.A);
        }

        [Test]
        public void Get_ResolvesDependencies_WhenInnerScopeTypeNeedsTypesFromOuterScope()
        {
            _definition.HasSingleton<ClassA>().Provides<IA>();
            IB actual = null;
            _definition.HasSubsystem(
                scope =>
                {
                    scope.HasSingleton<NeedsA>().Provides<IB>();
                    actual = scope.Get<IB>();
                });
            Assert.IsNotNull(actual);
            Assert.IsNotNull(actual.A);
        }

        [Test]
        public void Get_ReturnsABroadcaster_WhenMultipleListenersAreRegisteredForAType()
        {
            _definition.RelaysCallsTo<ITestListener>();
            var realListener1 = new TestListener();
            var realListener2 = new TestListener();
            _definition.HasInstance(realListener1).HandlesCallsTo<ITestListener>();
            _definition.HasInstance(realListener2).HandlesCallsTo<ITestListener>();
            var listener = _definition.Get<ITestListener>();

            listener.OnEvent();

            Assert.AreEqual("Called", realListener1.log);
            Assert.AreEqual("Called", realListener2.log);
        }

        [Test]
        public void Get_ReturnsACollectionConstructedFromTheProvidersOfThatType_WhenThereIsMoreThanOneProvider()
        {
            ISystemDefinition book = new SystemDefinition();
            book.HasCollection(Page1, Page2).Provides
                <Page[]>();
            Assert.AreEqual(2, book.Get<Page[]>().Length);
        }

        [Test]
        public void Get_ReturnsAnInstanceOfType_WhenTypeIsRegistered()
        {
            _definition.HasInstance(new object()).Provides<object>();
            Assert.IsNotNull(_definition.Get<object>());
        }

        [Test]
        public void Get_ReturnsASpecificInstance_WhenHasInstanceWasUsedToRegisterTheType()
        {
            var instance = new object();
            _definition.HasInstance(instance).Provides<object>();
            Assert.AreSame(instance, _definition.Get<object>());
        }

        [Test]
        public void Get_ReturnsInstanceFromInnerSystem_WhenCalledFromASubsystemThatDoesHaveAProviderForThatType()
        {
            var expected = new object();
            object actual = null;

            _definition.HasFactory<object>().Provides<object>();
            _definition.HasSubsystem(
                scope =>
                {
                    scope.HasInstance(expected).Provides<object>();
                    actual = scope.Get<object>();
                });
            Assert.AreSame(expected, actual);
        }

        [Test]
        public void Get_ReturnsInstanceFromOuterSystem_WhenCalledFromASubsystemThatDoesNotHaveAProviderForThatType()
        {
            var expected = new object();
            object actual = null;
            _definition.HasInstance(expected).Provides<object>();
            _definition.HasSubsystem(scope => actual = scope.Get<object>());
            Assert.AreSame(expected, actual);
        }

        [Test]
        public void Get_ReturnsNewInstance_WhenHasFactoryWasUsedToRegisterMultipleTypes()
        {
            _definition.HasFactory<ClassA>().Provides<ClassA>().Provides<object>();
            Assert.AreNotSame(_definition.Get<object>(), _definition.Get<ClassA>());
        }

        [Test]
        public void Get_ReturnsNewInstance_WhenHasFactoryWasUsedToRegisterTheType()
        {
            _definition.HasFactory<object>().Provides<object>();
            Assert.AreNotSame(_definition.Get<object>(), _definition.Get<object>());
        }

        [Test]
        public void Get_ReturnsSameInstance_WhenHasInstanceProvidesMultipleTypes()
        {
            var instance = new ClassA();
            _definition.HasInstance(instance).Provides<object>().Provides<ClassA>();
            Assert.AreSame(_definition.Get<object>(), _definition.Get<ClassA>());
        }

        [Test]
        public void Get_ReturnsSameInstance_WhenHasInstanceUsedToRegisterTheType()
        {
            _definition.HasInstance(new object()).Provides<object>();
            Assert.AreSame(_definition.Get<object>(), _definition.Get<object>());
        }

        [Test]
        public void Get_ReturnsSameInstance_WhenHasSingletonWasUsedToRegisterTheType()
        {
            _definition.HasSingleton<ClassA>().Provides<ClassA>().Provides<object>();
            Assert.AreSame(_definition.Get<ClassA>(), _definition.Get<object>());
        }

        [Test]
        public void
            Get_ThrowsAnException_WhenCalledFromASubsystemThatDoesHaveAProviderForThatTypeAndItIsUnknownInTheParentScope()
        {
            Assert.Throws<UnknownTypeException>(() =>
            {
                _definition.HasSubsystem(scope => scope.Get<object>());
            });
        }

        [Test]
        public void Get_ThrowsAnException_WhenTypeIsNotRegistered()
        {
            Assert.Throws<UnknownTypeException>(() => _definition.Get<object>());
        }

        [Test]
        public void ListensTo_DefinesASecondListener_WhenTheInterfaceIsListenedToAlready()
        {
            _definition.RelaysCallsTo<object>();
            _definition.HasFactory<object>().HandlesCallsTo<object>();
            _definition.HasSingleton<object>().HandlesCallsTo<object>();
        }

        [Test]
        public void ListensTo_ThrowsAnException_WhenNoBroadcasterIsRegistered()
        {
            _definition.HasFactory<object>().Provides<object>();
            Assert.Throws<UnknownTypeException>(() =>
                _definition.HasSingleton<object>().HandlesCallsTo<object>());
        }

        [Test]
        public void Provides_ThrowsAnException_WhenTypeTypeGivenIsAlreadyKnown()
        {
            _definition.HasSingleton<object>().Provides<object>();
            Assert.Throws<InvalidOperationException>(() =>
                _definition.HasInstance(new object()).Provides<object>());
        }

        [Test]
        public void Service_Decorates_Works()
        {
            _definition.HasSingleton<AddSomething>().Provides<IDoSomething>();
            _definition.HasSingleton<DoublingDecorator>().Decorates<IDoSomething>();
            var addThenDouble = _definition.Get<IDoSomething>();
            Assert.AreEqual(60, addThenDouble.DoSomething(10, 20));
        }

        [Test]
        public void Service_Decorates_Works_EvenOnParentScopeObjects()
        {
            _definition.HasSingleton<AddSomething>().Provides<IDoSomething>();
            var subsystem = _definition.CreateSubsystem(
                    scope => scope.HasSingleton<DoublingDecorator>().Decorates<IDoSomething>());

            Assert.AreEqual(60, subsystem.Get<IDoSomething>().DoSomething(10, 20));
        }

        [Test]
        public void Service_Decorates_Works_Twice()
        {
            _definition.HasSingleton<AddSomething>().Provides<IDoSomething>();
            _definition.HasSingleton<DoublingDecorator>().Decorates<IDoSomething>();
            _definition.HasSingleton<IncrementDecorator>().Decorates<IDoSomething>();
            var addThenDouble = _definition.Get<IDoSomething>();
            Assert.AreEqual(61, addThenDouble.DoSomething(10, 20));
        }

        [Test]
        public void Service_HasComposite_ProvidesArrayOfContributors()
        {
            _definition.HasComposite<IDoSomething>()
                .Provides<IDoSomething[]>();

            _definition.HasSingleton<AddSomething>()
                .AddsToComposite<IDoSomething>();
            _definition.HasSingleton<IncrementDecorator>()
                .AddsToComposite<IDoSomething>();

            var composite = _definition.Get<IDoSomething[]>();
            Assert.AreEqual(2, composite.Length);
        }

        [Test]
        public void Service_HasComposite_ProvidesEmptyArrayIfNoContributors()
        {
            _definition.HasComposite<object>().Provides<object[]>();
            var composite = _definition.Get<object[]>();
            Assert.AreEqual(0, composite.Length);
        }

        [Test]
        public void Service_HasComposite_ProvidesProgrammaticComposite()
        {
            _definition.HasComposite<IDoSomething>().Provides<IDoSomething[]>();

            var compositeProvider = _definition.Get<IComposite<IDoSomething>>();
            compositeProvider.Add(new AddSomething());

            var composite = _definition.Get<IDoSomething[]>();
            Assert.AreEqual(1, composite.Length);
        }
    }
}