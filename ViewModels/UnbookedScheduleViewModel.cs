using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels
{
    public class UnbookedScheduleViewModel
    {
        public int Id { get; set; }
        public string Day { get; set; }
        public string Month { get; set; }
        public string Year { get; set; }  // Assuming Day is represented as a string
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
    }
}
