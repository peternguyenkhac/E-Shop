using EShop.Models;
using EShop.Services;
using EShop.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NuGet.Protocol;

namespace EShop.ApiControllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class FeedbackController : ControllerBase
	{
		private readonly FeedbackService _feedbackService;
		private readonly UserService _userService;

		public FeedbackController(FeedbackService feedbackService, UserService userService)
		{
			_feedbackService = feedbackService;
			_userService = userService;
		}

		[HttpGet]
		[Route("GetFeedbacks/{productId}")]
		public async Task<IActionResult> GetProductFeedBack(int productId)
		{
			ProductFeedBackList feedback = await _feedbackService.GetProductFeedbacks(productId);
			return Ok(feedback);
		}

		[HttpPost]
		[Authorize]
		[Route("Create")]
		public async Task<IActionResult> Create([Bind("ProductId, OrderId, Comment, Rate")] Feedback newFeedback)
		{
			var userId = await _userService.GetUserId(User);
			newFeedback.UserId = userId;
			var feedback = await _feedbackService.Add(newFeedback);
			if(feedback == null)
			{
				return BadRequest();
			}
			return Ok();
		}

		[HttpDelete]
		[Authorize]
		[Route("Delete/{feedbackId}")]
		public async Task<IActionResult> Delete(int feedbackId)
		{
			var feedback = await _feedbackService.GetSingleById(feedbackId);
			if (feedback == null)
			{
				return BadRequest();
			}
			var userId = await _userService.GetUserId(User);
			if(feedback.UserId != userId)
			{
				return BadRequest();
			}
			await _feedbackService.Delete(feedbackId);
			return Ok();
		}

		[HttpPut]
		[Authorize]
		[Route("Edit")]
		public async Task<IActionResult> Edit(Feedback updateFeedback)
		{
			var userId = await _userService.GetUserId(User);
			var feedback = await _feedbackService.GetSingleById(updateFeedback.Id);
			if(feedback.UserId != userId)
			{
				return BadRequest();
			}

			feedback.Comment = updateFeedback.Comment;
			feedback.Rate = updateFeedback.Rate;

			await _feedbackService.Update(updateFeedback);
			return Ok();
		}
	}
}
