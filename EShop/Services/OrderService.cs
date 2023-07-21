using EShop.Models;
using EShop.Models.ViewModels;
using EShop.Repositories;
using EShop.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using System.Security.Claims;

namespace EShop.Services
{
	public class OrderService
	{
		private readonly OrderRepository _orderRepository;
		private readonly CartRepository _cartRepository;
		private readonly ProductRepository _productRepository;

		public OrderService(OrderRepository orderRepository, CartRepository cartRepository, ProductRepository productRepository)
		{
			_orderRepository = orderRepository;
			_cartRepository = cartRepository;
			_productRepository = productRepository;
		}

		public async Task<PaginatedList<Order>> GetOrdersByUserId(int userId, int pageIndex = 1, int pageSize = 5)
		{
			var query = await _orderRepository.GetAll();
			query.Where(o => o.UserId == userId);
			return await PaginatedList<Order>.CreateAsync(query, pageIndex, pageSize);
		}

		public async Task<PaginatedList<Order>> GetOrdersByUser(ClaimsPrincipal user, int pageIndex = 1, int pageSize = 3)
		{
			int userId = Int32.Parse(user.FindFirstValue(ClaimTypes.NameIdentifier));
			var query = await _orderRepository.GetAll();
			query
				.Where(o => o.UserId == userId)
				.OrderBy(o => o.CreatedDate);
			return await PaginatedList<Order>.CreateAsync(query, pageIndex, pageSize);
		}

		public async Task<Order> GetOrderById(int id)
		{
			var order = await _orderRepository.GetSingleById(id);
			return order;
		}

		public async Task<PaginatedList<Order>> GetPaginatedList(string? sortOrder = null, int pageIndex = 1, int pageSize = 5, int orderStatus = 0)
		{
			var query = await _orderRepository.GetAll();

			if (orderStatus != 0)
			{
				query = query.Where(a => a.Status == orderStatus);
			}

			switch (sortOrder)
			{
				case "date":
					query = query.OrderBy(o => o.CreatedDate);
					break;
				case "date_desc":
					query = query.OrderByDescending(o => o.CreatedDate);
					break;
				case "total":
					query = query.OrderByDescending(o => o.Total);
					break;
				case "total_desce":
					query = query.OrderBy(o => o.Total);
					break;
				default:
					query = query.OrderByDescending(o => o.CreatedDate);
					break;
			}

			return await PaginatedList<Order>.CreateAsync(query, pageIndex, pageSize);
		}

		public async Task<Order> Add(Order order)
		{
			int total = 0;
			var cart = await _cartRepository.GetSingleByCondition(i => i.UserId == order.UserId);
			foreach (var item in cart.CartItems)
			{
				var newOrderItem = new OrderItem
				{
					ProductId = item.ProductId,
					Quantity = item.Quantity,
					Price = item.Product.Price
				};
				total += (int)(item.Quantity * item.Product.Price);
				order.OrderItems.Add(newOrderItem);
			}
			order.Total = total;
			return await _orderRepository.Add(order);
		}

		public async Task<Order> ChangeStatus(int orderId, int orderStatus)
		{
			var order = await _orderRepository.GetSingleById(orderId);
			if (order.Status == orderStatus)
			{
				return order;
			}
			else if (orderStatus == 3)
			{
				return await ConfirmOrder(order, true);
			}
			else if (order.Status == 3)
			{
				return await ConfirmOrder(order, false);
			}
			else
			{
				order.Status = orderStatus;
				return await Update(order);
			}
			
		}

		public async Task<Order> ConfirmOrder(Order order, bool isConfirmed)
		{
			var products = new List<Product>();
			foreach (var item in order.OrderItems)
			{
				var product = await _productRepository.GetSingleById(item.ProductId);
				if (isConfirmed)
				{
					product.Quantity -= item.Quantity;
				}
				else
				{
					product.Quantity += item.Quantity;
				}
				if (product.Quantity < 0)
				{
					return null;
				}
				products.Add(product);
			}
			_productRepository.UpdateRange(products);
			order.Status = 3;
			return await _orderRepository.Update(order);
		}


		public async Task<Order> Update(Order order)
		{
			await _orderRepository.Update(order);
			return order;
		}
	}
}
