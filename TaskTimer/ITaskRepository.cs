using System.Collections.Generic;
using TaskTimer.Domain;

namespace TaskTimer
{
    public interface ITaskRepository
    {
        IList<TimerTask> LoadTasks();
        void SaveTasks(IList<TimerTask> list);
    }
}