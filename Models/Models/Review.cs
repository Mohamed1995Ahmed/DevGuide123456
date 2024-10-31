using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevGuide.Models.Models
{
    public class Review
    {
        public int Id { get; set; }
        public int Rate { get; set; }
        public DateTime ReviewDate { get; set; } = DateTime.Now;
        public string Description { get; set; }

      
        public virtual Session Session { get; set; }
        public int Session_Id { get; set; } // Foreign Key to Session
        public string UserId { get; set; }
        public virtual User User { get; set; }
    }

}
