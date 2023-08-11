namespace EShop.ViewModels
{
	public class FeedbackViewModel
	{
		public int Id { get; set; }
		public int UserId { get; set; }
		public string UserName { get; set; }
		public int ProductId { get; set; }
		public int OrderId { get; set; }
		public string Comment { get; set; } = null!;
		public int Rate { get; set; }
		public DateTime? CreatedDate { get; set; }
	}
}
