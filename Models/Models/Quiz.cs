
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevGuide.Models.Models
{
    public class Quiz
    {
        public int Id { get; set; }
        public string QuizName { get; set; }
        public int  Number_Of_Questions { get; set; }
        public int Duration { get; set; }

        // Relationships
        //public virtual User User { get; set; }
        //public  string User_Id { get; set; } // Foreign Key to User
        public virtual ICollection<Question> Questions { get; set; }
        public virtual ICollection<User_Quiz> Users { get; set; }

    }

}
