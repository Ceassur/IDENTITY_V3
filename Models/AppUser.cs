using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace IDENTITY_V2.Models
{
    public class AppUser : IdentityUser
    {
        public string FullName { get; set; }
        public bool IsActive { get; set; }
    }
}