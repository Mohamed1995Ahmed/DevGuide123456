using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public enum Day{
    Sunday = 0,
    Monday = 1,
    Tuesday = 2,
    Wednesday = 3,
    Thursday = 4,
    Friday = 7,
    Saturday = 6

}
namespace DevGuide.Models.Models
{
    

    public class Schedule
    {
        public int Id { get; set; }
        public Day Day { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }

        // Relationships
        public virtual User User { get; set; }
        public string User_Id { get; set; } // Foreign Key to User
    }
}
