using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels
{
    public class UserLoginViewModel
    {

        [Required]
        [Display(Name = "Enter UserName Or Email")]
        public string LoginMethod { get; set; }
        //[Required, StringLength(10, MinimumLength = 5)]
        //public string UserName { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public bool Rememberme { get; set; } = false;
        public string? ReturnUrl { get; set; } = "/";
        //[Required, EmailAddress]
        //public string Email { get; set; }
    }
}