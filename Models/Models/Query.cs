
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevGuide.Models.Models
{
    public class Query
    {
        public int Id { get; set; }
        public string? Question { get; set; }
        public string? File { get; set; }
        public DateTime DateTime { get; set; }

        // Relationships
        public virtual User User { get; set; }
        public string User_Id { get; set; } // Foreign Key to User
        public virtual User User_Instructor { get; set; }
        public string User_Instructor_Id { get; set; } // Foreign Key to Instructor
        public virtual ICollection<QueryAnswer> QueryAnswers { get; set; }
    }

}
