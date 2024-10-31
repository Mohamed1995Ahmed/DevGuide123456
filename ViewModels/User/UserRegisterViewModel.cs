using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels
{
    public class UserRegisterViewModel
    {
        [Required, StringLength(15, MinimumLength = 4)]
        public string FirstName { get; set; }

        [Required, StringLength(15, MinimumLength = 4)]
        public string LastName { get; set; }

        [Required, StringLength(18, MinimumLength = 4)]
        public string UserName { get; set; }
        [Required, StringLength(10, MinimumLength = 5)]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required, StringLength(15, MinimumLength = 5)]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
        [Required, EmailAddress]
        public string Email { get; set; }
        public string? Role { get; set; } = "user";


    }
}
