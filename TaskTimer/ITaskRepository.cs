using System.Collections.Generic;
using TaskTimer.Domain;

namespace TaskTimer
{
    public interface ITaskRepository
    {
        List<TimerTask> LoadList();
        void SaveHistory(List<TimerTask> list);
    }
}