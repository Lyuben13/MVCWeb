using Microsoft.EntityFrameworkCore;
using MVC.Intro.Data;
using MVC.Intro.Models;

namespace MVC.Intro.Services
{
    /// <summary>
    /// Service for managing product operations
    /// </summary>
    public class ProductService : IProductService
    {
        private const string ProductPrefix = "PRD_";
        private readonly ILogger<ProductService> _logger;
        private readonly AppDbContext _context;

        public ProductService(ILogger<ProductService> logger, AppDbContext context)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        /// <summary>
        /// Retrieves all products asynchronously
        /// </summary>
        /// <returns>List of all products</returns>
        public async Task<List<Product>> GetAllProductsAsync()
        {
            try
            {
                _logger.LogInformation("Retrieving all products");
                return await _context.Products.ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving all products");
                throw;
            }
        }

        /// <summary>
        /// Retrieves a product by its ID asynchronously
        /// </summary>
        /// <param name="id">Product ID</param>
        /// <returns>Product if found, null otherwise</returns>
        public async Task<Product?> GetProductByIdAsync(Guid id)
        {
            try
            {
                _logger.LogInformation("Retrieving product with ID: {ProductId}", id);
                return await _context.Products.FirstOrDefaultAsync(p => p.Id == id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving product with ID: {ProductId}", id);
                throw;
            }
        }

        /// <summary>
        /// Adds a new product asynchronously
        /// </summary>
        /// <param name="product">Product to add</param>
        /// <returns>Added product with generated ID</returns>
        /// <exception cref="ArgumentNullException">Thrown when product is null</exception>
        public async Task<Product> AddProductAsync(Product product)
        {
            if (product == null)
            {
                throw new ArgumentNullException(nameof(product), "Product cannot be null");
            }

            try
            {
                var toAdd = new Product
                {
                    Id = Guid.NewGuid(),
                    Name = product.Name,
                    Price = product.Price
                };

                _logger.LogInformation("Adding product: {ProductName} with price {ProductPrice}", toAdd.Name, toAdd.Price);

                if (!toAdd.Name.StartsWith(ProductPrefix))
                {
                    DecorateProductName(toAdd);
                }

                _context.Products.Add(toAdd);
                await _context.SaveChangesAsync();

                _logger.LogInformation("Successfully added product with ID: {ProductId}", toAdd.Id);
                return toAdd;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while adding product: {ProductName}", product.Name);
                throw;
            }
        }

        /// <summary>
        /// Updates an existing product asynchronously
        /// </summary>
        /// <param name="product">Product to update</param>
        /// <returns>Updated product</returns>
        /// <exception cref="ArgumentNullException">Thrown when product is null</exception>
        public async Task<Product> UpdateProductAsync(Product product)
        {
            if (product == null)
            {
                throw new ArgumentNullException(nameof(product), "Product cannot be null");
            }

            try
            {
                _logger.LogInformation("Updating product: {ProductName} with ID: {ProductId}", product.Name, product.Id);
                
                var existingProduct = await _context.Products.FirstOrDefaultAsync(p => p.Id == product.Id);
                if (existingProduct == null)
                {
                    throw new ArgumentException($"Product with ID {product.Id} not found", nameof(product));
                }

                // Update properties
                existingProduct.Name = product.Name;
                existingProduct.Price = product.Price;
                existingProduct.Description = product.Description;
                existingProduct.ImagePath = product.ImagePath;
                existingProduct.Category = product.Category;
                existingProduct.InStock = product.InStock;

                // Apply name decoration if needed
                if (!existingProduct.Name.StartsWith(ProductPrefix))
                {
                    DecorateProductName(existingProduct);
                }

                await _context.SaveChangesAsync();

                _logger.LogInformation("Successfully updated product with ID: {ProductId}", product.Id);
                return existingProduct;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating product: {ProductName}", product.Name);
                throw;
            }
        }

        /// <summary>
        /// Deletes a product by ID asynchronously
        /// </summary>
        /// <param name="id">Product ID to delete</param>
        /// <returns>True if product was deleted, false if not found</returns>
        public async Task<bool> DeleteProductAsync(Guid id)
        {
            try
            {
                _logger.LogInformation("Attempting to delete product with ID: {ProductId}", id);
                
                var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == id);
                if (product == null)
                {
                    _logger.LogWarning("Product with ID {ProductId} not found for deletion", id);
                    return false;
                }

                _context.Products.Remove(product);
                await _context.SaveChangesAsync();

                _logger.LogInformation("Successfully deleted product with ID: {ProductId}", id);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting product with ID: {ProductId}", id);
                throw;
            }
        }

        /// <summary>
        /// Decorates product name with prefix if not already present
        /// </summary>
        /// <param name="product">Product to decorate</param>
        private void DecorateProductName(Product product)
        {
            if (!product.Name.StartsWith(ProductPrefix))
            {
                product.Name = ProductPrefix + product.Name;
                _logger.LogDebug("Decorated product name with prefix: {ProductName}", product.Name);
            }
        }
    }
}
