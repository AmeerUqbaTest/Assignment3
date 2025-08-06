namespace Assignment.Models
{
    public abstract class Product
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }
        public ProductCategory Category { get; set; }
        public int StockQuantity { get; set; }
        public DateTime CreatedDate { get; set; }

        protected Product(string id, string name, decimal price, string description, ProductCategory category, int stockQuantity)
        {
            Id = id;
            Name = name;
            Price = price;
            Description = description;
            Category = category;
            StockQuantity = stockQuantity;
            CreatedDate = DateTime.Now;
        }

        public abstract string GetProductDetails();
        public abstract void ValidateProduct();
    }
}
