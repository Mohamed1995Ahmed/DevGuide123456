using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels
{
    public class CreatePaymentViewModel
    {
        public decimal Amount { get; set; }
        public string SessionID { get; set; } // Adjust type as necessary
    }
}
