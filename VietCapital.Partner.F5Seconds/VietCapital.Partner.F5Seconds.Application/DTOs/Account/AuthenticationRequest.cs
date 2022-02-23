using System;
using System.Collections.Generic;
using System.Text;

namespace VietCapital.Partner.F5Seconds.Application.DTOs.Account
{
    public class AuthenticationRequest
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
