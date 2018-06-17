using System;

namespace NDependencyInjection.Tests.ExampleUsage
{
    public class TaskRepository : ITaskHistory
    {
        public TimerTask LastAllocatedTask()
        {
            // TEMP
            return new TimerTask("Example Task", DateTime.Now.AddMinutes(-10));
        }
    }
}