using Assignment.Models;
using Assignment.Repositories;
using Assignment.Factories;
using Assignment.Singletons;

namespace Assignment.Services
{
    public interface IProductService
    {
        Task<Product> CreateProductAsync(ProductCategory category, Dictionary<string, object> parameters);
        Task<IEnumerable<Product>> GetAllProductsAsync();
        Task<Product?> GetProductByIdAsync(string id);
        Task<IEnumerable<Product>> GetProductsByCategoryAsync(ProductCategory category);
        Task<IEnumerable<Product>> GetLowStockProductsAsync(int threshold);
        Task UpdateProductStockAsync(string productId, int newStock);
    }

    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly IProductFactory _productFactory;
        private readonly ILogger _logger;

        public ProductService(IProductRepository productRepository, IProductFactory productFactory)
        {
            _productRepository = productRepository;
            _productFactory = productFactory;
            _logger = Logger.Instance;
        }

        public async Task<Product> CreateProductAsync(ProductCategory category, Dictionary<string, object> parameters)
        {
            try
            {
                _logger.LogInfo($"Creating new product of category {category}");
                var product = _productFactory.CreateProduct(category, parameters);
                await _productRepository.AddAsync(product);
                _logger.LogInfo($"Product {product.Name} created successfully with ID {product.Id}");
                return product;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to create product: {ex.Message}");
                throw;
            }
        }

        public async Task<IEnumerable<Product>> GetAllProductsAsync()
        {
            return await _productRepository.GetAllAsync();
        }

        public async Task<Product?> GetProductByIdAsync(string id)
        {
            return await _productRepository.GetByIdAsync(id);
        }

        public async Task<IEnumerable<Product>> GetProductsByCategoryAsync(ProductCategory category)
        {
            return await _productRepository.GetByCategoryAsync(category);
        }

        public async Task<IEnumerable<Product>> GetLowStockProductsAsync(int threshold)
        {
            return await _productRepository.GetLowStockProductsAsync(threshold);
        }

        public async Task UpdateProductStockAsync(string productId, int newStock)
        {
            try
            {
                await _productRepository.UpdateStockAsync(productId, newStock);
                _logger.LogInfo($"Updated stock for product {productId} to {newStock}");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to update stock for product {productId}: {ex.Message}");
                throw;
            }
        }
    }
}
