﻿using System;
using System.Collections.Generic;

namespace EShop.Models
{
    public partial class OrderItem
    {
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public int? Price { get; set; }
        public int? Quantity { get; set; }

        public virtual Order Order { get; set; } = null!;
        public virtual Product Product { get; set; } = null!;
    }
}
