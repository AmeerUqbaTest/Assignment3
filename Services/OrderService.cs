using Assignment.Models;
using Assignment.Repositories;
using Assignment.Strategies;
using Assignment.Singletons;

namespace Assignment.Services
{
    public interface IOrderService
    {
        Task<Order> CreateOrderAsync(string customerId, string customerName);
        Task AddItemToOrderAsync(string orderId, string productId, int quantity);
        Task<bool> ProcessOrderPaymentAsync(string orderId, IPaymentStrategy paymentStrategy, Dictionary<string, string> paymentDetails);
        Task<IEnumerable<Order>> GetAllOrdersAsync();
        Task<Order?> GetOrderByIdAsync(string id);
        Task<IEnumerable<Order>> GetOrdersByCustomerAsync(string customerId);
        Task<IEnumerable<Order>> GetOrdersByDateRangeAsync(DateTime start, DateTime end);
        Task UpdateOrderStatusAsync(string orderId, OrderStatus status);
    }

    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IProductRepository _productRepository;
        private readonly ILogger _logger;

        public OrderService(IOrderRepository orderRepository, IProductRepository productRepository)
        {
            _orderRepository = orderRepository;
            _productRepository = productRepository;
            _logger = Logger.Instance;
        }

        public async Task<Order> CreateOrderAsync(string customerId, string customerName)
        {
            try
            {
                var orderId = Guid.NewGuid().ToString();
                var order = new Order(orderId, customerId, customerName);
                await _orderRepository.AddAsync(order);
                _logger.LogInfo($"Order {orderId} created for customer {customerName}");
                return order;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to create order: {ex.Message}");
                throw;
            }
        }

        public async Task AddItemToOrderAsync(string orderId, string productId, int quantity)
        {
            try
            {
                var order = await _orderRepository.GetByIdAsync(orderId);
                if (order == null)
                    throw new InvalidOperationException($"Order {orderId} not found");

                var product = await _productRepository.GetByIdAsync(productId);
                if (product == null)
                    throw new InvalidOperationException($"Product {productId} not found");

                if (product.StockQuantity < quantity)
                    throw new InvalidOperationException($"Insufficient stock for {product.Name}. Available: {product.StockQuantity}, Requested: {quantity}");

                order.AddItem(product, quantity);
                
                // Update product stock
                await _productRepository.UpdateStockAsync(productId, product.StockQuantity - quantity);
                
                await _orderRepository.UpdateAsync(order);
                _logger.LogInfo($"Added {quantity} units of {product.Name} to order {orderId}");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to add item to order: {ex.Message}");
                throw;
            }
        }

        public async Task<bool> ProcessOrderPaymentAsync(string orderId, IPaymentStrategy paymentStrategy, Dictionary<string, string> paymentDetails)
        {
            try
            {
                var order = await _orderRepository.GetByIdAsync(orderId);
                if (order == null)
                    throw new InvalidOperationException($"Order {orderId} not found");

                var paymentContext = new PaymentContext(paymentStrategy);
                var paymentSuccess = paymentContext.ProcessPayment(order.TotalAmount, paymentDetails);

                if (paymentSuccess)
                {
                    order.Status = OrderStatus.Processing;
                    order.PaymentMethod = paymentStrategy.GetPaymentMethodName();
                    await _orderRepository.UpdateAsync(order);
                    _logger.LogInfo($"Payment processed successfully for order {orderId}");
                }
                else
                {
                    _logger.LogWarning($"Payment failed for order {orderId}");
                }

                return paymentSuccess;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to process payment for order {orderId}: {ex.Message}");
                throw;
            }
        }

        public async Task<IEnumerable<Order>> GetAllOrdersAsync()
        {
            return await _orderRepository.GetAllAsync();
        }

        public async Task<Order?> GetOrderByIdAsync(string id)
        {
            return await _orderRepository.GetByIdAsync(id);
        }

        public async Task<IEnumerable<Order>> GetOrdersByCustomerAsync(string customerId)
        {
            return await _orderRepository.GetOrdersByCustomerAsync(customerId);
        }

        public async Task<IEnumerable<Order>> GetOrdersByDateRangeAsync(DateTime start, DateTime end)
        {
            return await _orderRepository.GetOrdersByDateRangeAsync(start, end);
        }

        public async Task UpdateOrderStatusAsync(string orderId, OrderStatus status)
        {
            try
            {
                var order = await _orderRepository.GetByIdAsync(orderId);
                if (order == null)
                    throw new InvalidOperationException($"Order {orderId} not found");

                order.Status = status;
                await _orderRepository.UpdateAsync(order);
                _logger.LogInfo($"Order {orderId} status updated to {status}");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to update order status: {ex.Message}");
                throw;
            }
        }
    }
}
