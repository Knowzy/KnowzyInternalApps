using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.Knowzy.Domain;

namespace Microsoft.Knowzy.ProductsAPI.Data
{
    public interface IProductsStore : IDisposable
    {
        Task<bool> Connected();
        IEnumerable<Product> GetProducts();
        Product GetProduct(string productId);
        Task<Product> UpsertProductAsync(Product product);
        Task DeleteProductAsync(string productId);
    }
}