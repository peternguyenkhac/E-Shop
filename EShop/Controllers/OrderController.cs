using EShop.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EShop.Controllers
{
    [Authorize]
    public class OrderController : Controller
    {
        private readonly OrderService _orderService;
        private readonly UserService _userService;

        public OrderController(OrderService orderService, UserService userService)
        {
            _orderService = orderService;
            _userService = userService;
        }

        public async Task<IActionResult> Order(int pageIndex = 1)
        {
            var order = await _orderService.GetOrdersByUser(User, pageIndex: pageIndex);
            return View(order);
        }

        public async Task<IActionResult> Details(int orderId)
        {
            var order = await _orderService.GetOrderById(orderId);
            var userId = await _userService.GetUserId(User);
            if(order.UserId != userId)
            {
                return RedirectToAction("Order");
            }
            return View(order);
        }

    }
}
