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
using Microsoft.Knowzy.CustomersAPI.Data;
using System.Threading.Tasks;
using Microsoft.Knowzy.Domain;

namespace Microsoft.Knowzy.CustomersAPI.Controllers
{
    [Route("api/[controller]")]
    public class CustomerController : Controller
    {
        private ICustomersStore _customersStore;
        public CustomerController(ICustomersStore customersStore)
        {
            _customersStore = customersStore;
        }
        // GET api/Customer
        [HttpGet]
        public IEnumerable<Customer> Get()
        {
            return _customersStore.GetCustomers();
        }

        // GET api/Customer/5
        [HttpGet("{customerId}")]
        public Customer GetCustomer(string customerId)
        {
            return _customersStore.GetCustomer(customerId);
        }

        //POST
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Customer customer)
        {
            if (customer == null)
            {
                return BadRequest();
            }

            var result = await _customersStore.UpsertCustomerAsync(customer);

            return CreatedAtRoute("Create", new { id = customer.Id }, result);
        }

        // PUT
        [HttpPut("{customerId}")]
        public async Task<IActionResult> Update(string customerId, [FromBody] Customer customer)
        {
            if (customer == null || customer.Id != customerId)
            {
                return BadRequest();
            }

            var dbCustomer = _customersStore.GetCustomer(customerId);
            if (dbCustomer == null)
            {
                return NotFound();
            }

            var result = await _customersStore.UpsertCustomerAsync(customer);
            return new NoContentResult();
        }

        [HttpDelete("{customerId}")]
        public async Task<IActionResult> Delete(string customerId)
        {
            await _customersStore.DeleteCustomerAsync(customerId);
            return new NoContentResult();
        }
    }
}
