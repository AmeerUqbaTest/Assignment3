using Assignment.Models;
using Assignment.Factories;
using Assignment.Repositories;
using Assignment.Services;
using Assignment.Singletons;
using Assignment.Strategies;

namespace Assignment
{
    class Program
    {
        private static IProductService _productService = null!;
        private static IOrderService _orderService = null!;
        private static ILogger _logger = null!;
        private static IConfigurationManager _configManager = null!;

        static async Task Main(string[] args)
        {
            try
            {
                InitializeServices();
                await SeedInitialDataAsync();
                await RunApplicationAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Application error: {ex.Message}");
            }
        }

        private static void InitializeServices()
        {
            // Initialize Singletons
            _logger = Logger.Instance;
            _configManager = ConfigurationManager.Instance;

            // Initialize Repositories
            var productRepository = new ProductRepository();
            var orderRepository = new OrderRepository();

            // Initialize Factory
            var productFactory = new ProductFactory();

            // Initialize Services
            _productService = new ProductService(productRepository, productFactory);
            _orderService = new OrderService(orderRepository, productRepository);

            _logger.LogInfo("E-Commerce Order Management System initialized");
        }

        private static async Task SeedInitialDataAsync()
        {
            try
            {
                // Seed some initial products
                await _productService.CreateProductAsync(ProductCategory.Electronics, new Dictionary<string, object>
                {
                    {"id", "ELEC001"},
                    {"name", "Gaming Laptop"},
                    {"price", 1299.99m},
                    {"description", "High-performance gaming laptop with RTX graphics"},
                    {"stockQuantity", 10},
                    {"warrantyMonths", 24},
                    {"brand", "TechBrand"},
                    {"model", "GX-2024"}
                });

                await _productService.CreateProductAsync(ProductCategory.Clothing, new Dictionary<string, object>
                {
                    {"id", "CLOTH001"},
                    {"name", "Premium T-Shirt"},
                    {"price", 29.99m},
                    {"description", "Comfortable cotton t-shirt"},
                    {"stockQuantity", 50},
                    {"size", "L"},
                    {"color", "Blue"},
                    {"material", "100% Cotton"}
                });

                await _productService.CreateProductAsync(ProductCategory.Books, new Dictionary<string, object>
                {
                    {"id", "BOOK001"},
                    {"name", "Design Patterns Book"},
                    {"price", 49.99m},
                    {"description", "Comprehensive guide to software design patterns"},
                    {"stockQuantity", 25},
                    {"isbn", "978-0201633610"},
                    {"author", "Gang of Four"},
                    {"publisher", "Addison-Wesley"},
                    {"pages", 395}
                });

                await _productService.CreateProductAsync(ProductCategory.HomeGarden, new Dictionary<string, object>
                {
                    {"id", "HOME001"},
                    {"name", "Indoor Plant Pot"},
                    {"price", 19.99m},
                    {"description", "Decorative ceramic plant pot"},
                    {"stockQuantity", 30},
                    {"category", "Decorative"},
                    {"isIndoor", true},
                    {"dimensions", "8x8x10 inches"}
                });

                _logger.LogInfo("Initial data seeded successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to seed initial data: {ex.Message}");
            }
        }

