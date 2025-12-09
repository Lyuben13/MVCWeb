using System.ComponentModel.DataAnnotations;

namespace MVC.Intro.ViewModels
{
    public class ChangePasswordViewModel
    {
        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password is required")]
        [StringLength(40, MinimumLength = 8,
            ErrorMessage = "The {0} must be at {2} and at max {1} characters long.")]
        [DataType(DataType.Password)]
        [Compare("ConfirmPassword",
            ErrorMessage = "The password and confirmation password do not match.")]
        public string NewPassword { get; set; } = string.Empty;

        [Required(ErrorMessage = "Confirm New Password is required")]
        [Display(Name = "Confirm New Password")]
        public string ConfirmNewPassword { get; set; } = string.Empty;
    }
}