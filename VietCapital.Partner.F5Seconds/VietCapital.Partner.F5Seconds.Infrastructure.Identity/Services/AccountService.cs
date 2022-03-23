using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using VietCapital.Partner.F5Seconds.Application.DTOs.Account;
using VietCapital.Partner.F5Seconds.Application.DTOs.Email;
using VietCapital.Partner.F5Seconds.Application.Enums;
using VietCapital.Partner.F5Seconds.Application.Exceptions;
using VietCapital.Partner.F5Seconds.Application.Interfaces;
using VietCapital.Partner.F5Seconds.Application.Wrappers;
using VietCapital.Partner.F5Seconds.Domain.Settings;
using VietCapital.Partner.F5Seconds.Infrastructure.Identity.Helpers;
using VietCapital.Partner.F5Seconds.Infrastructure.Identity.Models;

namespace VietCapital.Partner.F5Seconds.Infrastructure.Identity.Services
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IEmailService _emailService;
        private readonly JWTSettings _jwtSettings;
        private readonly IDateTimeService _dateTimeService;
        public AccountService(UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IOptions<JWTSettings> jwtSettings,
            IDateTimeService dateTimeService,
            SignInManager<ApplicationUser> signInManager,
            IEmailService emailService)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _jwtSettings = jwtSettings.Value;
            _dateTimeService = dateTimeService;
            _signInManager = signInManager;
            this._emailService = emailService;
        }

        public async Task<Response<AuthenticationResponse>> AuthenticateAsync(AuthenticationRequest request, string ipAddress)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
            {
                throw new ApiException($"Không có tài khoản nào với mail {request.Email}.");
            }
            var result = await _signInManager.PasswordSignInAsync(user.UserName, request.Password, false, lockoutOnFailure: false);
            if (!result.Succeeded)
            {
                throw new ApiException($"Thông tin đăng nhập không hợp lệ cho '{request.Email}'.");
            }
            if (!user.EmailConfirmed)
            {
                throw new ApiException($"Tài khoản chưa được xác nhận cho '{request.Email}'.");
            }
            JwtSecurityToken jwtSecurityToken = await GenerateJWToken(user);
            AuthenticationResponse response = new AuthenticationResponse();
            response.Id = user.Id;
            response.JWToken = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
            response.Email = user.Email;
            response.UserName = user.UserName;
            var rolesList = await _userManager.GetRolesAsync(user).ConfigureAwait(false);
            response.Roles = rolesList.ToList();
            response.IsVerified = user.EmailConfirmed;
            var refreshToken = GenerateRefreshToken(ipAddress);
            response.RefreshToken = refreshToken.Token;
            return new Response<AuthenticationResponse>(response, $"Đã xác thực {user.UserName}");
        }

        public async Task<Response<string>> RegisterAsync(RegisterRequest request, string origin)
        {
            var userWithSameUserName = await _userManager.FindByNameAsync(request.UserName);
            if (userWithSameUserName != null)
            {
                throw new ApiException($"Tên tài khoản '{request.UserName}' đã tồn tại.");
            }
            var user = new ApplicationUser
            {
                Email = request.Email,
                FirstName = request.FirstName,
                LastName = request.LastName,
                UserName = request.UserName,
                EmailConfirmed = true
            };
            var userWithSameEmail = await _userManager.FindByEmailAsync(request.Email);
            if (userWithSameEmail == null)
            {
                var result = await _userManager.CreateAsync(user, request.Password);
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, Roles.Basic.ToString());
                    return new Response<string>(user.Id, message: $"Đăng ký thành công");

                    // var verificationUri = await SendVerificationEmail(user, origin);
                    //TODO: Attach Email Service here and configure it via appsettings
                    // await _emailService.SendAsync(new Application.DTOs.Email.EmailRequest() { From = "mail@codewithmukesh.com", To = user.Email, Body = $"Please confirm your account by visiting this URL {verificationUri}", Subject = "Confirm Registration" });
                    // return new Response<string>(user.Id, message: $"User Registered. Please confirm your account by visiting this URL {verificationUri}");
                }
                else
                {
                    throw new ApiException($"{result.Errors}");
                }
            }
            else
            {
                throw new ApiException($"Email {request.Email } đã tồn tại.");
            }
        }

        private async Task<JwtSecurityToken> GenerateJWToken(ApplicationUser user)
        {
            var userClaims = await _userManager.GetClaimsAsync(user);
            var roles = await _userManager.GetRolesAsync(user);

            var roleClaims = new List<Claim>();

            for (int i = 0; i < roles.Count; i++)
            {
                roleClaims.Add(new Claim("roles", roles[i]));
            }

            string ipAddress = IpHelper.GetIpAddress();

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim("uid", user.Id),
                new Claim("ip", ipAddress)
            }
            .Union(userClaims)
            .Union(roleClaims);

            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));
            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

            var jwtSecurityToken = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_jwtSettings.DurationInMinutes),
                signingCredentials: signingCredentials);
            return jwtSecurityToken;
        }

        private string RandomTokenString()
        {
            using var rngCryptoServiceProvider = new RNGCryptoServiceProvider();
            var randomBytes = new byte[40];
            rngCryptoServiceProvider.GetBytes(randomBytes);
            // convert random bytes to hex string
            return BitConverter.ToString(randomBytes).Replace("-", "");
        }

        private async Task<string> SendVerificationEmail(ApplicationUser user, string origin)
        {
            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
            var route = "api/account/confirm-email/";
            var _enpointUri = new Uri(string.Concat($"{origin}/", route));
            var verificationUri = QueryHelpers.AddQueryString(_enpointUri.ToString(), "userId", user.Id);
            verificationUri = QueryHelpers.AddQueryString(verificationUri, "code", code);
            //Email Service Call Here
            return verificationUri;
        }
        public async Task<object> InfoUser(string username)
        {
            var nhanvien = await _userManager.FindByNameAsync(username);
            var role = await _userManager.GetRolesAsync(nhanvien);
            var claim = await _userManager.GetClaimsAsync(nhanvien);
            var roles = _roleManager.Roles.ToList();
            var arr = new List<string>();
            foreach (var item in roles)
            {
                if (role.IndexOf(item.Name) != -1)
                {
                    var claim_role = await _roleManager.GetClaimsAsync(item);
                    foreach (var items in claim_role)
                    {
                        arr.Add(items.Value);
                    }
                }
            }
            if (nhanvien != null)
            {
                return new
                {
                    Id = nhanvien.Id,
                    Email = nhanvien.Email,
                    Username = nhanvien.UserName,
                    Result = true,
                    role = role,
                    roleClaim = arr
                };
            }
            else
            {
                return new
                {
                    Result = false,
                    Errors = new List<string>(){
                                                "Không hợp lệ"
                                            }
                };
            }
        }

        public async Task<Response<string>> ConfirmEmailAsync(string userId, string code)
        {
            var user = await _userManager.FindByIdAsync(userId);
            code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));
            var result = await _userManager.ConfirmEmailAsync(user, code);
            if (result.Succeeded)
            {
                return new Response<string>(user.Id, message: $"Account Confirmed for {user.Email}. You can now use the /api/Account/authenticate endpoint.");
            }
            else
            {
                throw new ApiException($"An error occured while confirming {user.Email}.");
            }
        }

        private RefreshToken GenerateRefreshToken(string ipAddress)
        {
            return new RefreshToken
            {
                Token = RandomTokenString(),
                Expires = DateTime.UtcNow.AddDays(7),
                Created = DateTime.UtcNow,
                CreatedByIp = ipAddress
            };
        }

        public async Task<object> ForgotPassword(ForgotPasswordRequest model, string origin)
        {
            var account = await _userManager.FindByEmailAsync(model.Email);

            // always return ok response to prevent email enumeration
            if (account == null)
                return null;

            var code = await _userManager.GeneratePasswordResetTokenAsync(account);
            var route = "api/account/reset-password/";
            var _enpointUri = new Uri(string.Concat($"{origin}/", route));
            var emailRequest = new EmailRequest()
            {
                Body = $"You reset token is - {code}",
                To = model.Email,
                Subject = "Reset Password",
            };
            await _emailService.SendAsync(emailRequest);
            return emailRequest;

        }

        public async Task<Response<string>> ResetPassword(ResetPasswordRequest model)
        {
            var account = await _userManager.FindByEmailAsync(model.Email);
            if (account == null) throw new ApiException($"No Accounts Registered with {model.Email}.");
            var result = await _userManager.ResetPasswordAsync(account, model.Token, model.Password);
            if (result.Succeeded)
            {
                return new Response<string>(model.Email, message: $"Password Resetted.");
            }
            else
            {
                throw new ApiException($"Error occured while reseting the password.");
            }
        }
        public async Task<Response<string>> ChangePassword(string Email, string ConfirmPassword, string newPassword)
        {
            if (ConfirmPassword.Equals(newPassword))
            {
                var account = await _userManager.FindByEmailAsync(Email);
                if (account == null) throw new ApiException($"Không có tài khoản nào với email là {Email}.");

                var token = await _userManager.GeneratePasswordResetTokenAsync(account);

                var result = await _userManager.ResetPasswordAsync(account, token, newPassword);
                // var result = await _userManager.ResetPasswordAsync(account, model.Token, model.Password);
                if (result.Succeeded)
                {
                    return new Response<string>(Email, message: $"Đổi mật khẩu thành công.");
                }
                else
                {
                    throw new ApiException($"CÓ lỗi trong quá trình đổi mật khẩu");
                }
            }
            else
            {
                    throw new ApiException($"Mật khẩu không khớp!!!");

            }
        }


        /////role

        public async Task<object> GetAllRoles()
        {
            var roles = await _roleManager.Roles.ToListAsync();
            return roles;
        }

        public async Task<object> CreateRole(string roleName)
        {
            var roleExist = await _roleManager.RoleExistsAsync(roleName);
            if (!roleExist)
            {
                //create the roles and seed them to the database: Question 1
                var roleResult = await _roleManager.CreateAsync(new IdentityRole(roleName));

                if (roleResult.Succeeded)
                {
                    return new { result = $"Quyền {roleName} thêm thành công" };
                }
                else
                {
                    return new { error = $"Sự cố khi thêm  mới quyền {roleName}" };
                }
            }

            return new { error = "Quyền đã tồn tại" };
        }

        public async Task<object> AddUserToRole(string userName, string roleName)
        {
            var user = await _userManager.FindByNameAsync(userName);
            if (user != null)
            {
                var result = await _userManager.AddToRoleAsync(user, roleName);
                var role = await _roleManager.FindByNameAsync(roleName);
                var roles = await _roleManager.GetClaimsAsync(role);
                foreach (var item in roles)
                {
                    await _userManager.AddClaimAsync(user, new Claim(item.Type, item.Value));
                }
                return new { result = $"Thêm thành công" };

            }

            return new { result = $"Nhân viên không tồn tại" };
        }

        public async Task<object> AddUsersToRole(List<SelectNhanVien> listuser, string roleName)
        {
            foreach (var item in listuser)
            {
                var user = await _userManager.FindByEmailAsync(item.Email);
                if (user != null)
                {
                    var result = await _userManager.AddToRoleAsync(user, roleName);
                    var role = await _roleManager.FindByNameAsync(roleName);
                    var roles = await _roleManager.GetClaimsAsync(role);
                    foreach (var item1 in roles)
                    {
                        await _userManager.AddClaimAsync(user, new Claim(item1.Type, item1.Value));
                    }
                }
            }
            return new { result = $"Thêm thành công" };
        }
        public async Task<object> RemoveUserFromRole(string username, string roleName)
        {
            var user = await _userManager.FindByNameAsync(username);

            if (user != null)
            {
                var result = await _userManager.RemoveFromRoleAsync(user, roleName);

                if (result.Succeeded)
                {
                    var role = await _roleManager.FindByNameAsync(roleName);
                    var roles = await _roleManager.GetClaimsAsync(role);
                    foreach (var item in roles)
                    {
                        await _userManager.RemoveClaimAsync(user, new Claim(item.Type, item.Value));
                    }
                    return new { result = $"Người dùng {user.UserName} đã được xóa khỏi quyền {roleName}" };
                }
                else
                {
                    return new { error = $"Error: Không thể xóa người dùng {user.UserName} ra khỏi quyền {roleName}" };
                }
            }

            // User doesn't exist
            return new { error = "Không thể tìm thấy người dùng" };
        }

        public async Task<object> DeleteRole(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);
            var users = await _userManager.GetUsersInRoleAsync(role.Name);
            if (users.Count == 0)
            {
                var rolename = role.Name;
                if (role != null)
                {
                    var result = await _roleManager.DeleteAsync(role);

                    if (result.Succeeded)
                    {
                        return new { result = $"Đã xóa quyền {rolename}" };
                    }
                    else
                    {
                        return new { error = $"Error: Không thể xóa quyền {rolename}" };
                    }
                }
                return new { error = "Không tìm thấy quyền" };
            }
            return new { error = "Đã có nhân viên trong quyền này" };


        }
        public async Task<object> UpdateRole(string id, string newRoleName)
        {
            var role = await _roleManager.FindByIdAsync(id);
            var users = await _userManager.GetUsersInRoleAsync(role.Name);
            var rolename = role.Name;
            if (role != null)
            {
                role.Name = newRoleName;
                var result = await _roleManager.UpdateAsync(role);

                if (result.Succeeded)
                {
                    return new { result = $"Cập nhật quyền {rolename} thành quyền {newRoleName}" };
                }
                else
                {
                    return new { error = $"Error:Không thể cập nhật quyền {rolename} thành quyền {newRoleName}" };
                }
            }
            return new { error = "Không thể tìm thấy quyền" };


        }


        public async Task<object> GetAllUsersByRole(string id)
        {
            var roles = await _roleManager.FindByIdAsync(id);
            var users = await _userManager.GetUsersInRoleAsync(roles.Name);
            List<string> danhsachnhanvien = new List<string>();
            foreach (var item in users)
            {
                danhsachnhanvien.Add(item.UserName);
            }
            return new { listUser = danhsachnhanvien };
        }
        //clam
        public async Task<object> AddClaimToUser(string userName, string claimName, string value)
        {
            var user = await _userManager.FindByNameAsync(userName);
            if (user != null)
            {
                await _userManager.AddClaimAsync(user, new Claim(claimName, value));
                return new { result = $"Thêm thành công" };
            }
            return new { result = $"Nhân viên không tồn tại" };
        }
        public async Task<object> RemoveClaimToUser(string username, string ClaimName, string value)
        {
            var user = await _userManager.FindByNameAsync(username);

            if (user != null)
            {
                try
                {
                    await _userManager.RemoveClaimAsync(user, new Claim(ClaimName, value));

                    return new { result = $"Người dùng {user.UserName} đã được xóa yêu cầu {ClaimName}" };

                }
                catch (Exception e)
                {
                    return new { error = $"Error: Không thể xóa yêu cầu {ClaimName} với thao tác {value} ra khỏi người dùng {user.UserName} " };
                }
            }

            // User doesn't exist
            return new { error = "Không thể tìm thấy người dùng" };
        }
        public async Task<object> GetAllClaimByUser(string username)
        {
            var nhanvien = await _userManager.FindByNameAsync(username);
            if (nhanvien != null)
            {
                var listClaimRole = new List<Claim>();
                var listRole = await _userManager.GetRolesAsync(nhanvien);
                foreach (var item in listRole)
                {
                    var role = await _roleManager.FindByNameAsync(item);
                    var roleClaim = await _roleManager.GetClaimsAsync(role);
                    foreach (var item1 in roleClaim)
                    {
                        listClaimRole.Add(item1);
                    }
                }
                var listClaimUser = await _userManager.GetClaimsAsync(nhanvien);
                var listClaim = new List<Claim>();
                foreach (var item in listClaimUser)
                {
                    foreach (var item1 in listClaimRole)
                    {
                        if (item.Value.Equals(item1.Value))
                        {
                            listClaim.Add(item);
                            break;
                        }
                    }
                }
                foreach (var item in listClaim)
                {
                    listClaimUser.Remove(item);
                }
                return listClaimUser;
            }
            return new { error = "Không thể tìm thấy người dùng" };
        }


        public async Task<object> AddClaimToRoles(string role, string claimName, string value)
        {
            var getrole = await _roleManager.FindByNameAsync(role);
            if (getrole != null)
            {
                var getclaim = await _roleManager.GetClaimsAsync(getrole);
                foreach (var item in getclaim)
                {
                    if (item.Value == value && item.Type == claimName)
                    {
                        return new { error = $"Error: Yêu cầu {claimName} với thao tác {value} cho quyền {getrole.Name} đã được sử dụng" };
                    }
                }
                var userClaim = new Claim(claimName, value);
                var result = await _roleManager.AddClaimAsync(getrole, userClaim);

                if (result.Succeeded)
                {
                    var roles = await _roleManager.FindByNameAsync(role);
                    var users = await _userManager.GetUsersInRoleAsync(roles.Name);
                    foreach (var item in users)
                    {
                        await _userManager.AddClaimAsync(item, new Claim(claimName, value));
                    }
                    return new { result = $"Yêu cầu {claimName} với thao tác {value} đã được thêm vào quyền {getrole.Name}" };
                }
                else
                {
                    return new { error = $"Error: Không thể thêm yêu cầu {claimName} với thao tác {value} cho quyền {getrole.Name}" };
                }
            }

            return new { error = "Không thể tìm thấy quyền" };
        }

        public async Task<object> RemoveClaimToRole(string role, string claimName, string value)
        {
            var getrole = await _roleManager.FindByNameAsync(role);
            var getclaim = await _roleManager.GetClaimsAsync(getrole);
            Claim claim = null;
            if (getclaim != null)
            {
                claim = getclaim.FirstOrDefault(c => c.Type == claimName && c.Value == value);
            }
            if (claim == null)
            {
                return new { error = "Không tìm thấy yêu cầu" };
            }
            if (getrole != null)
            {

                var result = await _roleManager.RemoveClaimAsync(getrole, claim);


                if (result.Succeeded)
                {
                    var roles = await _roleManager.FindByNameAsync(role);
                    var users = await _userManager.GetUsersInRoleAsync(roles.Name);
                    foreach (var item in users)
                    {
                        await _userManager.RemoveClaimAsync(item, new Claim(claimName, value));
                    }
                    return new { result = $"Yêu cầu {claimName} với thao tác {value} đã được xóa khỏi quyền {getrole.Name}" };
                }
                else
                {
                    return new { error = $"Error: Không thể tìm thấy yêu cầu {claimName} với thao tác {value} cho quyền {getrole.Name}" };
                }
            }

            return new { error = "Không thể tìm thấy quyền" };
        }

        public async Task<object> GetAllClaimsInRole(string rolename)
        {
            var role = await _roleManager.FindByNameAsync(rolename);
            var roles = await _roleManager.GetClaimsAsync(role);
            List<string> arr = new List<string>();

            foreach (var item in roles)
            {
                arr.Add(item.Value);
            }
            return new { clams = arr };
        }

        public object GetAllUser()
        {
            var listUser = _userManager.Users.ToList();
            var list = new List<Employee>();
            foreach (var item in listUser)
            {
                var emp = new Employee();
                emp.Id = item.Id;
                emp.Name = item.FirstName + " " + item.LastName;
                emp.Email = item.Email;
                emp.Username = item.UserName;
                list.Add(emp);
            }
            return new
            {
                listUser = list
            };
        }
        public async Task<object> GetUserById(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            return new
            {
                firstName = user.FirstName,
                lastName = user.LastName,
                email = user.Email,
                username = user.UserName
            };
        }

        public async Task<object> UpdateUser(string id, User data)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user is not null)
            {
                user.FirstName = data.FirstName;
                user.LastName = data.LastName;
                user.Email = data.Email;
                user.UserName = data.UserName;
                await _userManager.UpdateAsync(user);
                return new
                {
                    firstName = user.FirstName,
                    lastName = user.LastName,
                    email = user.Email,
                    username = user.UserName
                };
            }
            return new { error = "Không thể tìm thấy người dùng" };


        }

        public async Task<object> RemoveUser(string userName)
        {
            var user = await _userManager.FindByNameAsync(userName);

            if (user != null)
            {
                var result = await _userManager.GetRolesAsync(user);

                if (result != null)
                {
                    return new { error = $"Error: Không thể xóa người dùng {user.UserName} do đã có trong một quyền" };

                }
                else
                {
                    var userClaim = await _userManager.GetClaimsAsync(user);
                    foreach (var item in userClaim)
                    {
                        await _userManager.RemoveClaimAsync(user, new Claim(item.Type, item.Value));
                    }
                    return new { result = $"Người dùng {user.UserName} đã được xóa" };
                }
            }

            // User doesn't exist
            return new { error = "Không thể tìm thấy người dùng" };
        }
    }
}
