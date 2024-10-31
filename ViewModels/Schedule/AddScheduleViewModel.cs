using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels
{
    public class AddScheduleViewModel
    {
        public decimal? Price { get; set; }
        public List<ScheduleViewModel>? Schedules { get; set; }
       
    }
}
