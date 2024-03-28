using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace IDENTITY_V2.Models
{
    public interface IEmailSender{
        Task SendEmailAsync(string email, string subject, string message);
    }
}