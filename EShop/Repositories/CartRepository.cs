using EShop.Data;
using EShop.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace EShop.Repositories
{
    public class CartRepository : RepositoryBase<Cart>
    {
        public CartRepository(EShopDBContext context) : base(context)
        {
        }

        public async Task<Cart> GetSingleById(int id)
        {
            return await _context.Set<Cart>()
                .Include(c => c.CartItems)
                .ThenInclude(i => i.Product)
                .FirstAsync(p => p.Id == id);
        }

        public async Task<Cart> GetSingleByCondition(Expression<Func<Cart, bool>> expression)
        {
            return await _context.Set<Cart>()
                .Include(c => c.CartItems)
                .ThenInclude(i => i.Product)
                .SingleOrDefaultAsync(expression);
        }
    }
}
