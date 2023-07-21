using System;
using System.Collections.Generic;

namespace EShop.Models
{
    public partial class Category
    {
        public Category()
        {
            Products = new HashSet<Product>();
        }

        public int Id { get; set; }
        public string? Name { get; set; }
        public int? DisplayOrder { get; set; }

        public virtual ICollection<Product> Products { get; set; }
    }
}
