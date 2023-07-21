using EShop.Models;

namespace EShop.ViewModels
{
    public class CheckoutViewModel
    {
        public User User { get; set; }

        public string PaymentMethod { get; set; }

        public Cart Cart { get; set; }

        public int Total { get; set; }
    }
}
