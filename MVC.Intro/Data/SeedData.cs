using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MVC.Intro.Models;

namespace MVC.Intro.Data
{
    public static class SeedData
    {
        public static async Task SeedAsync(AppDbContext context)
        {
            // Създава базата, ако още не е създадена
            await context.Database.EnsureCreatedAsync();

            // Проверява дали вече има продукти
            if (context.Products.Any())
                return;

            // Примерни начални продукти
            var products = new List<Product>
            {
                new Product
                {
                    Id = Guid.NewGuid(),
                    Name = "Gaming Laptop",
                    Price = 2500.00m,
                    Description = "Мощен лаптоп за гейминг и работа",
                    ImagePath = "images/nitro.jpg.webp",
                    Category = "Електроника",
                    InStock = true
                },
                new Product
                {
                    Id = Guid.NewGuid(),
                    Name = "Smartphone",
                    Price = 800.00m,
                    Description = "Модерен смартфон с най-новите функции",
                    ImagePath = "images/phone.svg",
                    Category = "Електроника",
                    InStock = true
                },
                new Product
                {
                    Id = Guid.NewGuid(),
                    Name = "Programming Book",
                    Price = 45.00m,
                    Description = "Книга за програмиране на C# и ASP.NET",
                    ImagePath = "images/book.svg",
                    Category = "Книги",
                    InStock = true
                },
                new Product
                {
                    Id = Guid.NewGuid(),
                    Name = "Coffee Mug",
                    Price = 15.00m,
                    Description = "Керамична чаша за кафе с лого",
                    ImagePath = "images/product-placeholder.svg",
                    Category = "Други",
                    InStock = true
                }
            };

            await context.Products.AddRangeAsync(products);
            await context.SaveChangesAsync();
        }
    }
}
