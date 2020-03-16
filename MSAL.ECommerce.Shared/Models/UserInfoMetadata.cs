using Newtonsoft.Json;
using System.Collections.Generic;

namespace MSAL.ECommerce.Shared.Models
{
    public class UserInfoMetadata
    {
        [JsonProperty("value")]
        public IEnumerable<UserInfo> Value { get; set; }
    }
}