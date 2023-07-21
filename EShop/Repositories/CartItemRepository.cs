using EShop.Data;
using EShop.Models;
using Microsoft.EntityFrameworkCore;

namespace EShop.Repositories
{
    public class CartItemRepository : RepositoryBase<CartItem>
    {
        public CartItemRepository(EShopDBContext context) : base(context)
        {
        }

        public async Task Delete(int productId, int cartId)
        {
            var entity = await _context.Set<CartItem>().FirstOrDefaultAsync(i => i.CartId == cartId && i.ProductId == productId); 
            _context.Remove(entity);
            await _context.SaveChangesAsync();
        }
    }
}
