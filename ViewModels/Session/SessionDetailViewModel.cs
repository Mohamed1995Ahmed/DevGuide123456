using DevGuide.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels
{
    public class SessionDetailViewModel
    {
        public int Id { get; set; }
        public string Topic { get; set; }
        public string Description { get; set; }
        public DateTime DateTime { get; set; }
        public int Duration { get; set; }
        public string? MeetingLink { get; set; }
        public string? Feedback { get; set; }
        public string? Notes { get; set; }
        public BookingStatus? Status { get; set; }

        // User information (optional)
        public string UserId { get; set; }
        public string? UserImage { get; set; }
        public string? UserTitle { get; set; }
        public string UserFirstName { get; set; }
        public string UserLastName { get; set; }


        public string? MentorImage { get; set; }
        public string? MentorTitle { get; set; }
        public string MentorFirstName { get; set; }
        public string MentorLastName { get; set; }
        public decimal Price { get; set; }





        public string UserName { get; set; } // assuming User model has a Name property
        public string UserEmail { get; set; } // assuming User model has an Email property

        // Instructor information (optional)
        public string InstructorId { get; set; }
        public string InstructorName { get; set; } // assuming Instructor User model has a Name property
        public string InstructorEmail { get; set; } // assuming Instructor User model has an Email property

        // Payment information (optional)
        public bool IsPaymentCompleted { get; set; }

        // Review information (optional)
        public string? ReviewDescription { get; set; }
        public int? ReviewRating { get; set; }
    }
}