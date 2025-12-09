using System.ComponentModel.DataAnnotations;

namespace MVC.Intro.ViewModels
{
    public class VerifyEmailViewModel
    {
        [EmailAddress]
        [Required(ErrorMessage = "Email is required")]
        public string Email { get; set; } = string.Empty;
    }
}