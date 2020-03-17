using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MSAL.ECommerce.Api.Filters;
using MSAL.ECommerce.Api.Storage;
using MSAL.ECommerce.Shared.Models;

namespace MSAL.ECommerce.Api.Controllers
{
    [Route("api/products")]
    [ApiController]
    [Authorize]
    public class ProductsController : ControllerBase
    {
        private readonly ICatalogService _catalogService;

        public ProductsController(ICatalogService catalogService)
        {
            _catalogService = catalogService;
        }
        // GET: api/Products
        [HttpGet]
        [Route("")]
        public IEnumerable<Product> Get()
        {
            return _catalogService.GetProducts();
        }

        // GET: api/Products/5
        [HttpGet]
        [Route("{id}")]
        public Product Get(int id)
        {
            return _catalogService.GetProductById(id);
        }

        // POST: api/Products
        [HttpPost]
        public void Post([FromBody] Product model)
        {
        }

        // PUT: api/Products/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] Product model)
        {
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
