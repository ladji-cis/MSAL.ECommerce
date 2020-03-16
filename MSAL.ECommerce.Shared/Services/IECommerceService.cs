using MSAL.ECommerce.Shared.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MSAL.ECommerce.Shared.Services
{
    public interface IECommerceService
    {
        Task<ICollection<Product>> GetAllProductsAsync(string accessToken);   
    }
}
