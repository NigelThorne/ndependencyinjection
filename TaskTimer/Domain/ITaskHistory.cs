namespace TaskTimer.Domain
{
    public interface ITaskHistory
    {
        TimerTask CurrentTask();
        void ReplaceCurrentTask(TimerTask timerTask);
        void RenameCurrentTask(string name);
    }
}