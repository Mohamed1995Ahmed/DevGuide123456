﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels
{
    public class LoginResponse
    {
        public bool Success { get; set; }
        public string Token { get; set; }
        public List<string> Roles { get; set; }
    }
}
