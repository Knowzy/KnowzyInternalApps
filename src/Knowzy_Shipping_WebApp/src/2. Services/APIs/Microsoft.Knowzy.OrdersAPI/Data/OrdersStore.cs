using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Azure.Documents.Client;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Azure.Documents;
using Microsoft.Knowzy.Domain;

namespace Microsoft.Knowzy.OrdersAPI.Data
{
    public class OrdersStore : IOrdersStore
    {
        private readonly DocumentClient _client;
        private Uri _ordersLink;
        private FeedOptions _options = new FeedOptions();

        public OrdersStore(IConfiguration config)
        {
            var EndpointUri = config["COSMOSDB_ENDPOINT"];
            var PrimaryKey = config["COSMOSDB_KEY"];
            _client = new DocumentClient(new Uri(EndpointUri), PrimaryKey);
            //Make sure the below values match your set up
            _ordersLink = UriFactory.CreateDocumentCollectionUri("knowzydb", "orders");
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

        public IEnumerable<Shipping> GetShippings()
        {
            return _client.CreateDocumentQuery<Shipping>(
                _ordersLink,
                "SELECT * FROM orders o WHERE o.type='shipping'",
                _options).ToList();
        }

        public Shipping GetShipping(string orderId)
        {
            return _client.CreateDocumentQuery<Shipping>(
                _ordersLink,
                new SqlQuerySpec
                {
                    QueryText = "SELECT TOP 1 * FROM orders o WHERE (o.id = @orderid)",
                    Parameters = new SqlParameterCollection()
                    {
                     new SqlParameter("@orderid", orderId)
                    }
                },
                _options).ToList().FirstOrDefault();
        }

        public IEnumerable<Receiving> GetReceivings()
        {
            return _client.CreateDocumentQuery<Receiving>(
                _ordersLink,
                "SELECT * FROM orders o WHERE o.type='receiving'",
                _options).ToList();
        }

        public Receiving GetReceiving(string orderId)
        {
            return _client.CreateDocumentQuery<Receiving>(
                _ordersLink,
                new SqlQuerySpec
                {
                    QueryText = "SELECT TOP 1 * FROM orders o WHERE (o.id = @orderid)",
                    Parameters = new SqlParameterCollection()
                    {
                     new SqlParameter("@orderid", orderId)
                    }
                },
                _options).ToList().FirstOrDefault();
        }
        
        public async Task UpsertAsync(Order order)
        {
            await _client.UpsertDocumentAsync(_ordersLink.ToString(), order);
        }

        public async Task DeleteOrderAsync(string orderId)
        {
            await _client.DeleteDocumentAsync(UriFactory.CreateDocumentUri("knowzydb", "orders", orderId));
        }

        public IEnumerable<PostalCarrier> GetPostalCarriers()
        {
            return _client.CreateDocumentQuery<PostalCarrier>(
                    _ordersLink,
                    "SELECT o.postalCarrier.id, o.postalCarrier.name FROM orders o",
                    _options).ToList().GroupBy(x => x.Name).Select(x => x.First());
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