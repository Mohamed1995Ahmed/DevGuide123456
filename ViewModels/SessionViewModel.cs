using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels
{
    public class SessionViewModel
    {
        public string Topic { get; set; }
        public DateTime DateTime { get; set; }
        public string UserName { get; set; }
        public string InstructorName { get; set; }
    }
}
