using System;
using System.Collections.Generic;

namespace EShop.Models
{
    public partial class Banner
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Image { get; set; }
        public string? Url { get; set; }
        public int? DisplayOrder { get; set; }
        public int? Position { get; set; }
    }
}
