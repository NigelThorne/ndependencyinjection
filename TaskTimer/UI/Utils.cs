namespace TaskTimer.UI
{
    public class Utils
    {
        public static string WholeNumber ( double val )
        {
            return ( (int) val ).ToString ();
        }

        public static string AllocationStopTime ( TimerViewModel model )
        {
            return
                $"{model.CurrentTaskStartedAt.AddMinutes ( model.MinutesToAllocate )}({model.MinutesToAllocate} mins)";
        }
    }
}