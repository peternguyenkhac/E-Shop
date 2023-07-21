using EShop.Services;
using Microsoft.AspNetCore.Mvc;
using NuGet.Protocol;

namespace EShop.Areas.Admin.Controllers
{
	public class OrderController : Controller
	{
		private readonly OrderService _orderService;

		public OrderController(OrderService orderService)
		{
			_orderService = orderService;
		}

		[HttpGet]
		public async Task<IActionResult> Index(int pageIndex = 1, string sortOrder = null, int orderStatus = 0)
		{
			var ordersPaginatedList = await _orderService.GetPaginatedList(pageIndex: pageIndex, sortOrder: sortOrder, orderStatus: orderStatus);
			return View(ordersPaginatedList);
		}

		[HttpGet]
		public async Task<IActionResult> ChangeOrderStatus(int orderId, int orderStatus)
		{
			var order = await _orderService.ChangeStatus(orderId, orderStatus);
			if(order != null)
			{
				return Ok();
			}
			else
			{
				return BadRequest();
			}
		}
	}
}
