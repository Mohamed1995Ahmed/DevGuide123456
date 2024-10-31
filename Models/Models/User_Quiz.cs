using DevGuide.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevGuide.Models.Models
{
    public class User_Quiz
    {
        public int Id { get; set; }

        public string User_Id { get; set; }

        public virtual User User { get; set; }

        public int Quiz_Id { get; set; }

        public virtual Quiz Quiz { get; set; }  

        public DateTime QuizCreated { get; set; }

        public virtual ICollection<User_Answer> Answers { get; set; }


        public decimal Score { get; set; }

        public bool Result {  get; set; }

    }
}
