using System;
using System.Collections.Generic;

namespace CFMS.ProductService.Core.Entities
{
    public class Category
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        
        public ICollection<Product> Products { get; set; } = new List<Product>();
    }
}
