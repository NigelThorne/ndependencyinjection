#region usings

using System;

#endregion

namespace TaskTimer.UI
{
    public interface IClock
    {
        DateTime CurrentTime ( );
        void StartTicking ( );
    }

    public interface IScheduler
    {
        void ScheduleCallback ( Action<DateTime> action, int frequency );
    }
}