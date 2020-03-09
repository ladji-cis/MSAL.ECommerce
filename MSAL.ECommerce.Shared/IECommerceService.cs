using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MSAL.ECommerce.Shared
{
    public interface IECommerceService
    {
        Task<ICollection<Product>> GetAllProductsAsync();   
    }
}
