namespace TemplateApi.Domain.Services;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using TemplateApi.Domain.Abstract;
using TemplateApi.Domain.Contracts;
using TemplateApi.Domain.Extensions;
using TemplateApi.Domain.Model;


public class DomainService(IDataService dataService) : IDomainService
{
    public async Task<ProductResponse> AddProduct(AddProductRequest request)
    {
        // TODO: Validate request
        Product product = request.ToEntity();
        product = await dataService.AddProduct(product);
        // TODO: Add status codes to response data type
        return product.ToResponse();
    }

    public async Task<IEnumerable<ProductResponse>> GetProducts()
    {
        IEnumerable<Product> products = await dataService.GetProducts();
        return products.Select(p => p.ToResponse());
    }
}