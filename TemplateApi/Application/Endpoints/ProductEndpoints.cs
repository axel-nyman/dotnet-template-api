using Microsoft.AspNetCore.Http.HttpResults;

using TemplateApi.Domain.Abstract;
using TemplateApi.Domain.Contracts;

namespace TemplateApi.Application.Endpoints
{
    public static class ProductEndpoints
    {
        public static IEndpointRouteBuilder MapProductEndpoints(this IEndpointRouteBuilder builder)
        {
            RouteGroupBuilder group = builder.MapGroup("/products").WithTags("Products");

            group.MapPost("/addproduct",
                async Task<Results<Ok<ProductResponse>, BadRequest>> (AddProductRequest request, IDomainService domainService) =>
                {
                    ProductResponse response = await domainService.AddProduct(request);
                    return response is null
                        ? TypedResults.BadRequest()
                        : TypedResults.Ok(response);
                })
                .WithName("AddProduct")
                .WithOpenApi();

            group.MapGet("/getproducts",
                async Task<Results<Ok<IEnumerable<ProductResponse>>, NotFound>> (IDomainService domainService) =>
                {
                    IEnumerable<ProductResponse> response = await domainService.GetProducts();
                    return response is null
                        ? TypedResults.NotFound()
                        : TypedResults.Ok(response);
                })
                .WithName("GetProducts")
                .WithOpenApi();

            return builder;
        }
    }
}