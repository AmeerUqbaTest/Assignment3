using Assignment.Models;

namespace Assignment.Factories
{
    public interface IProductFactory
    {
        Product CreateProduct(ProductCategory category, Dictionary<string, object> parameters);
    }

    public class ProductFactory : IProductFactory
    {
        public Product CreateProduct(ProductCategory category, Dictionary<string, object> parameters)
        {
            try
            {
                return category switch
                {
                    ProductCategory.Electronics => CreateElectronics(parameters),
                    ProductCategory.Clothing => CreateClothing(parameters),
                    ProductCategory.Books => CreateBooks(parameters),
                    ProductCategory.HomeGarden => CreateHomeGarden(parameters),
                    _ => throw new ArgumentException($"Invalid product category: {category}")
                };
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Failed to create product of category {category}: {ex.Message}", ex);
            }
        }

        private Electronics CreateElectronics(Dictionary<string, object> parameters)
        {
            return new Electronics(
                id: parameters["id"].ToString()!,
                name: parameters["name"].ToString()!,
                price: Convert.ToDecimal(parameters["price"]),
                description: parameters["description"].ToString()!,
                stockQuantity: Convert.ToInt32(parameters["stockQuantity"]),
                warrantyMonths: Convert.ToInt32(parameters["warrantyMonths"]),
                brand: parameters["brand"].ToString()!,
                model: parameters["model"].ToString()!
            );
        }

        private Clothing CreateClothing(Dictionary<string, object> parameters)
        {
            return new Clothing(
                id: parameters["id"].ToString()!,
                name: parameters["name"].ToString()!,
                price: Convert.ToDecimal(parameters["price"]),
                description: parameters["description"].ToString()!,
                stockQuantity: Convert.ToInt32(parameters["stockQuantity"]),
                size: parameters["size"].ToString()!,
                color: parameters["color"].ToString()!,
                material: parameters["material"].ToString()!
            );
        }

        private Books CreateBooks(Dictionary<string, object> parameters)
        {
            return new Books(
                id: parameters["id"].ToString()!,
                name: parameters["name"].ToString()!,
                price: Convert.ToDecimal(parameters["price"]),
                description: parameters["description"].ToString()!,
                stockQuantity: Convert.ToInt32(parameters["stockQuantity"]),
                isbn: parameters["isbn"].ToString()!,
                author: parameters["author"].ToString()!,
                publisher: parameters["publisher"].ToString()!,
                pages: Convert.ToInt32(parameters["pages"])
            );
        }

        private HomeGarden CreateHomeGarden(Dictionary<string, object> parameters)
        {
            return new HomeGarden(
                id: parameters["id"].ToString()!,
                name: parameters["name"].ToString()!,
                price: Convert.ToDecimal(parameters["price"]),
                description: parameters["description"].ToString()!,
                stockQuantity: Convert.ToInt32(parameters["stockQuantity"]),
                category: parameters["category"].ToString()!,
                isIndoor: Convert.ToBoolean(parameters["isIndoor"]),
                dimensions: parameters["dimensions"].ToString()!
            );
        }
    }
}
