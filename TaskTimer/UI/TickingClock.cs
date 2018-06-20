using System;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Threading;

namespace TaskTimer.UI
{
    public class TickingClock : IClock
    {
        private readonly DispatcherTimer _dispatcherTimer;

        public TickingClock(ITickListener tickListener)
        {
            _dispatcherTimer = new DispatcherTimer {Interval = TimeSpan.FromSeconds(1)};
            _dispatcherTimer.Tick += ((sender, args) => tickListener.OnTick(CurrentTime()));
        }

        public void StartTicking()
        {
            _dispatcherTimer.Start();
        }

        public DateTime CurrentTime()
        {
            return DateTime.Now;
        }
    }
}