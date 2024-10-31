
using DevGuide.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels
{
    public class SessionListViewModel
    {
        public int Id { get; set; }
        public string Topic { get; set; }
        public DateTime DateTime { get; set; }
        public BookingStatus? Status { get; set; }
        public decimal Total { get; set; }
        public string User_FirstName { get; set; }
        public string User_LastName { get; set; }
        public string User_Instructor_Id { get; set; }
    }
}
