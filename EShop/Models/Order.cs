using System;
using System.Collections.Generic;

namespace EShop.Models
{
    public partial class Order
    {
        public Order()
        {
            Feedbacks = new HashSet<Feedback>();
            OrderItems = new HashSet<OrderItem>();
        }

        public int Id { get; set; }
        public int? UserId { get; set; }
        public string? Address { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public int? Total { get; set; }
        public int? Status { get; set; }
        public DateTime CreatedDate { get; set; }

        public virtual User? User { get; set; }
        public virtual ICollection<Feedback> Feedbacks { get; set; }
        public virtual ICollection<OrderItem> OrderItems { get; set; }
    }
}
