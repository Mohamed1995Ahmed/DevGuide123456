
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Collections.Specialized.BitVector32;
using Microsoft.AspNetCore.Identity;
using Models.Models;

namespace DevGuide.Models.Models
{
    public class User:IdentityUser
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }
        public string? About { get; set; }
        public string? Title { get; set; }

        public string? CV { get; set; }
        public string? Country { get; set; }

        public int? YearsOfExperience { get; set; }
        public string? Level { get; set; }
        public string? Image { get; set; }

        public decimal? Price { get; set; }





        // Relationships
        public virtual ICollection<Experience> Experiences { get; set; }
        public virtual ICollection<Education> Educations { get; set; }
        public virtual ICollection<User_Quiz> Quizzes { get; set; }
        public virtual ICollection<Session> Sessions { get; set; }

        public virtual ICollection<Session> InstructedSessions { get; set; }
        public virtual ICollection<Payment> Payments { get; set; }
        public virtual ICollection<Review> Reviews { get; set; }
        public virtual ICollection<Support> Supports { get; set; }
        public virtual ICollection<User_Badges> Badges { get; set; }
        public virtual ICollection<Query> Queries { get; set; }

        public virtual ICollection<Query> InstructedQueries { get; set; }
        public virtual ICollection<User_Skills> Skills { get; set; }

        public virtual ICollection<SocialAccounts>? SocialAccounts { get; set; }

        public virtual ICollection<Schedule>? Schedules { get; set; }
    }

}
