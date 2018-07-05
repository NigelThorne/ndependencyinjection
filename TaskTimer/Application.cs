#region usings

using System;
using AmmySidekick;
using TaskTimer.UI;

#endregion

namespace TaskTimer
{
    public class Application : IRunnable, IViewClosedHandler, ITickListener
    {
        private readonly IClock _clock;
        private readonly IUIFactory _factory;
        private readonly IScheduler _scheduler;
        private ITimerUI _currentUI;
        private readonly object _UILock = new object ();
        private DateTime _nextShow;
        private static readonly TimeSpan ReShowInterval = TimeSpan.FromMinutes(10);

        public Application ( IUIFactory factory, IClock clock, IScheduler scheduler )
        {
            _factory = factory;
            _clock = clock;
            _scheduler = scheduler;
        }

        public void Run ( )
        {
            var app = new App ();
            RuntimeUpdateHandler.Register ( app, "/" + Ammy.GetAssemblyName ( app ) + ";component/App.g.xaml" );

            _scheduler.ScheduleCallback ( (t) => ShowUI(t), 30 );
            _clock.StartTicking ();

            app.Run ();
        }

        public void ShowRunningUI ( )
        {
            ShowUI (_clock.CurrentTime (), true);
        }

        public void OnTick ( DateTime time )
        {
            ITimerUI currentUI;
            lock ( _UILock )
            {
                currentUI = _currentUI;
            }

            if ( currentUI != null ) currentUI.OnTick ( time );
        }

        public void OnViewClosed ( )
        {
            lock ( _UILock )
            {
                _currentUI = null;
            }
            _nextShow = _clock.CurrentTime() + ReShowInterval;
        }

        private void ShowUI ( DateTime time, bool force = false )
        {
            if (!force && time < _nextShow) return;
            _nextShow = time + ReShowInterval;

            ITimerUI currentUI;
            lock ( _UILock )
            {
                if ( _currentUI == null ) _currentUI = _factory.CreateUI ();
                currentUI = _currentUI;
            }

            currentUI.ShowUI ();
        }
    }
}