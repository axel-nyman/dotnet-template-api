using System.Collections.Generic;
using System.Threading.Tasks;

using TemplateApi.Domain.Contracts;

namespace TemplateApi.Domain.Abstract
{
    public interface IDomainService
    {
        Task<ProductResponse> AddProduct(AddProductRequest request);
        Task<IEnumerable<ProductResponse>> GetProducts();
    }
}