using Newtonsoft.Json;
using System;

namespace MSAL.ECommerce.Shared.Models
{
    public class UserInfo
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("displayName")]
        public string DisplayName { get; set; }

        [JsonProperty("mail")]
        public string Email { get; set; }

        [JsonProperty("userPrincipalName")]
        public string UserPrincipalName { get; set; }
    }
}