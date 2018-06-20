using System;
using System.Threading.Tasks;
using System.Timers;

namespace TaskTimer.UI
{
    public class TickingClock : IClock
    {
        private readonly ITickListener _tickListener;

        public TickingClock(ITickListener tickListener)
        {
            _tickListener = tickListener;
        }

        public async Task StartTicking()
        {
            while (true)
            {
                _tickListener.OnTick(CurrentTime());
                await Task.Delay(1000);
            }
        }

        public DateTime CurrentTime()
        {
            return DateTime.Now;
        }
    }
}