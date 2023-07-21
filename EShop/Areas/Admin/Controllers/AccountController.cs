using EShop.Models;
using EShop.Services;
using Microsoft.AspNetCore.Mvc;

namespace EShop.Areas.Admin.Controllers
{
	public class AccountController : Controller
	{
		private readonly UserService _userService;

		public AccountController(UserService userService)
		{
			_userService = userService;
		}

		[HttpGet]
		public async Task<IActionResult> Index(int pageIndex, string? searchString)
		{
			var usersPaginatedList = await _userService.GetPaginatedList(pageIndex: pageIndex, searchString: searchString);
			return View(usersPaginatedList);
		}

		[HttpGet]
		public async Task<IActionResult> Create()
		{
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> Create(User user)
		{
			if(!await _userService.IsEmailExists(user.Email))
			{
				await _userService.Add(user);
			}			
			return RedirectToAction("Index");
		}

		[HttpGet]
		public async Task<IActionResult> Edit(int userId)
		{
			var user = await _userService.GetSingleById(userId);
			return View(user);
		}

		[HttpPost]
		public async Task<IActionResult> Edit(User user)
		{
			if(await _userService.IsEmailExists(user.Email))
			{
				await _userService.Update(user);
			}
			return RedirectToAction("Index");
		}

		[HttpGet]
		public async Task<IActionResult> Delete(int userId)
		{
			await _userService.Delete(userId);
			return RedirectToAction("Index");
		}
	}
}
