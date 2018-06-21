#region usings

using System;

#endregion

namespace TaskTimer.UI
{
    public interface ITickListener
    {
        void OnTick ( DateTime time );
    }
}