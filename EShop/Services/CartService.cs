using EShop.Models;
using EShop.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace EShop.Services
{
    public class CartService
    {
        private readonly CartRepository _cartRepository;
        private readonly CartItemRepository _cartItemRepository;

        public CartService(CartRepository cartRepository, CartItemRepository cartItemRepository)
        {
            _cartRepository = cartRepository;
            _cartItemRepository = cartItemRepository;
        }

        public async Task<Cart> GetUserCart(ClaimsPrincipal user)
        {
            int userId = Int32.Parse(user.FindFirstValue(ClaimTypes.NameIdentifier));
            var cart = await _cartRepository.GetSingleByCondition(c => c.UserId == userId);
            if(cart == null)
            {
                cart = await _cartRepository.Add(new Cart
                {
                    UserId = userId
                });
            }
            return cart;
        }

        public async Task<Cart> GetCartByUserId(int userId)
        {
            return await _cartRepository.GetSingleByCondition(c => c.UserId == userId);
        }

        public async Task<CartItem> AddToCart(int productId, int cartId, int quantity)
        {
            quantity = Math.Clamp(quantity, 1, 100);
            var cartItem = await _cartItemRepository.GetSingleByCondition(i => i.ProductId == productId && i.CartId == cartId);
            if(cartItem == null)
            {
                var newCartItem = new CartItem { ProductId = productId, CartId = cartId, Quantity = quantity };
                return await _cartItemRepository.Add(newCartItem);
            }
            else
            {
                cartItem.Quantity += quantity;
                return await _cartItemRepository.Update(cartItem);
            }
        }

        public async Task<CartItem> UpdateCartItem(int productId, int cartId, int quantity)
        {
            quantity = Math.Clamp(quantity, 1, 100);
            var cartItem = await _cartItemRepository.GetSingleByCondition(i => i.ProductId == productId && i.CartId == cartId);
            cartItem.Quantity = quantity;
            return await _cartItemRepository.Update(cartItem);
        }

        public async Task RemoveFromCart(int productId, int cartId)
        {
            await _cartItemRepository.Delete(productId, cartId);
        }

        public async Task ClearCart(Cart cart)
        {
            await _cartItemRepository.DeleteRange(cart.CartItems);
        }
    }
}
