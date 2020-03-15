using MSAL.ECommerce.ClientDesk.Exceptions;
using MSAL.ECommerce.Shared;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace MSAL.ECommerce.ClientDesk.Services
{
    public class ECommerceService : IECommerceService
    {
        private readonly HttpClient _httpClient;

        public ECommerceService(HttpClient httpClient)
        {
            _httpClient = httpClient;            
        }

        public async Task<ICollection<Product>> GetAllProductsAsync()
        {
            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", App.AuthenticationResult?.AccessToken);

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
