using Assignment.Models;

namespace Assignment.Repositories
{
    public interface IOrderRepository : IRepository<Order>
    {
        Task<IEnumerable<Order>> GetOrdersByCustomerAsync(string customerId);
        Task<IEnumerable<Order>> GetOrdersByDateRangeAsync(DateTime start, DateTime end);
        Task<IEnumerable<Order>> GetOrdersByStatusAsync(OrderStatus status);
    }

    public class OrderRepository : IOrderRepository
    {
        private readonly List<Order> _orders = new List<Order>();

        public async Task<Order?> GetByIdAsync(string id)
        {
            await Task.Delay(10); // Simulate async operation
            return _orders.FirstOrDefault(o => o.Id == id);
        }

        public async Task<IEnumerable<Order>> GetAllAsync()
        {
            await Task.Delay(10); // Simulate async operation
            return _orders.ToList();
        }

        public async Task AddAsync(Order entity)
        {
            await Task.Delay(10); // Simulate async operation
            if (_orders.Any(o => o.Id == entity.Id))
                throw new InvalidOperationException($"Order with ID {entity.Id} already exists");

            _orders.Add(entity);
        }

        public async Task UpdateAsync(Order entity)
        {
            await Task.Delay(10); // Simulate async operation
            var existingOrder = _orders.FirstOrDefault(o => o.Id == entity.Id);
            if (existingOrder == null)
                throw new InvalidOperationException($"Order with ID {entity.Id} not found");

            var index = _orders.IndexOf(existingOrder);
            _orders[index] = entity;
        }

        public async Task DeleteAsync(string id)
        {
            await Task.Delay(10); // Simulate async operation
            var order = _orders.FirstOrDefault(o => o.Id == id);
            if (order == null)
                throw new InvalidOperationException($"Order with ID {id} not found");

            _orders.Remove(order);
        }

        public async Task<IEnumerable<Order>> GetOrdersByCustomerAsync(string customerId)
        {
            await Task.Delay(10); // Simulate async operation
            return _orders.Where(o => o.CustomerId == customerId).ToList();
        }

        public async Task<IEnumerable<Order>> GetOrdersByDateRangeAsync(DateTime start, DateTime end)
        {
            await Task.Delay(10); // Simulate async operation
            return _orders.Where(o => o.OrderDate >= start && o.OrderDate <= end).ToList();
        }

        public async Task<IEnumerable<Order>> GetOrdersByStatusAsync(OrderStatus status)
        {
            await Task.Delay(10); // Simulate async operation
            return _orders.Where(o => o.Status == status).ToList();
        }
    }
}
