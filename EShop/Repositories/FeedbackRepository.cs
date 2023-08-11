using EShop.Data;
using EShop.Models;

namespace EShop.Repositories
{
	public class FeedbackRepository : RepositoryBase<Feedback>
	{
		public FeedbackRepository(EShopDBContext context) : base(context)
		{
		}
	}
}
