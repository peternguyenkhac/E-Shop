using EShop.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NuGet.Protocol;
using System.Security.Claims;

namespace EShop.Controllers
{
    [Authorize]
    public class CartController : Controller
    {
        private readonly CartService _cartService;

        public CartController(CartService cartService)
        {
            _cartService = cartService;
        }

        public async Task<IActionResult> Cart()
        {
            var cart = await _cartService.GetUserCart(User);
            return View(cart);
        }

        [HttpPost]
        public async Task<IActionResult> AddToCart(int productId, int quantity)
        {
            var cart = await _cartService.GetUserCart(User);
            var cartItem = await _cartService.AddToCart(productId, cart.Id ,quantity);
            if(cartItem != null)
            {
                return Ok();
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpPost]
        public async Task<IActionResult> UpdateCartItem(int productId, int quantity)
        {
            var cart = await _cartService.GetUserCart(User);
            var cartItem = await _cartService.UpdateCartItem(productId, cart.Id ,quantity);
            if (cartItem != null)
            {
                return Ok();
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpPost]
        public async Task<IActionResult> RemoveCartItem(int productId)
        {
            var cart = await _cartService.GetUserCart(User);
            await _cartService.RemoveFromCart(productId, cart.Id);
            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> ClearCart()
        {
            var cart = await _cartService.GetUserCart(User);
            await _cartService.ClearCart(cart);
            return Ok();
        }
    }
}
