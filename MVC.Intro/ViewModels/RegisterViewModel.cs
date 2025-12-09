using System.ComponentModel.DataAnnotations;

namespace MVC.Intro.ViewModels
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Името е задължително")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Името трябва да е между 2 и 100 символа")]
        [Display(Name = "Име")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Имейлът е задължителен")]
        [EmailAddress(ErrorMessage = "Невалиден имейл адрес")]
        [Display(Name = "Имейл")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Паролата е задължителна")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Паролата трябва да е поне 6 символа")]
        [DataType(DataType.Password)]
        [Display(Name = "Парола")]
        public string Password { get; set; } = string.Empty;

        [Required(ErrorMessage = "Потвърждението на паролата е задължително")]
        [DataType(DataType.Password)]
        [Display(Name = "Потвърди парола")]
        [Compare("Password", ErrorMessage = "Паролите не съвпадат")]
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}