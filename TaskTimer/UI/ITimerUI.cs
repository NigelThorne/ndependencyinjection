using System;

namespace TaskTimer.UI
{
    public interface ITimerUI
    {
        void StartTimerUI(string currentTask, DateTime startTime, DateTime taskEndTime);
    }
}