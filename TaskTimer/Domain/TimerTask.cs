using System;
using System.Collections.Generic;
using System.Linq;

namespace TaskTimer.Domain
{
    public class TimerTask
    {
        public string Name { get; set; }

        public IList<Allocation> Allocations  = new List<Allocation>();

        public DateTime StartTime
        {
            get
            {
                return Allocations.Min(a => a.StartTime); 
            }
        }

        public DateTime EndTime
        {
            get { return Allocations.Max(a => a.EndTime); }
        }

        public void AddAllocation(string comment, DateTime startTime, DateTime endTime)
        {
            Allocations.Add(new Allocation {Comment = comment, StartTime = startTime, EndTime = endTime});
        }

        public class Allocation
        {
            public DateTime StartTime { get; set; }
            public DateTime EndTime { get; set; }
            public string Comment{ get; set; }

            public int Minutes => (int) (EndTime - StartTime).TotalMinutes;
        }

        public static TimerTask CreateDefaultTask()
        {
            return new TimerTask
            {
                Allocations = new List<Allocation>(new[]
                {
                    new Allocation
                    {
                        Comment = "start",
                        StartTime = DateTime.Now,
                        EndTime = DateTime.Now
                    }
                }),
                Name = "Start tracking time."
            };
        }
    }
}