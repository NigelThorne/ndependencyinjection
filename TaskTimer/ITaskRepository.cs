#region usings

using System.Collections.Generic;
using TaskTimer.Domain;

#endregion

namespace TaskTimer
{
    public interface ITaskRepository
    {
        IList<TimerTask> LoadTasks ( );
        void SaveTasks ( IList<TimerTask> list );
    }
}