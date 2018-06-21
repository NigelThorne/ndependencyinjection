#region usings

using System;
using Moq;
using NDependencyInjection.Generics;
using NDependencyInjection.interfaces;
using NUnit.Framework;

#endregion

namespace NDependencyInjection.Tests
{
    public interface IBroadcasterTestsListener
    {
        void OnBanana ( int x, int y );
        int GetSomething ( );
    }

    [TestFixture]
    public class TypeSafeBroadcasterTests : MockingTestFixture
    {
        protected override void SetUp ( )
        {
            listener1 = NewMock<IBroadcasterTestsListener> ();
            listener2 = NewMock<IBroadcasterTestsListener> ();
        }

        private TypeSafeBroadcaster<IBroadcasterTestsListener> broadcaster;
        private IBroadcasterTestsListener listener1;
        private IBroadcasterTestsListener listener2;

        [Test]
        public void AddListeners_DoesNothing_IfPassedAnEmptyArray ( )
        {
            broadcaster = new TypeSafeBroadcaster<IBroadcasterTestsListener> ();
            broadcaster.AddListeners ();
            broadcaster.Listener.OnBanana ( 1, 2 );
        }

        [Test]
        public void DoesNothingWhenThereAreNoListeners ( )
        {
            broadcaster = new TypeSafeBroadcaster<IBroadcasterTestsListener> ();
            broadcaster.Listener.OnBanana ( 1, 2 );
            MockOf ( listener1 ).Verify ( l => l.OnBanana ( 1, 2 ), Times.Never );
            MockOf ( listener2 ).Verify ( l => l.OnBanana ( 1, 2 ), Times.Never );
        }

        [Test]
        public void ForwardsCallsToListeners ( )
        {
            broadcaster = new TypeSafeBroadcaster<IBroadcasterTestsListener> ();
            broadcaster.AddListeners ( new IBroadcasterTestsListener[] {listener1, listener2} );
            broadcaster.Listener.OnBanana ( 1, 2 );
            MockOf ( listener1 ).Verify ( l => l.OnBanana ( 1, 2 ), Times.Once );
            MockOf ( listener2 ).Verify ( l => l.OnBanana ( 1, 2 ), Times.Once );
        }

        [Test]
        public void RemoveListener_RemovesListener_EvenWhenHandlingTheEvent ( )
        {
            broadcaster = new TypeSafeBroadcaster<IBroadcasterTestsListener> ();
            broadcaster.AddListeners ( new ForgetMeListener ( broadcaster ) );
            broadcaster.Listener.OnBanana ( 1, 2 );
        }

        [Test]
        public void RemoveListeners_DoesNothing_IfPassedAnEmptyArray ( )
        {
            broadcaster = new TypeSafeBroadcaster<IBroadcasterTestsListener> ();
            broadcaster.RemoveListeners ();
            broadcaster.Listener.OnBanana ( 1, 2 );
        }

        [Test]
        public void RemoveListeners_DoesNothing_IfPassedListenersNotInTheBroadcaster ( )
        {
            broadcaster = new TypeSafeBroadcaster<IBroadcasterTestsListener> ();
            broadcaster.RemoveListeners ( listener1, listener2 );
            broadcaster.Listener.OnBanana ( 1, 2 );
        }

        [Test]
        public void RemoveListeners_RemovesListeners_IfPassedKnownListeners ( )
        {
            broadcaster = new TypeSafeBroadcaster<IBroadcasterTestsListener> ();
            broadcaster.AddListeners ( listener1, listener2 );
            broadcaster.RemoveListeners ( listener1, listener2 );
            broadcaster.Listener.OnBanana ( 1, 2 );
        }

        [Test]
        public void RethrowsExceptionWhenListenThorwsException ( )
        {
            broadcaster = new TypeSafeBroadcaster<IBroadcasterTestsListener> ();
            broadcaster.AddListeners ( listener1 );
            MockOf ( listener1 ).Setup ( l => l.OnBanana ( 1, 2 ) ).Throws ( new ArgumentException () );
            Assert.Throws<ArgumentException> ( ( ) => broadcaster.Listener.OnBanana ( 1, 2 ) );
        }

        [Test]
        public void ThrowsExceptionWhenNonVoidMethodIsCalled ( )
        {
            broadcaster = new TypeSafeBroadcaster<IBroadcasterTestsListener> ();
            broadcaster.AddListeners ( listener1 );
            Assert.Throws<InvalidOperationException> ( ( ) => broadcaster.Listener.GetSomething () );
        }
    }

    internal class ForgetMeListener : IBroadcasterTestsListener
    {
        private readonly IBroadcaster<IBroadcasterTestsListener> broadcaster;

        public ForgetMeListener ( IBroadcaster<IBroadcasterTestsListener> broadcaster )
        {
            this.broadcaster = broadcaster;
        }

        public void OnBanana ( int x, int y )
        {
            broadcaster.RemoveListeners ( this );
        }

        public int GetSomething ( )
        {
            return 0;
        }
    }
}