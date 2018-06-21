#region usings

using System;

#endregion

namespace TaskTimer.Domain
{
    public interface ITasksDomainController
    {
        TimerTask CurrentTask { get; }

        void UpdateCurrentTask (
            string taskName,
            string comment,
            DateTime startAt,
            DateTime endAt );

        void AddNewTask ( string taskName,
            string comment,
            DateTime startAt,
            DateTime endAt );
    }
}