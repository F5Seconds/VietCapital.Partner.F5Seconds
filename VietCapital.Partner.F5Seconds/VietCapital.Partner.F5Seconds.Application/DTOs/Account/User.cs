namespace VietCapital.Partner.F5Seconds.Application.DTOs.Account
{
    public class User
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }

    }

    public class ChangePassword
    {
        public string Email { get; set; }
        public string OldPassword { get; set; }
        public string ConfirmPassword { get; set; }
        public string NewPassword { get; set; }

    }
}
