using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using VietCapital.Partner.F5Seconds.Application.DTOs.Account;

namespace VietCapital.Partner.F5Seconds.Infrastructure.Identity.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public List<RefreshToken> RefreshTokens { get; set; }
        public bool OwnsToken(string token)
        {
            return this.RefreshTokens?.Find(x => x.Token == token) != null;
        }
    }
}
