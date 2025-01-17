# 🛍️ Multi-Layered ASP.NET Core Web API - Online Shopping Platform

Welcome to the Multi-Layered Online Shopping Platform built using **ASP.NET Core Web API**! This project demonstrates a clean architecture approach with multiple layers for scalability, maintainability, and reusability. 🚀

## 🌟 Features

- **Multi-Layered Architecture**: Clear separation of concerns with `API`, `Business`, `Data Access`, `Domain`, and `Tests` layers.
- **Entity Framework Core**: Code First approach for database interactions.
- **Authentication & Authorization**: 
  - ASP.NET Core Identity for user management.
  - JWT for secure token-based authorization.
- **Middleware**: 
  - Custom middleware for logging incoming requests.
  - Maintenance middleware for service downtime management.
- **Validation**: Robust validation using data annotations.
- **Dependency Injection**: Built-in DI for managing services.
- **Global Exception Handling**: Centralized error management.

## 🧠 Technologies

- **ASP.NET Core 6+**
- **Microsoft SQL Server**
- **Entity Framework Core**
- **JWT Authentication**
- **xUnit**
- **AutoMapper**
- **Serilog**

## 🏗️ Architecture

The project is designed with a **multi-layered architecture** to separate responsibilities:

1. **API Layer (Presentation)**: Contains Controllers for handling HTTP requests and responses.
2. **Business Layer**: Implements business logic and validation.
3. **Data Access Layer**: Manages database operations using repositories and Unit of Work.
4. **Domain Layer**: Contains entities and shared models.
5. **Tests Layer**: Unit tests for ensuring code quality and reliability.

## 🗂️ Project Structure

```
📁 MultiLayeredShoppingPlatform
├── 📂 API
│   ├── Controllers
│   └── Program.cs
├── 📂 Business
│   ├── Interfaces
│   ├── Services
│   └── DTOs
├── 📂 Data Access
│   ├── Repositories
│   ├── UnitOfWork
│   └── DbContext
├── 📂 Domain
│   ├── Entities
│   ├── Enums
├── 📂 Tests
│   ├── UnitTests
└── README.md
```

## ✨ Key Components

### Entities

- **User**:
  - Properties: `Id`, `FirstName`, `LastName`, `Email`, `PhoneNumber`, `Password`, `Role`.
  - Passwords are encrypted using **Data Protection**.
  
- **Product**:
  - Properties: `Id`, `ProductName`, `Price`, `StockQuantity`.

- **Order**:
  - Properties: `Id`, `OrderDate`, `TotalAmount`, `CustomerId`.
  - Relationships: One-to-Many with `User`.

- **OrderProduct**:
  - Properties: `OrderId`, `ProductId`, `Quantity`.
  - Relationships: Many-to-Many between `Order` and `Product`.

### Middleware

- **Logging Middleware**: Logs each request's URL, timestamp, user ID and user IP.
- **Maintenance Middleware**: Restricts access during maintenance periods.

### Business Logic

- Implements business rules and communicates with the Data Access layer.
- Uses **AutoMapper** for mapping between entities and DTOs.

### Validation

- **Data Annotations**: Validates models (e.g., required fields, email format).
- **FluentValidation**: Optional for complex validations.

### Exception Handling

- A global exception handler captures all errors and provides meaningful responses.

## 🚀 Getting Started

1. Clone the repository:
   ```bash
   git clone https://github.com/nazifkaraca/E-Commerce.git
   ```

2. Navigate to the project folder:
   ```bash
   cd E-Commerce
   ```

3. Restore dependencies:
   ```bash
   dotnet restore
   ```

4. Update the database:
   ```bash
   dotnet ef database update
   ```

5. Run the application:
   ```bash
   dotnet run --project API
   ```

6. Open Swagger to explore the API:
   ```
   https://localhost:5001/swagger
   ```

## 🧪 Testing

Run unit and integration tests:
```bash
dotnet test
```

## 🛠️ Future Enhancements

- Implement a **payment gateway** (e.g., Iyzico).
- Add more complex **caching mechanisms**.
- Build a **frontend client**.

## 📄 License

This project is licensed under the [MIT License](LICENSE).

## 📬 Contact

For any queries, reach out to [nazif808@gmail.com](mailto:nazif808@karaca.com).
