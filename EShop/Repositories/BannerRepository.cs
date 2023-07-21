using EShop.Data;
using EShop.Models;

namespace EShop.Repositories
{
    public class BannerRepository : RepositoryBase<Banner>
    {
        public BannerRepository(EShopDBContext context) : base(context)
        {
        }
    }
}
