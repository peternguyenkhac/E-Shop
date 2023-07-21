using EShop.Data;
using EShop.Models;
using Microsoft.EntityFrameworkCore;

namespace EShop.Repositories
{
    public class OrderRepository : RepositoryBase<Order>
    {
        public OrderRepository(EShopDBContext context) : base(context)
        {
        }

        public async Task<Order> GetSingleById(int id)
        {
            return await _context.Set<Order>()
                .Include(o => o.OrderItems)
                .ThenInclude(i => i.Product)
                .FirstAsync(o => o.Id == id);
        }
    }
}
