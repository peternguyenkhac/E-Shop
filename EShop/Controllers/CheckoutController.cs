using EShop.Models;
using EShop.Services;
using EShop.ViewModels;
using Microsoft.AspNetCore.Mvc;
using NuGet.Protocol;
using System.Net;

namespace EShop.Controllers
{
	public class CheckoutController : Controller
	{
		private readonly OrderService _orderService;
		private readonly CartService _cartService;
		private readonly UserService _userService;
		private readonly VnPayService _vnPayService;

		public CheckoutController(OrderService orderService, CartService cartService, UserService userService, VnPayService vnPayService)
		{
			_orderService = orderService;
			_cartService = cartService;
			_userService = userService;
			_vnPayService = vnPayService;
		}

		public async Task<IActionResult> Checkout()
		{
			var user = await _userService.GetUser(User);
			var cart = await _cartService.GetUserCart(User);

			int total = 0;

			foreach (var item in cart.CartItems)
			{
				total += (int)item.Product.Price * (int)item.Quantity;
			}

			if (cart.CartItems.Count == 0)
			{
				return RedirectToAction("Cart", "Cart");
			}

			var checkoutModel = new CheckoutViewModel
			{
				User = user,
				Cart = cart,
				Total = total
			};

			return View(checkoutModel);
		}

		[HttpPost]
		public async Task<IActionResult> Confirm(User user, string paymentMethod)
		{
			var currentUser = await _userService.GetUser(User);
			var orderInfo = new Order
			{
				UserId = currentUser.Id,
				Address = user.Address,
				Email = user.Email,
				Phone = user.Phone,
				CreatedDate = DateTime.Now,
				Status = 1
			};

			var order = await _orderService.Add(orderInfo);

			switch (paymentMethod)
			{
				case "default":
					return RedirectToAction("Complete", new { orderId = order.Id });
				case "vnpay":
					string paymentUrl = _vnPayService.CreatePaymentUrl(order, HttpContext);
					return Redirect(paymentUrl);
				default:
					return RedirectToAction("Complete", new { orderId = order.Id });
			}
		}

		public async Task<IActionResult> VNPayConfirm()
		{
			var vnpay = _vnPayService.PaymentExecute(HttpContext.Request.Query);

			if (vnpay == null)
			{
				return BadRequest();
			}

			long orderId = Convert.ToInt64(vnpay.GetResponseData("vnp_TxnRef"));
			long vnp_Amount = Convert.ToInt64(vnpay.GetResponseData("vnp_Amount")) / 100;
			string vnp_ResponseCode = vnpay.GetResponseData("vnp_ResponseCode");
			string vnp_TransactionStatus = vnpay.GetResponseData("vnp_TransactionStatus");

			var order = await _orderService.GetOrderById((int)orderId);
			if (order != null)
			{
				if (order.Total == vnp_Amount && order.Status == 1 && vnp_ResponseCode == "00" && vnp_TransactionStatus == "00")
				{
					order.Status = 2;
					await _orderService.Update(order);
					
					return RedirectToAction("Complete", new { orderId = order.Id });
				}
			}

			return BadRequest();

		}

		public async Task<IActionResult> Complete(int orderId)
		{
			var cart = await _cartService.GetUserCart(User);
			await _cartService.ClearCart(cart);
			var order = await _orderService.GetOrderById(orderId);
			return View(order);
		}

	}
}
