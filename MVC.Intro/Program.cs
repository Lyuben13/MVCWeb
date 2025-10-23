using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MVC.Intro.Data;
using MVC.Intro.Services;

namespace MVC.Intro
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            // --- ContentRoot/WebRoot ---
            var exeDir = AppContext.BaseDirectory; // ...\bin\Debug\net8.0\
            var projectDir = exeDir;
            if (exeDir.Contains($"{Path.DirectorySeparatorChar}bin{Path.DirectorySeparatorChar}", StringComparison.OrdinalIgnoreCase))
            {
                projectDir = Path.GetFullPath(Path.Combine(exeDir, "..", "..", ".."));
            }

            var builder = WebApplication.CreateBuilder(new WebApplicationOptions
            {
                Args = args,
                ContentRootPath = projectDir,
                WebRootPath = Path.Combine(projectDir, "wwwroot")
            });

            // --- Services ---
            builder.Services.AddControllersWithViews();

            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")
                                  ?? "Data Source=products.db"));

            builder.Services.AddScoped<IProductService, ProductService>();

            var app = builder.Build();

            // --- Pipeline ---
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.MapControllerRoute(
                name: "routing",
                pattern: "{controller=Routing}/{*default}",
                defaults: new { controller = "Routing", action = "Default" });

            // --- Apply migrations (or EnsureCreated) + Seed ---
            using (var scope = app.Services.CreateScope())
            {
                var ctx = scope.ServiceProvider.GetRequiredService<AppDbContext>();

                // Ако имаш миграции, прилагаме ги.
                if ((await ctx.Database.GetPendingMigrationsAsync()).Any())
                {
                    await ctx.Database.MigrateAsync();
                }
                // Ако НЯМА никакви миграции в проекта (чист dev проект) – създай схемата.
                else if (!(await ctx.Database.GetAppliedMigrationsAsync()).Any())
                {
                    await ctx.Database.EnsureCreatedAsync();
                }

                await SeedData.SeedAsync(ctx);
            }

            await app.RunAsync();
        }
    }
}
