namespace TemplateApi.Domain.Contracts;

public sealed record AddProductRequest(
    string Name,
    string? Description,
    decimal Price
);
public sealed record ProductResponse(
    int Id,
    string Name,
    string? Description,
    decimal Price
);