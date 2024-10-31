using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevGuide.Models.Models
{
    public enum SocialAccountsTypes
    {
        Facebook=1,
        Linkedin=2,
        GitHub=3,
    }
    public class SocialAccounts
    {

        public int Id { get; set; }

        public SocialAccountsTypes SocialName { get; set; }

        public string? SocialLink { get; set; }
        public virtual User User { get; set; }
        public string User_Id { get; set; } // Foreign Key to User
        
    }

}
