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
using Microsoft.Knowzy.ProductsAPI.Data;
using System.Threading.Tasks;
using Microsoft.Knowzy.Domain;

namespace Microsoft.Knowzy.ProductsAPI.Controllers
{
    [Route("api/[controller]")]
    public class ProductController : Controller
    {
        private IProductsStore _productsStore;
        public ProductController(IProductsStore productsStore)
        {
            _productsStore = productsStore;
        }
        // GET api/Product
        [HttpGet]
        public IEnumerable<Product> Get()
        {
            return _productsStore.GetProducts();
        }

        // GET api/Product/5
        [HttpGet("{productId}")]
        public Product GetProduct(string productId)
        {
            return _productsStore.GetProduct(productId);
        }

        //POST
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Product product)
        {
            if (product == null)
            {
                return BadRequest();
            }

            var result = await _productsStore.UpsertProductAsync(product);

            return CreatedAtRoute("Create", new { id = product.Id }, result);
        }

        // PUT
        [HttpPut("{productId}")]
        public async Task<IActionResult> Update(string productId, [FromBody] Product product)
        {
            if (product == null || product.Id != productId)
            {
                return BadRequest();
            }

            var dbProduct = _productsStore.GetProduct(productId);
            if (dbProduct == null)
            {
                return NotFound();
            }

            var result = await _productsStore.UpsertProductAsync(product);
            return new NoContentResult();
        }

        [HttpDelete("{productId}")]
        public async Task<IActionResult> Delete(string productId)
        {
            await _productsStore.DeleteProductAsync(productId);
            return new NoContentResult();
        }
    }
}
