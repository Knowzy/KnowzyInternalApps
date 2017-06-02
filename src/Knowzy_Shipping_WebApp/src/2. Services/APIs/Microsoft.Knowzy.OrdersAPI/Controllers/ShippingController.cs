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
using System.Threading.Tasks;

namespace Microsoft.Knowzy.OrdersAPI.Controllers
{
    [Route("api/[controller]")]
    public class ShippingController : Controller
    {
        private IOrdersStore _ordersStore;
        public ShippingController(IOrdersStore ordersStore)
        {
            _ordersStore = ordersStore;
        }
        // GET api/Shippping
        [HttpGet]
        public IEnumerable<Domain.Shipping> Get()
        {
            return _ordersStore.GetShippings();
        }

        // GET api/Shipping/5
        [HttpGet("{orderId}", Name = "GetShipping")]
        public Domain.Shipping GetShipping(string orderId)
        {
            return _ordersStore.GetShipping(orderId);
        }

        //POST
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Domain.Shipping order)
        {
            if (order == null)
            {
                return BadRequest();
            }

            await _ordersStore.UpsertAsync(order);

            return CreatedAtRoute("GetShipping", new { orderId = order.Id }, order);
        }

        // PUT
        [HttpPut("{orderId}")]
        public async Task<IActionResult> Update(string orderId, [FromBody] Domain.Shipping order)
        {
            if (order == null || order.Id != orderId)
            {
                return BadRequest();
            }

            var dbOrder = _ordersStore.GetShipping(orderId);
            if (dbOrder == null)
            {
                return NotFound();
            }

            await _ordersStore.UpsertAsync(order);
            return new NoContentResult();
        }

        [HttpDelete("{orderId}")]
        public async Task<IActionResult> Delete(string orderId)
        {
            await _ordersStore.DeleteOrderAsync(orderId);
            return new NoContentResult();
        }
    }
}
