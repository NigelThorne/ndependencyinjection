using System;

namespace TaskTimer.UI
{
    public interface ITimerUpdateHandler
    {
        void UpdateCurrentTask(string taskName, string comment, DateTime startAt, DateTime endAt);
        void AddNewTask(string taskName, string comment, DateTime startAt, DateTime endAt);
    }
}