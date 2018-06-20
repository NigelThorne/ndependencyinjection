using System;

namespace TaskTimer
{
    public interface ITimerCommandsHandler
    {
        void UpdateCurrentTask(string taskName, string comment, DateTime startAt, DateTime endAt);
        void AddNewTask(string taskName, DateTime startAt, DateTime endAt, string comment);
    }
}