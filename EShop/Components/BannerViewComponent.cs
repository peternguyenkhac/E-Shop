using EShop.Models.ViewModels;
using EShop.Services;
using Microsoft.AspNetCore.Mvc;

namespace EShop.Components
{
    public class BannerViewComponent : ViewComponent
    {
		private readonly BannerService _bannerService;

		public BannerViewComponent(BannerService bannerService)
		{
			_bannerService = bannerService;
		}

		public async Task<IViewComponentResult> InvokeAsync()
		{
			var banners = await _bannerService.GetBannersByPosition(2, 3);
			return View(banners);
		}
	}
}
