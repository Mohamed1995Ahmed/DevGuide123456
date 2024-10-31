using DevGuide.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels
{
    public static class ExtnsionSchedule
    {
        public static Schedule tomoodeSchedule(this ScheduleViewModel schedule)
        {
            return new Schedule
            {
                Day = schedule.Day,
                StartTime = schedule.StartTime,
                EndTime = schedule.EndTime,
            };
        }

    }
}
