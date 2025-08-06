using Assignment.Models;

namespace Assignment.Repositories
{
    public interface IProductRepository : IRepository<Product>
    {
        Task<IEnumerable<Product>> GetByCategoryAsync(ProductCategory category);
        Task<IEnumerable<Product>> GetLowStockProductsAsync(int threshold);
        Task UpdateStockAsync(string productId, int newStock);
    }

    public class ProductRepository : IProductRepository
    {
        private readonly List<Product> _products = new List<Product>();

        public async Task<Product?> GetByIdAsync(string id)
        {
            await Task.Delay(10); // Simulate async operation
            return _products.FirstOrDefault(p => p.Id == id);
        }

        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            await Task.Delay(10); // Simulate async operation
            return _products.ToList();
        }

        public async Task AddAsync(Product entity)
        {
            await Task.Delay(10); // Simulate async operation
            if (_products.Any(p => p.Id == entity.Id))
                throw new InvalidOperationException($"Product with ID {entity.Id} already exists");
            
            entity.ValidateProduct();
            _products.Add(entity);
        }

        public async Task UpdateAsync(Product entity)
        {
            await Task.Delay(10); // Simulate async operation
            var existingProduct = _products.FirstOrDefault(p => p.Id == entity.Id);
            if (existingProduct == null)
                throw new InvalidOperationException($"Product with ID {entity.Id} not found");

            entity.ValidateProduct();
            var index = _products.IndexOf(existingProduct);
            _products[index] = entity;
        }

        public async Task DeleteAsync(string id)
        {
            await Task.Delay(10); // Simulate async operation
            var product = _products.FirstOrDefault(p => p.Id == id);
            if (product == null)
                throw new InvalidOperationException($"Product with ID {id} not found");

            _products.Remove(product);
        }

        public async Task<IEnumerable<Product>> GetByCategoryAsync(ProductCategory category)
        {
            await Task.Delay(10); // Simulate async operation
            return _products.Where(p => p.Category == category).ToList();
        }

        public async Task<IEnumerable<Product>> GetLowStockProductsAsync(int threshold)
        {
            await Task.Delay(10); // Simulate async operation
            return _products.Where(p => p.StockQuantity <= threshold).ToList();
        }

        public async Task UpdateStockAsync(string productId, int newStock)
        {
            await Task.Delay(10); // Simulate async operation
            var product = _products.FirstOrDefault(p => p.Id == productId);
            if (product == null)
                throw new InvalidOperationException($"Product with ID {productId} not found");

            product.StockQuantity = newStock;
        }
    }
}
