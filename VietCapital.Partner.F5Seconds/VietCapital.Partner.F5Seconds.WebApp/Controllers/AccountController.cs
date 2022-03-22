using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using VietCapital.Partner.F5Seconds.Application.DTOs.Account;
using VietCapital.Partner.F5Seconds.Application.Interfaces;
using VietCapital.Partner.F5Seconds.Domain.Settings;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using VietCapital.Partner.F5Seconds.Infrastructure.Identity.Models;

namespace VietCapital.Partner.F5Seconds.WebApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;
        private readonly JWTSettings _jwtSettings;

        public AccountController(IAccountService accountService, IOptions<JWTSettings> jwtSettings)
        {
            _accountService = accountService;
            _jwtSettings = jwtSettings.Value;

        }
        [HttpPost("authenticate")]
        [AllowAnonymous]

        public async Task<IActionResult> AuthenticateAsync(AuthenticationRequest request)
        {
            return Ok(await _accountService.AuthenticateAsync(request, GenerateIPAddress()));
        }
        [HttpPost("register")]
        [Authorize(Policy = "userCreatePolicy")]
        public async Task<IActionResult> RegisterAsync(RegisterRequest request)
        {
            var origin = Request.Headers["origin"];
            return Ok(await _accountService.RegisterAsync(request, origin));
        }
        // [HttpGet("confirm-email")]
        // public async Task<IActionResult> ConfirmEmailAsync([FromQuery] string userId, [FromQuery] string code)
        // {
        //     var origin = Request.Headers["origin"];
        //     return Ok(await _accountService.ConfirmEmailAsync(userId, code));
        // }

        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordRequest model)
        {
            return Ok(await _accountService.ForgotPassword(model, Request.Headers["origin"]));
        }
        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword(ResetPasswordRequest model)
        {

            return Ok(await _accountService.ResetPassword(model));
        }
        private string GenerateIPAddress()
        {
            if (Request.Headers.ContainsKey("X-Forwarded-For"))
                return Request.Headers["X-Forwarded-For"];
            else
                return HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
        }
        [HttpGet("me")]
        public async Task<IActionResult> InfoUser()
        {
            string username = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return Ok(await _accountService.InfoUser(username));
        }
        [HttpGet("getAllUser")]
        [Authorize(Policy = "userSeenPolicy")]
        public IActionResult GetAllUser()
        {
            return Ok(_accountService.GetAllUser());
        }
        //role
        [HttpGet("role")]
        [Authorize(Policy = "phanQuyenSeenPolicy")]
        public async Task<IActionResult> GetAllRoles()
        {
            return Ok(await _accountService.GetAllRoles());
        }
        [HttpPost("role")]
        [Authorize(Policy = "phanQuyenCreatePolicy")]
        public async Task<IActionResult> CreateRole(string rolename)
        {
            return Ok(await _accountService.CreateRole(rolename));
        }
        [HttpPost("addUserToRole")]
        [Authorize(Policy = "phanQuyenEditPolicy")]
        public async Task<IActionResult> AddUserToRole(string UserName, string rolename)
        {
            return Ok(await _accountService.AddUserToRole(UserName, rolename));
        }
        [HttpPost("addUsersToRole")]
        [Authorize(Policy = "phanQuyenEditPolicy")]
        public async Task<IActionResult> AddUsersToRole(List<SelectNhanVien> listUser, string rolename)
        {
            return Ok(await _accountService.AddUsersToRole(listUser, rolename));
        }
        [HttpDelete("removeUserFromRole")]
        [Authorize(Policy = "phanQuyenEditPolicy")]
        public async Task<IActionResult> RemoveUserFromRole(string UserName, string rolename)
        {
            return Ok(await _accountService.RemoveUserFromRole(UserName, rolename));
        }

        [HttpDelete("removeUser")]
        [Authorize(Policy = "userDeletePolicy")]
        public async Task<IActionResult> RemoveUser(string UserName)
        {
            return Ok(await _accountService.RemoveUser(UserName));
        }
        [HttpDelete("DeleteRole")]
        [Authorize(Policy = "phanQuyenDeletePolicy")]
        public async Task<IActionResult> DeleteRole(string roleId)
        {
            return Ok(await _accountService.DeleteRole(roleId));
        }

        [HttpPut("UpdateRole")]
        [Authorize(Policy = "phanQuyenEditPolicy")]
        public async Task<IActionResult> UpdateRole(string roleId, string roleName)
        {
            return Ok(await _accountService.UpdateRole(roleId, roleName));
        }

        [HttpGet("GetAllUsersByRole")]
        [Authorize(Policy = "phanQuyenSeenPolicy")]
        public async Task<IActionResult> GetAllUsersByRole(string roleId)
        {
            return Ok(await _accountService.GetAllUsersByRole(roleId));
        }
        [HttpPost("AddClaimToRoles")]
        [Authorize(Policy = "phanQuyenEditPolicy")]
        public async Task<IActionResult> AddClaimToRoles(string roleName, string claimName, string value)
        {
            return Ok(await _accountService.AddClaimToRoles(roleName, claimName, value));
        }
        [HttpDelete("RemoveClaimToRole")]
        [Authorize(Policy = "phanQuyenEditPolicy")]
        public async Task<IActionResult> RemoveClaimToRole(string roleName, string claimName, string value)
        {
            return Ok(await _accountService.RemoveClaimToRole(roleName, claimName, value));
        }
        [HttpGet("GetAllClaimsInRole")]
        [Authorize(Policy = "phanQuyenSeenPolicy")]
        public async Task<IActionResult> GetAllClaimsInRole(string roleName)
        {
            return Ok(await _accountService.GetAllClaimsInRole(roleName));
        }
        [HttpGet("GetUserById")]
        [Authorize(Policy = "userSeenPolicy")]
        public async Task<IActionResult> GetUserById(string id)
        {
            return Ok(await _accountService.GetUserById(id));
        }
        [HttpPut("UpdateUser")]
        [Authorize(Policy = "userEditPolicy")]
        public async Task<IActionResult> UpdateUser(string id, User data)
        {
            return Ok(await _accountService.UpdateUser(id, data));
        }
        [HttpPost("AddClaimToUser")]
        [Authorize(Policy = "userEditPolicy")]
        public async Task<IActionResult> AddClaimToUser(string userName, string claimName, string value)
        {
            return Ok(await _accountService.AddClaimToUser(userName, claimName, value));
        }
        [HttpDelete("RemoveClaimToUser")]
        [Authorize(Policy = "userEditPolicy")]
        public async Task<IActionResult> RemoveClaimToUser(string userName, string claimName, string value)
        {
            return Ok(await _accountService.RemoveClaimToUser(userName, claimName, value));
        }
        [HttpGet("GetAllClaimByUser")]
        [Authorize(Policy = "userEditPolicy")]
        public async Task<IActionResult> GetAllClaimByUser(string userName)
        {
            return Ok(await _accountService.GetAllClaimByUser(userName));
        }
    }
}