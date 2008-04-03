using System;
using NDependencyInjection.Generics;
using NMock2;
using NMockExtensions;
using NUnit.Framework;


namespace NDependencyInjection.Tests
{
    public interface IBroadcasterTestsListener
    {
        void OnBanana(int x, int y);
        int GetSomething();
    }

    [TestFixture]
    public class TypeSafeBroadcasterTests : MockingTestFixture
    {
        private TypeSafeBroadcaster<IBroadcasterTestsListener> broadcaster;
        private IBroadcasterTestsListener listener1;
        private IBroadcasterTestsListener listener2;

        protected override void SetUp()
        {
            listener1 = NewMock<IBroadcasterTestsListener>();
            listener2 = NewMock<IBroadcasterTestsListener>();
        }

        [Test]
        public void DoesNothingWhenThereAreNoListeners()
        {
            broadcaster = new TypeSafeBroadcaster<IBroadcasterTestsListener>();
            Expect.Never.On(listener1).Method("OnBanana").With(1, 2);
            Expect.Never.On(listener2).Method("OnBanana").With(1, 2);
            broadcaster.Listener.OnBanana(1, 2);
        }

        [Test]
        public void ForwardsCallsToListeners()
        {
            broadcaster = new TypeSafeBroadcaster<IBroadcasterTestsListener>(listener1, listener2);
            Expect.Once.On(listener1).Method("OnBanana").With(1, 2);
            Expect.Once.On(listener2).Method("OnBanana").With(1, 2);
            broadcaster.Listener.OnBanana(1, 2);
        }

        [Test, ExpectedException(typeof (ArgumentException))]
        public void RethrowsExceptionWhenListenThorwsException()
        {
            broadcaster = new TypeSafeBroadcaster<IBroadcasterTestsListener>(listener1);
            Stub.On(listener1).Method("OnBanana").With(1, 2).Will(Throw.Exception(new ArgumentException()));
            broadcaster.Listener.OnBanana(1, 2);
        }

        [Test, ExpectedException(typeof (InvalidOperationException))]
        public void ThrowsExceptionWhenNonVoidMethodIsCalled()
        {
            broadcaster = new TypeSafeBroadcaster<IBroadcasterTestsListener>(listener1);
            broadcaster.Listener.GetSomething();
        }

        [Test]
        public void AddListeners_DoesNothing_IfPassedAnEmptyArray()
        {
            broadcaster = new TypeSafeBroadcaster<IBroadcasterTestsListener>();
            broadcaster.AddListeners(new IBroadcasterTestsListener[0]);
            broadcaster.Listener.OnBanana(1, 2);
        }

        [Test]
        public void AddListeners_AddsListeners_IfPassedAny()
        {
            broadcaster = new TypeSafeBroadcaster<IBroadcasterTestsListener>();
            broadcaster.AddListeners(new IBroadcasterTestsListener[]{listener1,listener2});
            Expect.Once.On(listener1).Method("OnBanana").With(1, 2);
            Expect.Once.On(listener2).Method("OnBanana").With(1, 2);
            broadcaster.Listener.OnBanana(1, 2);
        }

        [Test]
        public void RemoveListeners_DoesNothing_IfPassedAnEmptyArray()
        {
            broadcaster = new TypeSafeBroadcaster<IBroadcasterTestsListener>();
            broadcaster.RemoveListeners(new IBroadcasterTestsListener[0]);
            broadcaster.Listener.OnBanana(1, 2);
        }

        [Test]
        public void RemoveListeners_DoesNothing_IfPassedListenersNotInTheBroadcaster()
        {
            broadcaster = new TypeSafeBroadcaster<IBroadcasterTestsListener>();
            broadcaster.RemoveListeners(new IBroadcasterTestsListener[] { listener1, listener2 });
            broadcaster.Listener.OnBanana(1, 2);
        }

        [Test]
        public void RemoveListeners_RemovesListeners_IfPassedKnownListeners()
        {
            broadcaster = new TypeSafeBroadcaster<IBroadcasterTestsListener>();
            broadcaster.AddListeners(new IBroadcasterTestsListener[] { listener1, listener2 });
            broadcaster.RemoveListeners(new IBroadcasterTestsListener[] { listener1, listener2 });
            broadcaster.Listener.OnBanana(1, 2);
        }
    }
}