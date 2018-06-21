#region usings

using System;
using System.Collections.Generic;

#endregion

namespace TaskTimer.UI
{
    public class Scheduler : IScheduler, ITickListener
    {
        private readonly IList<ScheduledEvent> _schedule = new List<ScheduledEvent> ();

        public void ScheduleCallback ( Action<DateTime> action, int frequency )
        {
            _schedule.Add ( new ScheduledEvent
            {
                Callback = action,
                Frequency = frequency,
                NextEvent = DateTime.Now
            } );
        }

        public void OnTick ( DateTime time )
        {
            foreach ( var scheduledEvent in _schedule ) scheduledEvent.InvokeIfExpired ( time );
        }

        private class ScheduledEvent
        {
            public Action<DateTime> Callback;
            public int Frequency;
            public DateTime NextEvent;

            public void InvokeIfExpired ( DateTime now )
            {
                if ( NextEvent >= now ) return;
                NextEvent = now.AddSeconds ( Frequency );
                Callback ( now );
            }
        }
    }
}