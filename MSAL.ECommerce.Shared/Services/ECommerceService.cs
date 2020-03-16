using MSAL.ECommerce.Shared.Models;
using MSAL.ECommerce.Shared.Exceptions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace MSAL.ECommerce.Shared.Services
{
    public class ECommerceService : IECommerceService
    {
        private readonly HttpClient _httpClient;

        public ECommerceService(HttpClient httpClient)
        {
            _httpClient = httpClient;            
        }

        public async Task<ICollection<Product>> GetAllProductsAsync(string accessToken)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);

            var response = await _httpClient.GetAsync("api/products");

            if (response.IsSuccessStatusCode)
            {
                var strContent = await response.Content.ReadAsStringAsync();
                var products = JsonConvert.DeserializeObject<IEnumerable<Product>>(strContent);
                return products.ToList();
            }

            throw new ApiCallException((int)response.StatusCode, response.ReasonPhrase);
        }
    }
}
