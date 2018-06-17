using System;
using NDependencyInjection.Tests.ExampleUsage.UI;

namespace NDependencyInjection.Tests.ExampleUsage
{
    public class TimerViewModel
    {
        public TimeSpan UnAllocatedTime { get; set; }
        public DateTime TimeAllocatedUpTo { get; set; }
        public string TaskName { get; set; }
        public DateTime CurrentTaskStartedAt { get; set; }
    }
}