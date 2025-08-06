namespace Assignment.Models
{
    public class Electronics : Product
    {
        public int WarrantyMonths { get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }

        public Electronics(string id, string name, decimal price, string description, 
                          int stockQuantity, int warrantyMonths, string brand, string model)
            : base(id, name, price, description, ProductCategory.Electronics, stockQuantity)
        {
            WarrantyMonths = warrantyMonths;
            Brand = brand;
            Model = model;
        }

        public override string GetProductDetails()
        {
            return $"Electronics - {Name}\n" +
                   $"Brand: {Brand}\n" +
                   $"Model: {Model}\n" +
                   $"Price: ${Price:F2}\n" +
                   $"Warranty: {WarrantyMonths} months\n" +
                   $"Stock: {StockQuantity} units\n" +
                   $"Description: {Description}";
        }

        public override void ValidateProduct()
        {
            if (string.IsNullOrEmpty(Brand))
                throw new ArgumentException("Electronics must have a brand");
            if (WarrantyMonths < 0)
                throw new ArgumentException("Warranty months cannot be negative");
        }
    }
}
