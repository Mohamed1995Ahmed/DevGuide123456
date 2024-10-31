using DevGuide.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevGuide.Models.Models
{
    public class User_Answer
    {
        public int Id { get; set; }

        public int User_Quiz_Id {set;get;}
        public virtual User_Quiz User_Quiz { get; set; }

        public int Question_Id { get; set; }    

        public virtual Question Question { get; set; }

        public int Option_Id { get; set; }  

        public virtual Option Option { get; set; }





    }
}
