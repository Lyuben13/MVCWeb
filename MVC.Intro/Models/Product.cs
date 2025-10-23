using MVC.Intro.Attributes;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MVC.Intro.Models
{
    public class Product
    {
        [DisplayName("Идентификатор")]
        public Guid Id { get; set; }
        
        [DisplayName("Наименование")]
        [Required(ErrorMessage = "Полето е задължително")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Дължината на името трябва да е между 3 и 50 символа")]
        [RegularExpression(@"^[A-Za-z\s\-]+$", ErrorMessage = "Името може да съдържа само букви, интервали и тирета")]
        public required string Name { get; set; }
        
        [DisplayName("Цена")]
        [Required(ErrorMessage = "Задължително е продуктът да има цена")]
        [Range(24.99,9999.99, ErrorMessage = "Цената трябва да е между 24.99 и 9999.99")]
        public decimal Price { get; set; }
        
        [DisplayName("Описание")]
        [StringLength(500, ErrorMessage = "Описанието не може да бъде повече от 500 символа")]
        public string? Description { get; set; }
        
        [DisplayName("Картинка")]
        [StringLength(200, ErrorMessage = "Пътят към картинката не може да бъде повече от 200 символа")]
        public string? ImagePath { get; set; }
        
        [DisplayName("Категория")]
        [StringLength(50, ErrorMessage = "Категорията не може да бъде повече от 50 символа")]
        public string? Category { get; set; }
        
        [DisplayName("В наличност")]
        public bool InStock { get; set; } = true;
    }
}
