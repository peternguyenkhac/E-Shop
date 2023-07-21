using EShop.Models;
using EShop.Services;
using Microsoft.AspNetCore.Mvc;

namespace EShop.Areas.Admin.Controllers
{
	public class BannerController : Controller
	{
		private readonly BannerService _bannerService;

		public BannerController(BannerService bannerService)
		{
			_bannerService = bannerService;
		}

		public async Task<IActionResult> Index(int pageIndex)
		{
			var bannersPaginatedList = await _bannerService.GetPaginatedList(pageIndex: pageIndex);
			return View(bannersPaginatedList);
		}

		[HttpGet]
		public async Task<IActionResult> Create()
		{
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> Create(Banner banner)
		{
			await _bannerService.Add(banner);
			return RedirectToAction("Index");
		}

		[HttpGet]
		public async Task<IActionResult> Edit(int bannerId)
		{
			var banner = await _bannerService.GetSingleById(bannerId);
			return View(banner);
		}

		[HttpPost]
		public async Task<IActionResult> Edit(Banner banner)
		{
			await _bannerService.Update(banner);
			return RedirectToAction("Index");
		}

		[HttpGet]
		public async Task<IActionResult> Delete(int bannerId)
		{
			await _bannerService.Delete(bannerId);
			return RedirectToAction("Index");
		}
	}
}
