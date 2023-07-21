using System.ComponentModel.DataAnnotations;

namespace EShop.ViewModels
{
	public class LoginViewModel
	{
		[Required]
		[EmailAddress]
        public string UserEmail { get; set; }

		[Required]
		public string Password { get; set; }
    }
}
