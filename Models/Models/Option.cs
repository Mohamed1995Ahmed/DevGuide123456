using DevGuide.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevGuide.Models.Models
{
    public class Option
    {
        public int Id { get; set; }

        public string Text { get; set; }

        public int Question_Id { get; set; }

        public virtual Question Question { get; set; }
        public int User_Answer_Id { get; set; } 
        public virtual ICollection<User_Answer> UserAnswer { get; set; }

        public int Skill_Id { get; set; }
        public virtual Skill Skill { get; set; }

        public bool IsCorrect { get; set; }


    }
}
