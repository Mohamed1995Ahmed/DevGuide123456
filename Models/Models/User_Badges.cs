using DevGuide.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevGuide.Models.Models
{
    public class User_Badges
    {
        public int Id { get; set; }  // Primary key of User_Badges
        // Relationships
        public virtual User User { get; set; }
        public string User_Id { get; set; } // Foreign Key to User
        public virtual Badge Badge { get; set; }
        public int Badge_Id { get; set; } // Foreign Key to Badge

    }
}
