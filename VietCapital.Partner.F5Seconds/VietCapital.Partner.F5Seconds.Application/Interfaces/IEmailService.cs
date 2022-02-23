using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using VietCapital.Partner.F5Seconds.Application.DTOs.Email;

namespace VietCapital.Partner.F5Seconds.Application.Interfaces
{
    public interface IEmailService
    {
        Task SendAsync(EmailRequest request);
    }
}
