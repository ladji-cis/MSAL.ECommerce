using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MSAL.ECommerce.Shared
{
    public interface IMsGraphService
    {
        Task<UserInfo> GetUserInfoAsync();
    }
}
