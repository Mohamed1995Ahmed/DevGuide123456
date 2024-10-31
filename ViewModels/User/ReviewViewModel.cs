using DevGuide.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels
{
    public class ReviewViewModel
    {
        public int? Id { get; set; }
        public int Rate { get; set; }
        public DateTime ReviewDate { get; set; } = DateTime.Now;
        public string Description { get; set; }
        //public virtual Session Session { get; set; }
        //public int Session_Id { get; set; }
        public ReviewerViewModel? Reviewer {  get; set; }

    }
}
