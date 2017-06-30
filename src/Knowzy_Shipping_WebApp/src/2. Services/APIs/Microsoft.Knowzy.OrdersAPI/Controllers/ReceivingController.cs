﻿// ******************************************************************
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
using Microsoft.Knowzy.Domain;

namespace Microsoft.Knowzy.OrdersAPI.Controllers
{
    [Route("api/[controller]")]
    public class ReceivingController : Controller
    {
        private IOrdersStore _ordersStore;
        public ReceivingController(IOrdersStore ordersStore)
        {
            _ordersStore = ordersStore;
        }
        // GET api/Receiving
        [HttpGet]
        public IEnumerable<Receiving> Get()
        {
            return _ordersStore.GetReceivings();
        }

        // GET api/Receiving/5
        [HttpGet("{orderId}", Name = "GetReceiving")]
        public Receiving GetReceiving(string orderId)
        {
            return _ordersStore.GetReceiving(orderId);
        }

        //POST
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Receiving order)
        {
            if (order == null)
            {
                return BadRequest();
            }

            await _ordersStore.UpsertAsync(order);

            return CreatedAtRoute("GetReceiving", new { orderId = order.Id }, order);
        }

        // PUT
        [HttpPut("{orderId}")]
        public async Task<IActionResult> Update(string orderId, [FromBody] Receiving order)
        {
            if (order == null || order.Id != orderId)
            {
                return BadRequest();
            }

            var dbOrder = _ordersStore.GetReceiving(orderId);
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
