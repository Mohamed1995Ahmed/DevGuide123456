using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevGuide.Models.Models
{
    public class User_Skills
    {
        public int Id { get; set; }
        public virtual User User { get; set; }
        public string User_Id { get; set; } // Foreign Key to User
       
        public int Skill_Id { get; set; }

        public virtual Skill Skill { get; set; }
    }

}