        private static async Task RunApplicationAsync()
        {
            bool running = true;
            while (running)
            {
                try
                {
                    ShowMainMenu();
                    var choice = Console.ReadLine();

                    switch (choice)
                    {
                        case "1":
                            await ViewProductsAsync();
                            break;
                        case "2":
                            await AddNewProductAsync();
                            break;
                        case "3":
                            await CreateOrderAsync();
                            break;
                        case "4":
                            await ProcessPaymentAsync();
                            break;
                        case "5":
                            await ViewOrdersAsync();
                            break;
                        case "6":
                            await CheckInventoryAsync();
                            break;
                        case "7":
                            ViewSystemLogs();
                            break;
                        case "8":
                            ViewConfiguration();
                            break;
                        case "9":
                            running = false;
                            _logger.LogInfo("Application shutting down");
                            Console.WriteLine("Thank you for using E-Commerce Order Management System!");
                            break;
                        default:
                            Console.WriteLine("Invalid option. Please try again.");
                            break;
                    }

                    if (running)
                    {
                        Console.WriteLine("\nPress any key to continue...");
                        Console.ReadKey();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                    _logger.LogError($"Menu operation error: {ex.Message}");
                }
            }
        }

        private static void ShowMainMenu()
        {
            Console.Clear();
            Console.WriteLine("=== E-Commerce Order Management System ===");
            Console.WriteLine("1. View Products");
            Console.WriteLine("2. Add New Product");
            Console.WriteLine("3. Create Order");
            Console.WriteLine("4. Process Payment");
            Console.WriteLine("5. View Orders");
            Console.WriteLine("6. Check Inventory");
            Console.WriteLine("7. System Logs");
            Console.WriteLine("8. Configuration");
            Console.WriteLine("9. Exit");
            Console.Write("Please select an option: ");
        }

        private static async Task ViewProductsAsync()
        {
            Console.Clear();
            Console.WriteLine("=== Product Catalog ===\n");

            var products = await _productService.GetAllProductsAsync();
            if (!products.Any())
            {
                Console.WriteLine("No products available.");
                return;
            }

            foreach (var product in products)
            {
                Console.WriteLine(product.GetProductDetails());
                Console.WriteLine(new string('-', 50));
            }
        }

        private static async Task AddNewProductAsync()
        {
            Console.Clear();
            Console.WriteLine("=== Add New Product ===\n");

            try
            {
                Console.WriteLine("Select product category:");
                Console.WriteLine("1. Electronics");
                Console.WriteLine("2. Clothing");
                Console.WriteLine("3. Books");
                Console.WriteLine("4. Home & Garden");
                Console.Write("Choice: ");

                var categoryChoice = Console.ReadLine();
                ProductCategory category = categoryChoice switch
                {
                    "1" => ProductCategory.Electronics,
                    "2" => ProductCategory.Clothing,
                    "3" => ProductCategory.Books,
                    "4" => ProductCategory.HomeGarden,
                    _ => throw new ArgumentException("Invalid category selection")
                };

                var parameters = new Dictionary<string, object>();

                // Common parameters
                Console.Write("Product ID: ");
                parameters["id"] = Console.ReadLine()!;

                Console.Write("Product Name: ");
                parameters["name"] = Console.ReadLine()!;

                Console.Write("Price: $");
                parameters["price"] = decimal.Parse(Console.ReadLine()!);

                Console.Write("Description: ");
                parameters["description"] = Console.ReadLine()!;

                Console.Write("Stock Quantity: ");
                parameters["stockQuantity"] = int.Parse(Console.ReadLine()!);

                // Category-specific parameters
                switch (category)
                {
                    case ProductCategory.Electronics:
                        Console.Write("Brand: ");
                        parameters["brand"] = Console.ReadLine()!;
                        Console.Write("Model: ");
                        parameters["model"] = Console.ReadLine()!;
                        Console.Write("Warranty (months): ");
                        parameters["warrantyMonths"] = int.Parse(Console.ReadLine()!);
                        break;

                    case ProductCategory.Clothing:
                        Console.Write("Size: ");
                        parameters["size"] = Console.ReadLine()!;
                        Console.Write("Color: ");
                        parameters["color"] = Console.ReadLine()!;
                        Console.Write("Material: ");
                        parameters["material"] = Console.ReadLine()!;
                        break;

                    case ProductCategory.Books:
                        Console.Write("ISBN: ");
                        parameters["isbn"] = Console.ReadLine()!;
                        Console.Write("Author: ");
                        parameters["author"] = Console.ReadLine()!;
                        Console.Write("Publisher: ");
                        parameters["publisher"] = Console.ReadLine()!;
                        Console.Write("Pages: ");
                        parameters["pages"] = int.Parse(Console.ReadLine()!);
                        break;

                    case ProductCategory.HomeGarden:
                        Console.Write("Category: ");
                        parameters["category"] = Console.ReadLine()!;
                        Console.Write("Is Indoor (true/false): ");
                        parameters["isIndoor"] = bool.Parse(Console.ReadLine()!);
                        Console.Write("Dimensions: ");
                        parameters["dimensions"] = Console.ReadLine()!;
                        break;
                }

                var product = await _productService.CreateProductAsync(category, parameters);
                Console.WriteLine($"\nProduct '{product.Name}' added successfully!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error adding product: {ex.Message}");
            }
        }

        private static async Task CreateOrderAsync()
        {
            Console.Clear();
            Console.WriteLine("=== Create New Order ===\n");

            try
            {
                Console.Write("Customer ID: ");
                var customerId = Console.ReadLine()!;

                Console.Write("Customer Name: ");
                var customerName = Console.ReadLine()!;

                var order = await _orderService.CreateOrderAsync(customerId, customerName);
                Console.WriteLine($"Order {order.Id} created successfully!");

                // Add items to order
                bool addingItems = true;
                while (addingItems)
                {
                    Console.WriteLine("\nWould you like to add items to this order? (y/n)");
                    var response = Console.ReadLine()?.ToLower();

                    if (response == "y" || response == "yes")
                    {
                        await AddItemToOrderAsync(order.Id);
                    }
                    else
                    {
                        addingItems = false;
                    }
                }

                // Display order summary
                var updatedOrder = await _orderService.GetOrderByIdAsync(order.Id);
                if (updatedOrder != null)
                {
                    Console.WriteLine($"\nOrder Summary:");
                    Console.WriteLine($"Order ID: {updatedOrder.Id}");
                    Console.WriteLine($"Customer: {updatedOrder.CustomerName}");
                    Console.WriteLine($"Total Amount: ${updatedOrder.TotalAmount:F2}");
                    Console.WriteLine($"Status: {updatedOrder.Status}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating order: {ex.Message}");
            }
        }

        private static async Task AddItemToOrderAsync(string orderId)
        {
            try
            {
                // Show available products
                var products = await _productService.GetAllProductsAsync();
                Console.WriteLine("\nAvailable Products:");
                foreach (var product in products)
                {
                    Console.WriteLine($"{product.Id}: {product.Name} - ${product.Price:F2} (Stock: {product.StockQuantity})");
                }

                Console.Write("\nEnter Product ID: ");
                var productId = Console.ReadLine()!;

                Console.Write("Enter Quantity: ");
                var quantity = int.Parse(Console.ReadLine()!);

                await _orderService.AddItemToOrderAsync(orderId, productId, quantity);
                Console.WriteLine("Item added to order successfully!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error adding item to order: {ex.Message}");
            }
        }

        private static async Task ProcessPaymentAsync()
        {
            Console.Clear();
            Console.WriteLine("=== Process Payment ===\n");

            try
            {
                Console.Write("Enter Order ID: ");
                var orderId = Console.ReadLine()!;

                var order = await _orderService.GetOrderByIdAsync(orderId);
                if (order == null)
                {
                    Console.WriteLine("Order not found.");
                    return;
                }

                Console.WriteLine($"Order Total: ${order.TotalAmount:F2}");
                Console.WriteLine("\nSelect Payment Method:");
                Console.WriteLine("1. Credit Card");
                Console.WriteLine("2. PayPal");
                Console.WriteLine("3. Bank Transfer");
                Console.WriteLine("4. Cryptocurrency");
                Console.Write("Choice: ");

                var paymentChoice = Console.ReadLine();
                IPaymentStrategy paymentStrategy;
                var paymentDetails = new Dictionary<string, string>();

                switch (paymentChoice)
                {
                    case "1":
                        paymentStrategy = new CreditCardPayment();
                        Console.Write("Card Number (16 digits): ");
                        paymentDetails["cardNumber"] = Console.ReadLine()!;
                        Console.Write("Expiry Date (MM/yy): ");
                        paymentDetails["expiryDate"] = Console.ReadLine()!;
                        Console.Write("CVV (3 digits): ");
                        paymentDetails["cvv"] = Console.ReadLine()!;
                        Console.Write("Card Holder Name: ");
                        paymentDetails["cardHolderName"] = Console.ReadLine()!;
                        break;

                    case "2":
                        paymentStrategy = new PayPalPayment();
                        Console.Write("PayPal Email: ");
                        paymentDetails["email"] = Console.ReadLine()!;
                        Console.Write("Password: ");
                        paymentDetails["password"] = Console.ReadLine()!;
                        break;

                    case "3":
                        paymentStrategy = new BankTransferPayment();
                        Console.Write("Routing Number (9 digits): ");
                        paymentDetails["routingNumber"] = Console.ReadLine()!;
                        Console.Write("Account Number: ");
                        paymentDetails["accountNumber"] = Console.ReadLine()!;
                        Console.Write("Account Holder Name: ");
                        paymentDetails["accountHolderName"] = Console.ReadLine()!;
                        break;

                    case "4":
                        paymentStrategy = new CryptoPayment();
                        Console.Write("Wallet Address: ");
                        paymentDetails["walletAddress"] = Console.ReadLine()!;
                        Console.WriteLine("Crypto Type (Bitcoin, Ethereum, Litecoin, Dogecoin): ");
                        paymentDetails["cryptoType"] = Console.ReadLine()!;
                        break;

                    default:
                        Console.WriteLine("Invalid payment method.");
                        return;
                }

                var success = await _orderService.ProcessOrderPaymentAsync(orderId, paymentStrategy, paymentDetails);
                
                if (success)
                {
                    Console.WriteLine("Payment processed successfully!");
                }
                else
                {
                    Console.WriteLine("Payment failed. Please try again.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error processing payment: {ex.Message}");
            }
        }

        private static async Task ViewOrdersAsync()
        {
            Console.Clear();
            Console.WriteLine("=== Order History ===\n");

            var orders = await _orderService.GetAllOrdersAsync();
            if (!orders.Any())
            {
                Console.WriteLine("No orders found.");
                return;
            }

            foreach (var order in orders)
            {
                Console.WriteLine($"Order ID: {order.Id}");
                Console.WriteLine($"Customer: {order.CustomerName} (ID: {order.CustomerId})");
                Console.WriteLine($"Order Date: {order.OrderDate:yyyy-MM-dd HH:mm:ss}");
                Console.WriteLine($"Status: {order.Status}");
                Console.WriteLine($"Payment Method: {order.PaymentMethod ?? "Not Set"}");
                Console.WriteLine($"Total Amount: ${order.TotalAmount:F2}");
                Console.WriteLine("Items:");
                foreach (var item in order.Items)
                {
                    Console.WriteLine($"  - {item.ProductName} x {item.Quantity} @ ${item.UnitPrice:F2} = ${item.TotalPrice:F2}");
                }
                Console.WriteLine(new string('-', 50));
            }
        }

        private static async Task CheckInventoryAsync()
        {
            Console.Clear();
            Console.WriteLine("=== Inventory Management ===\n");

            Console.WriteLine("1. View All Products");
            Console.WriteLine("2. View Low Stock Products");
            Console.WriteLine("3. View Products by Category");
            Console.Write("Choice: ");

            var choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    await ViewProductsAsync();
                    break;

                case "2":
                    Console.Write("Enter low stock threshold: ");
                    var threshold = int.Parse(Console.ReadLine()!);
                    var lowStockProducts = await _productService.GetLowStockProductsAsync(threshold);
                    
                    Console.WriteLine($"\nProducts with stock <= {threshold}:");
                    foreach (var product in lowStockProducts)
                    {
                        Console.WriteLine($"{product.Name}: {product.StockQuantity} units");
                    }
                    break;

                case "3":
                    Console.WriteLine("Select category:");
                    Console.WriteLine("1. Electronics");
                    Console.WriteLine("2. Clothing");
                    Console.WriteLine("3. Books");
                    Console.WriteLine("4. Home & Garden");
                    Console.Write("Choice: ");

                    var categoryChoice = Console.ReadLine();
                    ProductCategory category = categoryChoice switch
                    {
                        "1" => ProductCategory.Electronics,
                        "2" => ProductCategory.Clothing,
                        "3" => ProductCategory.Books,
                        "4" => ProductCategory.HomeGarden,
                        _ => throw new ArgumentException("Invalid category")
                    };

                    var categoryProducts = await _productService.GetProductsByCategoryAsync(category);
                    Console.WriteLine($"\n{category} Products:");
                    foreach (var product in categoryProducts)
                    {
                        Console.WriteLine(product.GetProductDetails());
                        Console.WriteLine(new string('-', 30));
                    }
                    break;

                default:
                    Console.WriteLine("Invalid choice.");
                    break;
            }
        }

        private static void ViewSystemLogs()
        {
            Console.Clear();
            Console.WriteLine("=== System Logs ===\n");

            var logs = ((Logger)_logger).GetLogs();
            if (!logs.Any())
            {
                Console.WriteLine("No logs available.");
                return;
            }

            foreach (var log in logs.TakeLast(20)) // Show last 20 logs
            {
                Console.WriteLine(log);
            }
        }

        private static void ViewConfiguration()
        {
            Console.Clear();
            Console.WriteLine("=== System Configuration ===\n");

            var settings = new[]
            {
                "DatabaseConnection",
                "ApiKey",
                "MaxOrderItems",
                "DefaultCurrency",
                "LogLevel"
            };

            foreach (var setting in settings)
            {
                var value = _configManager.GetSetting(setting);
                Console.WriteLine($"{setting}: {value}");
            }

            Console.WriteLine("\nWould you like to update a setting? (y/n)");
            var response = Console.ReadLine()?.ToLower();

            if (response == "y" || response == "yes")
            {
                Console.Write("Enter setting name: ");
                var key = Console.ReadLine()!;
                Console.Write("Enter new value: ");
                var value = Console.ReadLine()!;

                _configManager.SetSetting(key, value);
                Console.WriteLine("Setting updated successfully!");
            }
        }
    }
}
