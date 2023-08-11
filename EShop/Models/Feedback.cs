using System;
using System.Collections.Generic;

namespace EShop.Models
{
    public partial class Feedback
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int ProductId { get; set; }
        public int OrderId { get; set; }
        public string Comment { get; set; } = null!;
        public int Rate { get; set; }
        public DateTime? CreatedDate { get; set; }

        public virtual Order? Order { get; set; } = null!;
        public virtual Product? Product { get; set; } = null!;
        public virtual User? User { get; set; } = null!;
    }
}
