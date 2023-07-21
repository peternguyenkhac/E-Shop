using EShop.Data;
using EShop.Models;

namespace EShop.Repositories
{
    public class UserRepository : RepositoryBase<User>
    {
        public UserRepository(EShopDBContext context) : base(context)
        {
        }
    }
}
