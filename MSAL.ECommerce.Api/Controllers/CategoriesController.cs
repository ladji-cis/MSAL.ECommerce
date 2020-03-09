using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MSAL.ECommerce.Api.Storage;
using MSAL.ECommerce.Shared;

namespace MSAL.ECommerce.Api.Controllers
{
    [Route("api/categories")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ICatalogService _catalogService;

        public CategoriesController(ICatalogService catalogService)
        {
            _catalogService = catalogService;
        }

        // GET: api/Categories
        [HttpGet]
        [Route("")]
        public IEnumerable<Category> Get()
        {
            return _catalogService.GetCategories();
        }

        // GET: api/Categories/5
        [HttpGet]
        [Route("{id}")]
        public Category Get(int id)
        {
            return _catalogService.GetCategoryById(id);
        }

        // POST: api/Categories
        [HttpPost]
        public void Post([FromBody] Category model)
        {
        }

        // PUT: api/Categories/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] Category model)
        {
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
