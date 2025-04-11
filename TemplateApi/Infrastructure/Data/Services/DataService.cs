using System.Collections.Generic;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;

using TemplateApi.Domain.Abstract;
using TemplateApi.Domain.Model;
using TemplateApi.Infrastructure.Data.Context;

namespace TemplateApi.Infrastructure.Data.Services
{
    public class DataService(ApplicationDbContext context)
        : IDataService
    {
        public async Task<Product> AddProduct(Product product)
        {
            _ = context.Add(product);
            _ = await context.SaveChangesAsync();
            return product;
        }

        public async Task<IEnumerable<Product>> GetProducts()
        {
            return await context.Products.ToListAsync();
        }
    }
}