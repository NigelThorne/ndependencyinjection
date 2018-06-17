using System;
using System.Timers;

namespace NDependencyInjection.Tests.ExampleUsage.UI
{
    public class TickingClock : IStartListener, IClock
    {
        private readonly ITickListener _tickListener;
        private readonly Timer _aTimer;
        private int _lastMinute;

        public TickingClock(ITickListener tickListener)
        {
            _tickListener = tickListener;
            _aTimer = new System.Timers.Timer();
            _aTimer.Elapsed += OnTimedEvent;
            _lastMinute = CurrentMinute();
        }

        private static int CurrentMinute()
        {
            return DateTime.Now.Minute;
        }

        private void OnTimedEvent(object sender, ElapsedEventArgs e)
        {
            var currentMinute = CurrentMinute();
            if (_lastMinute == currentMinute) return;

            _lastMinute = currentMinute;
            _tickListener.OnMinuteTick();
        }

        public void OnStart()
        {
            _aTimer.Interval = 1000 * 60;
            _aTimer.Enabled = true;
            _aTimer.AutoReset = true;
        }

        public DateTime CurrentTime()
        {
            return DateTime.Now;
        }
    }
}