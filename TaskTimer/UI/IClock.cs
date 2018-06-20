using System;
using System.Threading.Tasks;

namespace TaskTimer.UI
{
    public interface IClock
    {
        DateTime CurrentTime();
        Task StartTicking();
    }

    public interface IScheduler
    {
        void ScheduleCallback(Action<DateTime> action, int frequency);
    }
}