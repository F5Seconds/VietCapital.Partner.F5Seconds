using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using VietCapital.Partner.F5Seconds.Application.Interfaces;

namespace VietCapital.Partner.F5Seconds.WebMvc.Services
{
    public class AuthenticatedUserService : IAuthenticatedUserService
    {
        public AuthenticatedUserService(IHttpContextAccessor httpContextAccessor)
        {
            UserId = httpContextAccessor.HttpContext?.User?.FindFirstValue("uid");
        }

        public string UserId { get; }
    }
}
