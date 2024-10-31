using DevGuide.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Models
{
    public class Experience
    {
        public int Id { get; set; }
        public string FieldOfStudy { get; set; }
        public string Organization { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool? TillNow { get; set; }
        public virtual User User { get; set; }
        public string User_Id { get; set; } // Foreign Key to User
    }
}
