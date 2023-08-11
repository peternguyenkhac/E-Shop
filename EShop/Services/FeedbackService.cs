using EShop.Models.ViewModels;
using EShop.Models;
using EShop.Repositories;
using Microsoft.EntityFrameworkCore;
using EShop.ViewModels;

namespace EShop.Services
{
	public class FeedbackService
	{
		private readonly FeedbackRepository _feedbackRepository;
		private readonly OrderRepository _orderRepository;
		public FeedbackService(FeedbackRepository feedbackRepository, OrderRepository orderRepository)
		{
			_feedbackRepository = feedbackRepository;
			_orderRepository = orderRepository;
		}
		public async Task<ProductFeedBackList> GetProductFeedbacks(int productId)
		{
			var query = await _feedbackRepository.GetAll();
			var feedBack = query.Include(f => f.Product).Include(f => f.User).Where(f => f.ProductId == productId);
			return await ProductFeedBackList.CreateAsync(feedBack);
		}

		public async Task<Feedback> GetSingleById(int id)
		{
			var feedback = await _feedbackRepository.GetSingleById(id);
			return feedback;
		}

		public async Task<Feedback?> Add(Feedback feedback)
		{
			var order = await _orderRepository.GetSingleById(feedback.OrderId);

			if(order.UserId != feedback.UserId || !order.OrderItems.Any(i => i.ProductId == feedback.ProductId) || order.Status != 3)
			{
				return null;
			}

			var query = await _feedbackRepository.GetAll();
			bool hasFeedback = query.Any(f => f.ProductId == feedback.ProductId && f.OrderId == feedback.OrderId);

			if(hasFeedback)
			{
				return null;
			}

			return await _feedbackRepository.Add(feedback);
		}

		public async Task<Feedback> Update(Feedback feedback)
		{
			return await _feedbackRepository.Update(feedback);
		}

		public async Task Delete(int id)
		{
			await _feedbackRepository.Delete(id);
		}
	}
}
