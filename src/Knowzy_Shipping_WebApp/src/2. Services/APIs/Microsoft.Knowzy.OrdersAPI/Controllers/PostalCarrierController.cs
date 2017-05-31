using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Knowzy.OrdersAPI.Data;

namespace Microsoft.Knowzy.OrdersAPI.Controllers
{
    [Route("api/[controller]")]
    public class PostalCarrierController : Controller
    {
        private IOrdersStore _ordersStore;
        public PostalCarrierController(IOrdersStore ordersStore)
        {
            _ordersStore = ordersStore;
        }

        // GET api/Shippping
        [HttpGet]
        public IEnumerable<Domain.PostalCarrier> Get()
        {
            return _ordersStore.GetPostalCarriers();
        }
        
    }
}
