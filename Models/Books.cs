namespace Assignment.Models
{
    public class Books : Product
    {
        public string Isbn { get; set; }
        public string Author { get; set; }
        public string Publisher { get; set; }
        public int Pages { get; set; }

        public Books(string id, string name, decimal price, string description, 
                    int stockQuantity, string isbn, string author, string publisher, int pages)
            : base(id, name, price, description, ProductCategory.Books, stockQuantity)
        {
            Isbn = isbn;
            Author = author;
            Publisher = publisher;
            Pages = pages;
        }

        public override string GetProductDetails()
        {
            return $"Book - {Name}\n" +
                   $"Author: {Author}\n" +
                   $"Publisher: {Publisher}\n" +
                   $"ISBN: {Isbn}\n" +
                   $"Pages: {Pages}\n" +
                   $"Price: ${Price:F2}\n" +
                   $"Stock: {StockQuantity} units\n" +
                   $"Description: {Description}";
        }

        public override void ValidateProduct()
        {
            if (string.IsNullOrEmpty(Isbn))
                throw new ArgumentException("Books must have an ISBN");
            if (string.IsNullOrEmpty(Author))
                throw new ArgumentException("Books must have an author");
            if (Pages <= 0)
                throw new ArgumentException("Books must have a positive page count");
        }
    }
}
