using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevGuide.Models;
using DevGuide.Models.Models;


namespace ViewModels
{
    public class ScheduleViewModel
    {
        public bool? Available { get; set; } = false;

        public Day Day { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        

        public string? User_Id { get; set; }

        //public decimal? Price { get; set; }

        //public DevGuide.Models.Models.User user { get; set; }

        // Mentor (User) information


    }
}
