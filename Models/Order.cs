namespace Assignment.Models
{
    public class Order
    {
        public string Id { get; set; }
        public string CustomerId { get; set; }
        public string CustomerName { get; set; }
        public List<OrderItem> Items { get; set; }
        public decimal TotalAmount { get; set; }
        public DateTime OrderDate { get; set; }
        public OrderStatus Status { get; set; }
        public string? PaymentMethod { get; set; }

        public Order(string id, string customerId, string customerName)
        {
            Id = id;
            CustomerId = customerId;
            CustomerName = customerName;
            Items = new List<OrderItem>();
            OrderDate = DateTime.Now;
            Status = OrderStatus.Pending;
        }

        public void AddItem(Product product, int quantity)
        {
            if (product.StockQuantity < quantity)
                throw new InvalidOperationException($"Insufficient stock for {product.Name}");

            var existingItem = Items.FirstOrDefault(i => i.ProductId == product.Id);
            if (existingItem != null)
            {
                existingItem.Quantity += quantity;
            }
            else
            {
                Items.Add(new OrderItem
                {
                    ProductId = product.Id,
                    ProductName = product.Name,
                    UnitPrice = product.Price,
                    Quantity = quantity
                });
            }

            CalculateTotal();
        }

        public void CalculateTotal()
        {
            TotalAmount = Items.Sum(item => item.UnitPrice * item.Quantity);
        }
    }

    public class OrderItem
    {
        public required string ProductId { get; set; }
        public required string ProductName { get; set; }
        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; }
        public decimal TotalPrice => UnitPrice * Quantity;
    }

    public enum OrderStatus
    {
        Pending,
        Processing,
        Shipped,
        Delivered,
        Cancelled
    }
}
