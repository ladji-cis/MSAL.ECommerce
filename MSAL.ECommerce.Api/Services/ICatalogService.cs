using MSAL.ECommerce.Shared.Models;
using System.Collections.Generic;

namespace MSAL.ECommerce.Api.Storage
{
    public interface ICatalogService
    {
        IEnumerable<Category> GetCategories();
        Category GetCategoryById(int id);

        IEnumerable<Product> GetProducts();
        Product GetProductById(int id); 
    }
}
