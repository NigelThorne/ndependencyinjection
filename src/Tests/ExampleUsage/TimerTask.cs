using System;

namespace NDependencyInjection.Tests.ExampleUsage
{
    public class TimerTask
    {
        public TimerTask(string name, DateTime startTime)
        {
            Name = name;
            StartTime = startTime;
        }

        public string Name { get; private set; }
        public DateTime StartTime { get; private set; }
    }
}