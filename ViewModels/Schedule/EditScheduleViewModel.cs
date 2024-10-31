using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels
{
    public class EditScheduleViewModel
    {
        //public int ScheduleId { get; set; } // Used for editing schedules
        public Day? Day { get; set; }
        public TimeSpan? StartTime { get; set; }
        public TimeSpan? EndTime { get; set; }
        public string? MentorId { get; set; } // Assuming you still pass MentorId 
        public decimal? price { get; set; }
    }
}

