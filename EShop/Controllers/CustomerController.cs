using EShop.Services;
using EShop.ViewModels;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using EShop.Models;

namespace EShop.Controllers
{
	public class CustomerController : Controller
	{
		private readonly UserService _userService;

		public CustomerController(UserService userService)
		{
			_userService = userService;
		}

		public IActionResult Login()
		{
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> Login(LoginViewModel model)
		{
			if (!ModelState.IsValid)
			{
                return View();
            }
            var user = await _userService.PasswordSingIn(model, HttpContext);

			if (user == null) 
			{
				ModelState.AddModelError("", "Thông tin đăng nhập không chính xác");
				return View();
			}

			return RedirectToAction("Index", "Home");
		}

		public async Task<IActionResult> Logout()
		{
			await HttpContext.SignOutAsync();
			return RedirectToAction("Index", "Home");
		}

		public IActionResult Register()
		{
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> Register(RegisterViewModel model)
		{
			if (ModelState.IsValid)
			{

				if (await _userService.IsEmailExists(model.Email))
				{
					ModelState.AddModelError("", "Email này đã được sử dụng");
					return View();
				}

				var user = new User
				{
					Name = model.Name,
					Email = model.Email,
					Phone = model.Phone,
					Address	= model.Address,
					Password = model.Password,
					Role = "Customer"
				};

				var newUser = await _userService.Add(user);

				return RedirectToAction("Login", "Customer");
			}
			
			return View();
		}

		public async Task<IActionResult> Profile()
		{
			int userId = Int32.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
			var user = await _userService.GetSingleById(userId);
			return View(user);
		}

		[HttpPost]
		public async Task<IActionResult> EditProfile(User user)
		{
			int userId = Int32.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
			var oldUser = await _userService.GetSingleById(userId);

			oldUser.Name = user.Name;
			oldUser.Email = user.Email;
			oldUser.Address = user.Address;
			oldUser.Phone = user.Phone;

			var updatedUser = await _userService.Update(oldUser);

			await _userService.SingIn(updatedUser, HttpContext);

			return RedirectToAction("Profile", "Customer");
		}

		public IActionResult ChangePassword()
		{
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
		{
			int userId = Int32.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
			var user = await _userService.GetSingleById(userId);

			if(user.Password == model.OldPassword)
			{
				user.Password = model.NewPassword;
                await _userService.Update(user);
            }
			else
			{
				ModelState.AddModelError("", "Mật khẩu cũ không chính xác");
            }

			return View();
		}
	}
}
