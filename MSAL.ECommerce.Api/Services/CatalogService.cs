using Microsoft.Extensions.Caching.Memory;
using MSAL.ECommerce.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace MSAL.ECommerce.Api.Storage
{

    public class CatalogService : ICatalogService
    {
        public IEnumerable<Category> GetCategories()
        {
            return Categories();
        }

        public Category GetCategoryById(int id)
        {
            return Categories().First(x => x.Id == id);
        }

        public Product GetProductById(int id)
        {
            return Products().First(x => x.Id == id);
        }

        public IEnumerable<Product> GetProducts()
        {
            return Products();
        }

        private IEnumerable<Category> Categories()
        {
            var categories = new List<Category>
            {
                new Category{Id=1, Name="SmartPhone"},
                new Category{Id=2, Name="LapTop"}
            };

            return categories;
        }

        private IEnumerable<Product> Products()
        {
            var products = new List<Product>
            {
                new Product{Id=1, Name="IPhone X MAX", Description="256 GO - REG", Price=1190m, Category= Categories().First(x=> x.Id == 1)},
                new Product{Id=2, Name="OnePlue 7 PRO", Description="256 GO - Gray", Price=750m, Category= Categories().First(x=> x.Id == 1)},
                new Product{Id=1, Name="Samsung S10", Description="512 GO Blue", Price=899m, Category= Categories().First(x=> x.Id == 1)},
                new Product{Id=1, Name="DELL XPS 15", Description="32GO RAM - SSD 1TO - 4K", Price=2750m, Category= Categories().First(x=> x.Id == 2)},
                new Product{Id=1, Name="MackBook PRO 16", Description="16 GO Blue", Price=3660m, Category= Categories().First(x=> x.Id == 2)},
            };

            return products;
        }
    }
}
