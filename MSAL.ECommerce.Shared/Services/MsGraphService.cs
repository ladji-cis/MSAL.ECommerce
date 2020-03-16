using MSAL.ECommerce.Shared;
using MSAL.ECommerce.Shared.Exceptions;
using MSAL.ECommerce.Shared.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace MSAL.ECommerce.Shared.Services
{
    public class MsGraphService : IMsGraphService
    {
        private readonly HttpClient _httpClient;

        public MsGraphService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<UserInfo> GetUserInfoAsync(string accessToken)
        {
            //_httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", App.AuthenticationResult?.AccessToken);

            var request = new System.Net.Http.HttpRequestMessage(System.Net.Http.HttpMethod.Get, "users");

            //Add the token in Authorization header
            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);
            var response = await _httpClient.SendAsync(request);
            var content = await response.Content.ReadAsStringAsync();
            var products = JsonConvert.DeserializeObject<UserInfo>(content);
            return null;

            //var response = await _httpClient.GetAsync("users");

            //if (response.IsSuccessStatusCode)
            //{
            //    var strContent = await response.Content.ReadAsStringAsync();
            //    var products = JsonConvert.DeserializeObject<UserInfo>(strContent);
            //    return products.ToList();
            //}

            throw new ApiCallException((int)response.StatusCode, response.ReasonPhrase);
        }
    }
}
