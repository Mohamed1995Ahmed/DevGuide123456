
using Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevGuide.Models.Models
{
    public class Question
    {
        public int Id { get; set; }

        public string Text { get; set; }

        public int Quiz_Id { get; set; } // Foreign Key to Quiz

        public virtual Quiz Quiz { get; set; }


        // Add this field if Questions are linked to Skills
        public int Skill_Id { get; set; }
        public virtual Skill Skill { get; set; }

        public virtual ICollection<Option> Options { get; set; }

        //public virtual ICollection<Question_Skill> QuestionSkills { get; set; }

        public virtual ICollection<User_Answer> User_Answers { get; set; }


    }

}
