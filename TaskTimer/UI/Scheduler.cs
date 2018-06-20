using System;
using System.Collections.Generic;

namespace TaskTimer.UI
{
    public class Scheduler: IScheduler, ITickListener
    {

        private readonly IList<ScheduledEvent> _schedule = new List<ScheduledEvent>();

        public void ScheduleCallback(Action<DateTime> action, int frequency)
        {
            _schedule.Add(new ScheduledEvent
            {
                Callback = action,
                Frequency = frequency,
                NextEvent = DateTime.Now
            });
        }

        private class ScheduledEvent
        {
            public Action<DateTime> Callback;
            public DateTime NextEvent;
            public int Frequency;

            public void InvokeIfExpired(DateTime now)
            {
                if (NextEvent >= now) return;
                NextEvent = now.AddSeconds(Frequency);
                Callback(now);
            }
        }

        public void OnTick(DateTime time)
        {
            foreach (var scheduledEvent in _schedule)
            {
                scheduledEvent.InvokeIfExpired(time);
            }
        }
    }
}