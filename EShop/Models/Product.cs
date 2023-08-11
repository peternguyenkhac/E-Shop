using System;
using System.Collections.Generic;

namespace EShop.Models
{
    public partial class Product
    {
        public Product()
        {
            CartItems = new HashSet<CartItem>();
            Feedbacks = new HashSet<Feedback>();
            OrderItems = new HashSet<OrderItem>();
        }

        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? Image { get; set; }
        public int? CategoryId { get; set; }
        public int? Price { get; set; }
        public int? Quantity { get; set; }
        public bool? Status { get; set; }
        public DateTime? CreatedDate { get; set; }

        public virtual Category? Category { get; set; }
        public virtual ICollection<CartItem> CartItems { get; set; }
        public virtual ICollection<Feedback> Feedbacks { get; set; }
        public virtual ICollection<OrderItem> OrderItems { get; set; }
    }
}
