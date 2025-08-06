# E-Commerce Order Management System

A comprehensive console application demonstrating advanced Object-Oriented Programming concepts, Design Patterns, and SOLID principles in C#.

## Quick Start

### Prerequisites
- .NET 9.0 or later
- Visual Studio Code or Visual Studio

### Setup and Run
1. Navigate to the project directory:
   ```bash
   cd <folder>
   ```

2. Build the project:
   ```bash
   dotnet build
   ```

3. Run the application:
   ```bash
   dotnet run
   ```
   
   Or run the executable directly:
   ```bash
   "bin\Debug\net9.0\Assignment.exe"
   ```

## Features

### Design Patterns Implemented
- **Factory Pattern**: Product creation for different categories
- **Singleton Pattern**: Configuration management and logging
- **Strategy Pattern**: Multiple payment processing methods
- **Repository Pattern**: Data access abstraction

### Core Functionality
- ✅ Product Management (Electronics, Clothing, Books, Home & Garden)
- ✅ Order Processing with multiple items
- ✅ Payment Processing (Credit Card, PayPal, Bank Transfer, Cryptocurrency)
- ✅ Inventory Tracking and stock management
- ✅ System Logging and Configuration management
- ✅ Interactive Console Menu System

### SOLID Principles
- **Single Responsibility**: Each class has one clear purpose
- **Open/Closed**: System is extensible without modification
- **Liskov Substitution**: Derived classes can replace base classes
- **Interface Segregation**: Focused, specific interfaces
- **Dependency Inversion**: Depends on abstractions, not concretions

## Application Menu

```
=== E-Commerce Order Management System ===
1. View Products
2. Add New Product
3. Create Order
4. Process Payment
5. View Orders
6. Check Inventory
7. System Logs
8. Configuration
9. Exit
```

## Sample Usage Flow

1. **View Products** - See pre-loaded sample products
2. **Create Order** - Create a new customer order
3. **Add Items** - Add products to the order
4. **Process Payment** - Select payment method and complete transaction
5. **View Orders** - See order history and details

## Architecture

The system follows a layered architecture:
- **Presentation Layer**: Console menu interface
- **Services Layer**: Business logic and orchestration
- **Repository Layer**: Data access abstraction
- **Models Layer**: Domain entities and business objects

## Documentation

For detailed architecture information, design patterns explanation, and SOLID principles application, see [DESIGN_DOCUMENT.md](DESIGN_DOCUMENT.md).

## Project Structure

```
Assignment/
├── Models/           # Domain models and entities
├── Factories/        # Factory pattern implementations
├── Singletons/       # Singleton pattern implementations
├── Strategies/       # Strategy pattern implementations
├── Repositories/     # Repository pattern implementations
├── Services/         # Business logic layer
├── Program.cs        # Main application entry point
└── DESIGN_DOCUMENT.md # Detailed architecture documentation
```

## Sample Data

The application comes pre-loaded with sample products:
- Gaming Laptop (Electronics)
- Premium T-Shirt (Clothing)
- Design Patterns Book (Books)
- Indoor Plant Pot (Home & Garden)

## Error Handling

The application includes comprehensive error handling:
- Input validation
- Business rule validation
- Graceful error recovery
- Detailed error logging

## Thread Safety

All singleton implementations are thread-safe using:
- `Lazy<T>` initialization
- Lock statements for critical sections
- Thread-safe collections

Enjoy exploring the E-Commerce Order Management System!
