using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using VietCapital.Partner.F5Seconds.Application.DTOs.Account;
using VietCapital.Partner.F5Seconds.Application.Wrappers;

namespace VietCapital.Partner.F5Seconds.Application.Interfaces
{
    public interface IAccountService
    {
        Task<Response<AuthenticationResponse>> AuthenticateAsync(AuthenticationRequest request, string ipAddress);
        Task<Response<string>> RegisterAsync(RegisterRequest request, string origin);
        Task<Response<string>> ConfirmEmailAsync(string userId, string code);
        Task<object> InfoUser(string Auth);
        Task<object> ForgotPassword(ForgotPasswordRequest model, string origin);
        Task<Response<string>> ResetPassword(ResetPasswordRequest model);
        Task<object> CreateRole(string name);
        Task<object> GetAllRoles();
        object GetAllUser();
        Task<object> AddUserToRole(string username, string name);
        Task<object> AddUsersToRole(List<SelectNhanVien> listuser, string name);

        Task<object> RemoveUserFromRole(string username, string name);
        Task<object> DeleteRole(string roleId);
        Task<object> UpdateRole(string roleId, string roleName);
        Task<object> GetAllUsersByRole(string roleId);
        Task<object> AddClaimToRoles(string role, string claimName, string value);
        Task<object> RemoveClaimToRole(string role, string claimName, string value);
        Task<object> GetAllClaimsInRole(string roleName);
    }
}
