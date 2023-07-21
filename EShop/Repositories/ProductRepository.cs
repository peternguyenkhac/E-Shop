using EShop.Data;
using EShop.Models;
using Microsoft.EntityFrameworkCore;

namespace EShop.Repositories
{
    public class ProductRepository : RepositoryBase<Product>
    {
        public ProductRepository(EShopDBContext context) : base(context)
        {
        }

        public async Task<Product> GetSingleById(int id)
        {
            return await _context.Set<Product>()
                .Include(p => p.Category)
                .FirstAsync(p => p.Id == id);
        }
    }
}
