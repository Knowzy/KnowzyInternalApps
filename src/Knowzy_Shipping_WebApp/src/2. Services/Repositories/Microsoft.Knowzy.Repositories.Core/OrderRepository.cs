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

using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Knowzy.Domain;
using Microsoft.Knowzy.Models;
using Microsoft.Knowzy.Models.ViewModels;
using Micrososft.Knowzy.Repositories.Contracts;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Net.Http;
using System.Net.Http.Headers;

namespace Microsoft.Knowzy.Repositories.Core
{
    public class OrderRepository : IOrderRepository
    {

        private readonly IMapper _mapper;
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly IConfiguration _configuration;
        private HttpClient _orderClient;
        private HttpClient _productClient;
        private JsonSerializerSettings jsonSettings;

        public OrderRepository(IMapper mapper, IHostingEnvironment hostingEnvironment, IConfiguration configuration)
        {
            _mapper = mapper;
            _hostingEnvironment = hostingEnvironment;
            _configuration = configuration;

            _orderClient = new HttpClient();
            _orderClient.BaseAddress = new Uri(_configuration["ORDERSAPI_URL"]); //ENV var passed in via Docker-Compose override or Kubernetes
            _orderClient.DefaultRequestHeaders.Accept.Clear();
            _orderClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            _productClient = new HttpClient();
            _productClient.BaseAddress = new Uri(_configuration["PRODUCTSAPI_URL"]); //ENV var passed in via Docker-Compose override or Kubernetes
            _productClient.DefaultRequestHeaders.Accept.Clear();
            _productClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            jsonSettings = new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                NullValueHandling = NullValueHandling.Ignore
            };
        }

        public void Dispose()
        {
            _orderClient.Dispose();
            _productClient.Dispose();
        }

        public async Task<IEnumerable<ShippingsViewModel>> GetShippings()
        {
            var shippings = JsonConvert.DeserializeObject<IEnumerable<Shipping>>(await _orderClient.GetStringAsync("/api/Shipping"), jsonSettings);
            return _mapper.Map<IEnumerable<ShippingsViewModel>>(shippings);
        }

        public async Task<IEnumerable<ReceivingsViewModel>> GetReceivings()
        {
            var receivings = JsonConvert.DeserializeObject<IEnumerable<Receiving>>(await _orderClient.GetStringAsync("/api/Receiving"), jsonSettings);
            return _mapper.Map<IEnumerable<ReceivingsViewModel>>(receivings);
        }

        public async Task<IEnumerable<Product>> GetProducts()
        {
            var products = JsonConvert.DeserializeObject<IEnumerable<Product>>(await _productClient.GetStringAsync($"/api/Product"), jsonSettings);
            return _mapper.Map<IEnumerable<Product>>(products);
        }

        public async Task<ShippingViewModel> GetShipping(string orderId)
        {
            var shipping = JsonConvert.DeserializeObject<Shipping>(await _orderClient.GetStringAsync($"/api/Shipping/{orderId}"), jsonSettings);

            foreach (var orderLine in shipping.OrderLines)
            {
                var productInOrderLine = await GetProduct(orderLine.ProductId);
                orderLine.Product = productInOrderLine;
            }

            return _mapper.Map<ShippingViewModel>(shipping);
        }

        public async Task<ReceivingViewModel> GetReceiving(string orderId)
        {
            var receiving = JsonConvert.DeserializeObject<Receiving>(await _orderClient.GetStringAsync($"/api/Receiving/{orderId}"), jsonSettings);

            foreach (var orderLine in receiving.OrderLines)
            {
                var productInOrderLine = await GetProduct(orderLine.ProductId);
                orderLine.Product = productInOrderLine;
            }

            return _mapper.Map<ReceivingViewModel>(receiving);
        }

        public async Task<Product> GetProduct(string productId)
        {
            return JsonConvert.DeserializeObject<Product>(await _productClient.GetStringAsync($"/api/Product/{productId}"), jsonSettings);
        }

        public async Task<IEnumerable<PostalCarrier>> GetPostalCarriers()
        {
            var postalcarriers = JsonConvert.DeserializeObject<IEnumerable<PostalCarrier>>(await _orderClient.GetStringAsync("/api/PostalCarrier"), jsonSettings);
            return _mapper.Map<IEnumerable<PostalCarrier>>(postalcarriers);
        }

        public async Task<IEnumerable<Customer>> GetCustomers()
        {
            var customers = JsonConvert.DeserializeObject<IEnumerable<Customer>>(await _orderClient.GetStringAsync("/api/Customer"), jsonSettings);
            return _mapper.Map<IEnumerable<Customer>>(customers);
        }

        public async Task<int> GetShippingCount()
        {
            var shippings = await GetShippings();
            return shippings.Count();
        }

        public async Task<int> GetReceivingCount()
        {
            var receivings = await GetReceivings();
            return receivings.Count();
        }

        public async Task<int> GetProductCount()
        {
            var products = await GetProducts();
            return products.Count();
        }

        public async Task AddShipping(Shipping shipping)
        {
            shipping.Id = OrderRepositoryHelper.GenerateString(10);
            var shippingStr = JsonConvert.SerializeObject(shipping, jsonSettings).ToString();
            HttpResponseMessage res = await _orderClient.PostAsync("/api/Shipping", new StringContent(shippingStr));
            res.EnsureSuccessStatusCode();
        }

        public async Task UpdateShipping(Shipping shipping)
        {
            var stringData = JsonConvert.SerializeObject(shipping, jsonSettings);
            var contentData = new StringContent(stringData, System.Text.Encoding.UTF8, "application/json");
            HttpResponseMessage res = await _orderClient.PutAsync($"/api/Shipping/{shipping.Id}", contentData);
            res.EnsureSuccessStatusCode();
        }

        public async Task AddReceiving(Receiving receiving)
        {
            receiving.Id = OrderRepositoryHelper.GenerateString(10);
            var receivingStr = JsonConvert.SerializeObject(receiving, jsonSettings).ToString();
            HttpResponseMessage res = await _orderClient.PostAsync("/api/Receiving", new StringContent(receivingStr));
            res.EnsureSuccessStatusCode();
        }

        public async Task UpdateReceiving(Receiving receiving)
        {
            var stringData = JsonConvert.SerializeObject(receiving, jsonSettings);
            var contentData = new StringContent(stringData, System.Text.Encoding.UTF8, "application/json");
            HttpResponseMessage res = await _orderClient.PutAsync($"/api/Receiving/{receiving.Id}", contentData);
            res.EnsureSuccessStatusCode();
        }
    }
}
