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
