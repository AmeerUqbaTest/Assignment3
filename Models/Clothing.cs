namespace Assignment.Models
{
    public class Clothing : Product
    {
        public string Size { get; set; }
        public string Color { get; set; }
        public string Material { get; set; }

        public Clothing(string id, string name, decimal price, string description, 
                       int stockQuantity, string size, string color, string material)
            : base(id, name, price, description, ProductCategory.Clothing, stockQuantity)
        {
            Size = size;
            Color = color;
            Material = material;
        }

        public override string GetProductDetails()
        {
            return $"Clothing - {Name}\n" +
                   $"Size: {Size}\n" +
                   $"Color: {Color}\n" +
                   $"Material: {Material}\n" +
                   $"Price: ${Price:F2}\n" +
                   $"Stock: {StockQuantity} units\n" +
                   $"Description: {Description}";
        }

        public override void ValidateProduct()
        {
            if (string.IsNullOrEmpty(Size))
                throw new ArgumentException("Clothing must have a size");
            if (string.IsNullOrEmpty(Color))
                throw new ArgumentException("Clothing must have a color");
        }
    }
}
