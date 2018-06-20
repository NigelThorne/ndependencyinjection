using System;

namespace TaskTimer.UI
{

        public interface ITickListener
        {
            void OnTick(DateTime time);
        }
}