using System;

namespace NDependencyInjection.Tests.ExampleUsage
{
    public interface ITimerUI: IDisposable
    {
        void StartTimerUI(string currentTask, DateTime startTime);
    }
}