#region usings

using System;
using System.Collections.Generic;
using System.Linq;

#endregion

namespace TaskTimer.Domain
{
    public class TimerTask
    {
        private readonly IList<Allocation> _allocations;

        public TimerTask ( string name, IEnumerable<Allocation> allocations )
        {
            Name = name;
            _allocations = allocations.ToList ();
        }

        public string Name { get; }

        public DateTime StartTime => Allocations.Min ( a => a.StartTime );

        public DateTime EndTime => Allocations.Max ( a => a.EndTime );

        public IEnumerable<Allocation> Allocations => _allocations;

        public TimerTask AddAllocation ( string comment, DateTime startTime, DateTime endTime )
        {
            return new TimerTask (
                Name,
                Allocations.Concat ( new[]
                    {new Allocation {Comment = comment, StartTime = startTime, EndTime = endTime}} ).ToList () );
        }

        public static TimerTask CreateDefaultTask ( )
        {
            return new TimerTask ( "StartTicking tracking time.", new List<Allocation> ( new[]
            {
                new Allocation
                {
                    Comment = "start",
                    StartTime = DateTime.Now,
                    EndTime = DateTime.Now
                }
            } ) );
        }

        public class Allocation
        {
            public DateTime StartTime { get; set; }
            public DateTime EndTime { get; set; }
            public string Comment { get; set; }

            public int Minutes => (int) ( EndTime - StartTime ).TotalMinutes;
        }
    }
}