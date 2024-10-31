using Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevGuide.Models.Models
{
    public class Skill
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }

        public virtual ICollection<User_Skills> UserSkills { get; set; }

        // Relationship with Options (each skill can be associated with multiple options)
        public virtual ICollection<Option> Options { get; set; }

        // Many-to-many relationship with Questions
        //public virtual ICollection<Question_Skill> QuestionSkills { get; set; }

        public virtual ICollection<Question> Questions { get; set; }


    }

}
