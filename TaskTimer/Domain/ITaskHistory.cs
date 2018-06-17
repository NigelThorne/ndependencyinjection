using System;

namespace TaskTimer.Domain
{
    public interface ITaskHistory
    {
        TimerTask CurrentTask();
        void ReplaceCurrentTask(TimerTask timerTask);
        void AddAllocationToCurrentTask(DateTime start, DateTime end, string comment);
        void RenameCurrentTask(string name);
        void AddNewTask(string taskName, DateTime start, DateTime end, string comment);
    }
}