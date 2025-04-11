# Domain-Centric REST API Template

This template implements a clean, maintainable REST API architecture in .NET based on domain-centric design principles (also known as ports and adapters pattern or hexagonal architecture).

## Architecture Overview

The application is structured into three main layers:

```
YourProject/
├── Domain/                   # Core business logic
│   ├── Abstract/             # Core interfaces (IDomainService, IDataService)
│   ├── Contracts/            # Data transfer objects (DTOs)
│   ├── Extensions/           # Mapping utilities between contracts and domain models
│   ├── Model/                # Domain entities
│   ├── Registrations/        # Domain-layer dependency registrations
│   └── Services/             # Implementations of domain interfaces
├── Application/              # API's public interface
│   └── Endpoints/            # API endpoint definitions
└── Infrastructure/           # External system interactions
    └── Data/                 # Database interactions
        ├── Context/          # Entity Framework context
        ├── Registrations/    # Data-layer dependency registrations
        └── Services/         # Implementations of data interfaces
```

## Core Design Principles

1. **Domain-Centric Architecture**: Dependencies point inward, with the domain layer at the center
2. **Clean Separation of Concerns**: Each layer has clear responsibilities
3. **Interface-Based Design**: Components interact through interfaces, facilitating testing and flexibility
4. **Data Transformation**: Clear mapping between API contracts and domain models

## Getting Started

### 1. Clone the template

```bash
git clone <repository-url>
cd <project-name>
```

### 2. Update project and namespace references

- Rename solution and project files to match your project name
- Update namespaces throughout the codebase

### 3. Configure database connection

Create a user secret for your database connection string:

```bash
dotnet user-secrets init
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "your-connection-string"
```

> ⚠️ **Important**: Never store connection strings in `appsettings.json` - use secrets management instead.

### 4. Add your domain models

1. Create your entity classes in `Domain/Model/`
2. Make sure to mark appropriate properties as required or nullable
3. Consider adding a base entity class for common properties (Id, CreatedAt, etc.)

Example:

```csharp
public sealed class Product
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public string? Description { get; set; }
    public decimal Price { get; set; }
}
```

### 5. Create contracts (DTOs)

Define your API contracts in `Domain/Contracts/`:

```csharp
public sealed record AddProductRequest(string Name, string? Description, decimal Price);
public sealed record ProductResponse(int Id, string Name, string? Description, decimal Price);
```

### 6. Add mapping extensions

Create extension methods in `Domain/Extensions/` to map between contracts and domain models:

```csharp
public static class ProductExtensions
{
    public static Product ToEntity(this AddProductRequest request)
    {
        return new Product
        {
            Name = request.Name,
            Description = request.Description,
            Price = request.Price
        };
    }

    public static ProductResponse ToResponse(this Product product)
    {
        return new ProductResponse(
            product.Id,
            product.Name,
            product.Description,
            product.Price);
    }
}
```

### 7. Update domain interfaces

Add your operations to `Domain/Abstract/IDomainService.cs` and `Domain/Abstract/IDataService.cs`:

```csharp
// IDomainService
Task<ProductResponse> AddProduct(AddProductRequest request);
Task<IEnumerable<ProductResponse>> GetAllProducts();

// IDataService
Task<Product> AddProduct(Product product);
Task<IEnumerable<Product>> GetAllProducts();
```

### 8. Implement domain services

Implement the interfaces in `Domain/Services/DomainService.cs`:

```csharp
public async Task<ProductResponse> AddProduct(AddProductRequest request)
{
    // Validate request if needed
    Product product = request.ToEntity();
    product = await _dataService.AddProduct(product);
    return product.ToResponse();
}
```

### 9. Set up database context

Add your entity to the DbContext in `Infrastructure/Data/Context/` and add a configuration class for each entity:

```csharp
// ApplicationDbContext.cs
public DbSet<Product> Products => Set<Product>();

// Add configuration class for your entity
public sealed class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.HasKey(p => p.Id);
        builder.Property(p => p.Name).IsRequired();
        builder.Property(p => p.Price).HasPrecision(18, 2);
    }
}
```

### 10. Implement data services

Add your data operations in `Infrastructure/Data/Services/DataService.cs`:

```csharp
public async Task<Product> AddProduct(Product product)
{
    _context.Add(product);
    await _context.SaveChangesAsync();
    return product;
}

public async Task<IEnumerable<Product>> GetAllProducts()
{
    return await _context.Products.ToListAsync();
}
```

### 11. Create API endpoints

Define your endpoints in `Application/Endpoints/`:

```csharp
public static class ProductEndpoints
{
    public static IEndpointRouteBuilder MapProductEndpoints(this IEndpointRouteBuilder builder)
    {
        RouteGroupBuilder group = builder.MapGroup("/products").WithTags("Products");

        group.MapPost("/",
            async Task<Results<Ok<ProductResponse>, BadRequest>> (
                AddProductRequest request,
                IDomainService service) =>
            {
                ProductResponse response = await service.AddProduct(request);

                return response is null
                    ? TypedResults.BadRequest()
                    : TypedResults.Ok(response);
            })
            .WithName("AddProduct")
            .WithOpenApi();

        // Add more endpoints as needed

        return builder;
    }
}
```

### 12. Update registrations

If you create additional services, make sure to register them in `Domain/Registrations/DomainRegistrations.cs` and `Infrastructure/Data/Registrations/DataRegistrations.cs`.

### 13. Update Program.cs

Add your endpoint mappings in `Program.cs`:

```csharp
app.MapProductEndpoints();
```

### 14. Run migrations

Create and apply database migrations:

```bash
dotnet ef migrations add InitialCreate
dotnet ef database update
```

### 15. Run the application

```bash
dotnet run
```

## Best Practices

1. **Keep the domain layer independent** - It should not reference Application or Infrastructure
2. **Use records for DTOs** - They provide immutability and value-based equality
3. **Validate early** - Add validation in domain services before processing
4. **Use nullable reference types** - Mark properties as nullable where appropriate
5. **Document interfaces** - Add XML comments to explain contract requirements
6. **Keep endpoints focused** - Each endpoint should do one thing well
7. **Use meaningful names** - Name services and methods according to their purpose
8. **Test each layer independently** - Write unit tests targeting specific layers

## Additional Resources

- [Microsoft Minimal API Documentation](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/minimal-apis)
- [Entity Framework Core Documentation](https://docs.microsoft.com/en-us/ef/core/)
- [Domain-Driven Design](https://martinfowler.com/bliki/DomainDrivenDesign.html)
- [Hexagonal Architecture](https://alistair.cockburn.us/hexagonal-architecture/)

## License

[MIT](LICENSE)
