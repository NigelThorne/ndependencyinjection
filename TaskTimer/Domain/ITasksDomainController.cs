using System;

namespace TaskTimer.Domain
{
    public interface ITasksDomainController
    {
        TimerTask CurrentTask { get; }
        void ReplaceCurrentTask(TimerTask timerTask);
        void RenameCurrentTask(string name);

        void UpdateCurrentTask(
            string taskName,
            string comment,
            DateTime startAt,
            DateTime endAt);

        void AddNewTask(string taskName,
            DateTime startAt,
            DateTime endAt,
            string comment);
    }
}