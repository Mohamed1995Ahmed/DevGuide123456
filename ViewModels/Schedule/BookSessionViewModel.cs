using DevGuide.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels
{
    public class BookSessionViewModel
    {

       // public int ScheduleId { get; set; }  // ID of the schedule being booked
        public string MentorId { get; set; }  // ID of the mentor
        public string Topic { get; set; }
        public string Description { get; set; }
        public int Duration { get; set; } = 1; // Duration of the session in minutes
        //public string MeetingLink { get; set; }  // Link for the session
       // public decimal Amount { get; set; }  // Amount for the session
        //public decimal Tax { get; set; }  // Tax for the session
       // public decimal Total { get; set; }  // Total cost of the session (Amount + Tax)
        public DateTime DateTime { get; set; } = DateTime.Now;
       // public PaymentMethod PaymentType { get; set; }  // Payment type (Credit Card, PayPal, etc.)
        //public BookingStatus Status { get; set; }    
    }

}
