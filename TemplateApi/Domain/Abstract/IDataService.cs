using System.Collections.Generic;
using System.Threading.Tasks;

using TemplateApi.Domain.Model;

namespace TemplateApi.Domain.Abstract
{
    public interface IDataService
    {
        Task<Product> AddProduct(Product product);
        Task<IEnumerable<Product>> GetProducts();
    }
}