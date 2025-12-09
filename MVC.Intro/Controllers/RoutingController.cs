using Microsoft.AspNetCore.Mvc;

namespace MVC.Intro.Controllers
{
    public class RoutingController : Controller
    {
        public IActionResult Default()
        {
            return View();
        }

        /// <summary>
        /// Демонстрация на Attribute Routing
        /// </summary>
        /// <returns>View с примери за attribute routing</returns>
        public IActionResult AttributeRouting()
        {
            ViewData["Title"] = "Attribute Routing";
            return View();
        }

        /// <summary>
        /// Демонстрация на Conventional Routing
        /// </summary>
        /// <returns>View с примери за conventional routing</returns>
        public IActionResult ConventionalRouting()
        {
            ViewData["Title"] = "Conventional Routing";
            return View();
        }

        /// <summary>
        /// Демонстрация на Route Constraints
        /// </summary>
        /// <param name="id">ID параметър с ограничения</param>
        /// <returns>View с пример за constraints</returns>
        [HttpGet("routing/constraints/{id:int}")]
        public IActionResult RouteConstraints(int id)
        {
            ViewData["Title"] = "Route Constraints";
            ViewData["Id"] = id;
            return View();
        }

        /// <summary>
        /// Демонстрация на Optional Parameters
        /// </summary>
        /// <param name="category">Опционален категория параметър</param>
        /// <param name="page">Опционален страница параметър</param>
        /// <returns>View с пример за optional parameters</returns>
        [HttpGet("routing/optional/{category?}/{page?}")]
        public IActionResult OptionalParameters(string category = "all", int page = 1)
        {
            ViewData["Title"] = "Optional Parameters";
            ViewData["Category"] = category;
            ViewData["Page"] = page;
            return View();
        }
    }
}