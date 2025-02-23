using System.ComponentModel.DataAnnotations;

namespace IDENTITY_V2.ViewModels
{
    public class CreateViewModel
    {
        [Required]
        public string UserName { get; set; } = string.Empty;
       
        [Required]
        public string FullName { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;

        [Required]
        [Compare(nameof(Password),ErrorMessage ="Parolalar Aynı Değil.")]
        public string ConfirmPassword { get; set; } = string.Empty;
        public string ReCaptchaResponse { get; set; } = string.Empty;

        public IList<string>? SelectedRoles { get; set; }
    }
}