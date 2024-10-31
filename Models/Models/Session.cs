using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevGuide.Models.Models
{

    public enum BookingStatus
    {
        Completed = 1,
        Pending,
        Canceled,
    }
    public class Session
    {
        public int Id { get; set; }
        public string Topic { get; set; }
        public string Description { get; set; }
        public DateTime DateTime { get; set; }
        public int Duration { get; set; } = 1;
        public string? MeetingLink { get; set; }

        public string? Feedback { get; set; }

        public string? Notes { get; set; }

        public BookingStatus? Status { get; set; }



        // Relationships

        public virtual User User { get; set; }
        public string User_Id { get; set; } // Foreign Key to User
        public virtual User User_Instructor { get; set; }
        public string User_Instructor_Id { get; set; } // Foreign Key to Instructor

        public virtual Payment Payment { get; set; }

        //public int Payment_Id { get; set;}

       public virtual Review? Review { get; set; }

    }
    
}
