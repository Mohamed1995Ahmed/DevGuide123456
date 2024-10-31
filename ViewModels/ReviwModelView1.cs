using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels
{
    public class ReviwModelView
    {
        public int Rate { get; set; }
        public DateTime ReviewDate { get; set; } = DateTime.Now;
        public string Description { get; set; }
        public int Session_Id { get; set; } // Foreign Key to Session
    }
}
