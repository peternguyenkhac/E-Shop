using EShop.Models;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;

namespace EShop.ViewModels
{
	public class ProductFeedBackList
	{
        public int TotalRate { get; set; }

		public int FeedbackCount { get; set; }

		public string hello = "hello";

		public List<FeedbackViewModel> FeedbackList { get; set; }
		public ProductFeedBackList(List<FeedbackViewModel> items, int totalRate, int count)
		{
			TotalRate = totalRate;
			FeedbackCount = count;
			FeedbackList = items;
		}

		public static async Task<ProductFeedBackList> CreateAsync(IQueryable<Feedback> source)
		{
			var feedback = await source.Select(f => new FeedbackViewModel
			{
				Id = f.Id,
				ProductId = f.ProductId,
				UserId = f.UserId,
				UserName = f.User.Name,
				Comment = f.Comment,
				Rate = f.Rate,
				CreatedDate = f.CreatedDate
			}).ToListAsync();

			int totalRate = (int)feedback.Average(f => f.Rate);

			int count = feedback.Count();

			return new ProductFeedBackList(feedback, totalRate, count);
		}
	}
}
