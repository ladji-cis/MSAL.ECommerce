using MSAL.ECommerce.Shared.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MSAL.ECommerce.Shared.Services
{
    public interface IMsGraphService
    {
        Task<UserInfo> GetUserInfoAsync(string accessToken);
    }
}
