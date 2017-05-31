using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Azure.Documents.Client;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Azure.Documents;
using Microsoft.Knowzy.Domain;

namespace Microsoft.Knowzy.CustomersAPI.Data
{
    public class CustomersStore : ICustomersStore
    {
        private readonly DocumentClient _client;
        private Uri _customersLink;
        private FeedOptions _options = new FeedOptions();

        public CustomersStore(IConfiguration config)
        {
            var EndpointUri = config["COSMOSDB_ENDPOINT"];
            var PrimaryKey = config["COSMOSDB_KEY"];
            _client = new DocumentClient(new Uri(EndpointUri), PrimaryKey);
            //Make sure the below values match your set up
            _customersLink = UriFactory.CreateDocumentCollectionUri("knowzydb", "customers");
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

        public IEnumerable<Customer> GetCustomers()
        {
            return _client.CreateDocumentQuery<Customer>(
                _customersLink,
                "SELECT * FROM customers c",
                _options).ToList();
        }

        public Customer GetCustomer(string customerId)
        {
            return _client.CreateDocumentQuery<Customer>(
                _customersLink,
                new SqlQuerySpec
                {
                    QueryText = "SELECT TOP 1 * FROM customers c WHERE (c.id = @customerid)",
                    Parameters = new SqlParameterCollection()
                    {
                     new SqlParameter("@customerid", customerId)
                    }
                },
                _options).ToList().FirstOrDefault();
        }

        public async Task<Customer> UpsertCustomerAsync(Customer customer)
        {
            Document doc = await _client.UpsertDocumentAsync(_customersLink.ToString(), customer);
            Customer response = (dynamic)doc;
            return response;
        }

        public async Task DeleteCustomerAsync(string customerId)
        {
            await _client.DeleteDocumentAsync(UriFactory.CreateDocumentUri("knowzydb", "customers", customerId));
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