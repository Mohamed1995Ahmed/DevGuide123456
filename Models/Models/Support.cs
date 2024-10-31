using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevGuide.Models.Models
{
    public class Support
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
        public string PhoneNumber { get; set; }
        public string ObjectOfComplain { get; set; }
        public string Message { get; set; }
        public DateTime Date { get; set; } = DateTime.Now;

        // Relationships
        public virtual User User { get; set; }
        public string User_Id { get; set; } // Foreign Key to User
    }

}
