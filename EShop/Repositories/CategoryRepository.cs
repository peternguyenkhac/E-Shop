using EShop.Data;
using EShop.Models;

namespace EShop.Repositories
{
    public class CategoryRepository : RepositoryBase<Category>
    {
        public CategoryRepository(EShopDBContext context) : base(context)
        {
        }
    }
}
