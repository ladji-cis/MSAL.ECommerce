using System;

namespace MSAL.ECommerce.Shared.Models
{
    public class Product : Entity
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }
        public Category Category { get; set; }
    }   
}
