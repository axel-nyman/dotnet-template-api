using TemplateApi.Domain.Contracts;
using TemplateApi.Domain.Model;

namespace TemplateApi.Domain.Extensions
{
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
}