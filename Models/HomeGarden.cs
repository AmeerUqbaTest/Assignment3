namespace Assignment.Models
{
    public class HomeGarden : Product
    {
        public new string Category { get; set; }
        public bool IsIndoor { get; set; }
        public string Dimensions { get; set; }

        public HomeGarden(string id, string name, decimal price, string description, 
                         int stockQuantity, string category, bool isIndoor, string dimensions)
            : base(id, name, price, description, ProductCategory.HomeGarden, stockQuantity)
        {
            Category = category;
            IsIndoor = isIndoor;
            Dimensions = dimensions;
        }

        public override string GetProductDetails()
        {
            return $"Home & Garden - {Name}\n" +
                   $"Category: {Category}\n" +
                   $"Indoor Use: {(IsIndoor ? "Yes" : "No")}\n" +
                   $"Dimensions: {Dimensions}\n" +
                   $"Price: ${Price:F2}\n" +
                   $"Stock: {StockQuantity} units\n" +
                   $"Description: {Description}";
        }

        public override void ValidateProduct()
        {
            if (string.IsNullOrEmpty(Category))
                throw new ArgumentException("Home & Garden products must have a category");
            if (string.IsNullOrEmpty(Dimensions))
                throw new ArgumentException("Home & Garden products must have dimensions");
        }
    }
}
