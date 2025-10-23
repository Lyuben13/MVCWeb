using Microsoft.AspNetCore.Mvc;
using MVC.Intro.Models;
using MVC.Intro.Services;

namespace MVC.Intro.Controllers
{
    /// <summary>
    /// Controller for managing product operations
    /// </summary>
    [Route("[controller]/[action]")]
    public class ProductController : Controller
    {
        private readonly IProductService _productService;
        private readonly ILogger<ProductController> _logger;

        public ProductController(IProductService productService, ILogger<ProductController> logger)
        {
            _productService = productService ?? throw new ArgumentNullException(nameof(productService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        // GET: /Product/Index
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            try
            {
                _logger.LogInformation("Loading products index page");
                var products = await _productService.GetAllProductsAsync();
                return View(products);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while loading products index");
                return View("Error");
            }
        }

        // GET: /Product/Details/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> Details(Guid id)
        {
            try
            {
                if (id == Guid.Empty) return BadRequest();

                _logger.LogInformation("Loading product details for ID: {ProductId}", id);
                var product = await _productService.GetProductByIdAsync(id);

                if (product == null)
                {
                    _logger.LogWarning("Product with ID {ProductId} not found", id);
                    return NotFound();
                }

                return View(product);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while loading product details for ID: {ProductId}", id);
                return View("Error");
            }
        }

        // GET: /Product/Create
        [HttpGet]
        public IActionResult Create()
        {
            _logger.LogInformation("Loading create product form");
            return View();
        }

        // POST: /Product/Create  (стандартно име за asp-action="Create")
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Product product)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Product creation failed validation for product: {ProductName}", product?.Name);
                return View(product);
            }

            try
            {
                _logger.LogInformation("Creating new product: {ProductName}", product.Name);
                await _productService.AddProductAsync(product);
                TempData["SuccessMessage"] = "Продуктът е създаден успешно!";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating product: {ProductName}", product.Name);
                ModelState.AddModelError("", "Възникна грешка при създаването. Моля, опитайте отново.");
                return View(product);
            }
        }

        // POST: /Product/CreateProduct  (бекъп за съвместимост, ако View-то ти още използва asp-action="CreateProduct")
        [HttpPost("CreateProduct")]
        [ValidateAntiForgeryToken]
        public Task<IActionResult> CreateProduct(Product product) => Create(product);

        // GET: /Product/Edit/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> Edit(Guid id)
        {
            try
            {
                if (id == Guid.Empty) return BadRequest();

                _logger.LogInformation("Loading edit form for product ID: {ProductId}", id);
                var product = await _productService.GetProductByIdAsync(id);

                if (product == null)
                {
                    _logger.LogWarning("Product with ID {ProductId} not found for editing", id);
                    return NotFound();
                }

                return View(product);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while loading edit form for product ID: {ProductId}", id);
                return View("Error");
            }
        }

        // POST: /Product/Edit/{id}
        [HttpPost("{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, Product product)
        {
            if (id != product.Id) return BadRequest();

            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Product update failed validation for product: {ProductName}", product?.Name);
                return View(product);
            }

            try
            {
                _logger.LogInformation("Updating product: {ProductName}", product.Name);
                await _productService.UpdateProductAsync(product);
                TempData["SuccessMessage"] = "Продуктът е обновен успешно!";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating product: {ProductName}", product.Name);
                ModelState.AddModelError("", "Възникна грешка при обновяване. Моля, опитайте отново.");
                return View(product);
            }
        }

        // POST: /Product/Delete/{id}
        [HttpPost("{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                if (id == Guid.Empty) return BadRequest();

                _logger.LogInformation("Attempting to delete product with ID: {ProductId}", id);
                var deleted = await _productService.DeleteProductAsync(id);

                if (!deleted)
                {
                    _logger.LogWarning("Product with ID {ProductId} not found for deletion", id);
                    TempData["ErrorMessage"] = "Продуктът не е намерен.";
                }
                else
                {
                    _logger.LogInformation("Successfully deleted product with ID: {ProductId}", id);
                    TempData["SuccessMessage"] = "Продуктът е изтрит успешно.";
                }

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting product with ID: {ProductId}", id);
                TempData["ErrorMessage"] = "Възникна грешка при изтриването.";
                return RedirectToAction(nameof(Index));
            }
        }
    }
}
