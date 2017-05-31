// ******************************************************************
// Copyright (c) Microsoft. All rights reserved.
// This code is licensed under the MIT License (MIT).
// THE CODE IS PROVIDED “AS IS”, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
// INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
// IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
// DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
// TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH
// THE CODE OR THE USE OR OTHER DEALINGS IN THE CODE.
// ******************************************************************
using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Azure.Documents.Client;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Azure.Documents;
using Microsoft.Knowzy.Domain;

namespace Microsoft.Knowzy.ProductsAPI.Data
{
    public class ProductsStore : IProductsStore
    {
        private readonly DocumentClient _client;
        private Uri _productsLink;
        private FeedOptions _options = new FeedOptions();

        public ProductsStore(IConfiguration config)
        {
            var EndpointUri = config["COSMOSDB_ENDPOINT"];
            var PrimaryKey = config["COSMOSDB_KEY"];
            _client = new DocumentClient(new Uri(EndpointUri), PrimaryKey);
            //Make sure the below values match your set up
            _productsLink = UriFactory.CreateDocumentCollectionUri("knowzydb", "products");
            _options.EnableCrossPartitionQuery = true;
        }

        public async Task<bool> Connected()
        {
            try
            {
                var db = await _client.GetDatabaseAccountAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public IEnumerable<Product> GetProducts()
        {
            return _client.CreateDocumentQuery<Product>(
                _productsLink,
                "SELECT * FROM products p",
                _options).ToList();
        }

        public Product GetProduct(string productId)
        {
            return _client.CreateDocumentQuery<Product>(
                _productsLink,
                new SqlQuerySpec
                {
                    QueryText = "SELECT TOP 1 * FROM products p WHERE (p.id = @productid)",
                    Parameters = new SqlParameterCollection()
                    {
                     new SqlParameter("@productid", productId)
                    }
                },
                _options).ToList().FirstOrDefault();
        }

        public async Task<Product> UpsertProductAsync(Product product)
        {
            Document doc = await _client.UpsertDocumentAsync(_productsLink.ToString(), product);
            Product response = (dynamic)doc;
            return response;
        }

        public async Task DeleteProductAsync(string productId)
        {
            await _client.DeleteDocumentAsync(UriFactory.CreateDocumentUri("knowzydb", "products", productId));
        }

        private bool disposedValue = false; // To detect redundant calls
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _client.Dispose();
                }
                disposedValue = true;
            }
        }

        void IDisposable.Dispose()
        {
            Dispose(true);
        }
    }
}