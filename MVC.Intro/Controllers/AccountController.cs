using Microsoft.AspNetCore.Mvc;
using MVC.Intro.Services;
using MVC.Intro.ViewModels;

namespace MVC.Intro.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAuthService _authService;
        private readonly ILogger<AccountController> _logger;

        public AccountController(IAuthService authService, ILogger<AccountController> logger)
        {
            _authService = authService;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Login(string? returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, string? returnUrl = null)
        {
            if (ModelState.IsValid)
            {
                var user = await _authService.LoginAsync(model.Email, model.Password);

                if (user != null)
                {
                    // Създаване на session
                    HttpContext.Session.SetString("UserId", user.Id.ToString());
                    HttpContext.Session.SetString("UserName", user.Name);
                    HttpContext.Session.SetString("UserEmail", user.Email);

                    if (model.RememberMe)
                    {
                        var options = new CookieOptions
                        {
                            Expires = DateTimeOffset.UtcNow.AddDays(30),
                            HttpOnly = true,
                            Secure = true,
                            SameSite = SameSiteMode.Strict
                        };
                        Response.Cookies.Append("RememberMe", user.Id.ToString(), options);
                    }

                    _logger.LogInformation("User {Email} logged in successfully", user.Email);
                    TempData["SuccessMessage"] = $"Добре дошли, {user.Name}!";

                    return RedirectToLocal(returnUrl ?? "/Product/Index");
                }
                else
                {
                    ModelState.AddModelError("", "Имейлът или паролата са неверни.");
                    return View(model);
                }
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (await _authService.EmailExistsAsync(model.Email))
                {
                    ModelState.AddModelError("Email", "Този имейл вече е регистриран.");
                    return View(model);
                }

                var user = await _authService.RegisterAsync(model);

                if (user != null)
                {
                    _logger.LogInformation("User {Email} registered successfully", user.Email);
                    TempData["SuccessMessage"] = "Регистрацията е успешна! Моля, влезте в акаунта си.";
                    return RedirectToAction("Login", "Account");
                }
                else
                {
                    ModelState.AddModelError("", "Възникна грешка при регистрация. Моля, опитайте отново.");
                    return View(model);
                }
            }
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            HttpContext.Session.Clear();
            Response.Cookies.Delete("RememberMe");

            _logger.LogInformation("User logged out");
            TempData["SuccessMessage"] = "Излязохте успешно.";

            return RedirectToAction("Index", "Home");
        }

        private IActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }
    }
}