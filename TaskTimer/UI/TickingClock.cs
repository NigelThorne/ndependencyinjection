using System;
using System.Timers;

namespace TaskTimer.UI
{
    public class TickingClock : IStartListener, IClock
    {
        private readonly ITickListener _tickListener;
        private readonly Timer _aTimer;

        public TickingClock(ITickListener tickListener)
        {
            _tickListener = tickListener;
            _aTimer = new Timer();
            _aTimer.Elapsed += OnTimedEvent;
        }

        private static int CurrentMinute()
        {
            return DateTime.Now.Minute;
        }

        private void OnTimedEvent(object sender, ElapsedEventArgs e)
        {
            _tickListener.OnTick();
        }

        public void OnStart()
        {
            _aTimer.Interval = 1000;
            _aTimer.Enabled = true;
            _aTimer.AutoReset = true;
        }

        public DateTime CurrentTime()
        {
            return DateTime.Now;
        }
    }
}