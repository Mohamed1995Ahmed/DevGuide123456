using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevGuide.Models.Models
{
    public enum PaymentMethod
    {
        Visa=1,
        MasterCard=2,
        Paypal=3
    }
    public class Payment
    {
        public int Id { get; set; }
        public PaymentMethod PaymentType { get; set; }
        public decimal Amount { get; set; }
        public decimal Tax { get; set; }
        public decimal Total { get; set; }

        // Relationships
        public virtual Session Session { get; set; }
        public int Session_Id { get; set; } // Foreign Key to Session

 

    }

}
