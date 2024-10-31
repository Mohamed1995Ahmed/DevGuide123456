using DevGuide.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels
{
    public class ExperienceViewModel
    {
        public int? Id { get; set; } = 0;
        public string FieldOfStudy { get; set; }
        public string Organization { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool? TillNow { get; set; }
        public string? User_Id { get; set; } // Foreign Key to User
    }
}
