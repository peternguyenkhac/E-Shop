using EShop.Models.ViewModels;
using EShop.Services;
using Microsoft.AspNetCore.Mvc;

namespace EShop.Components
{
    public class SliderViewComponent : ViewComponent
    {
		private readonly CategoryService _categoryService;
		private readonly BannerService _bannerService;

		public SliderViewComponent(CategoryService categoryService, BannerService bannerService)
		{
			_categoryService = categoryService;
			_bannerService = bannerService;
		}

		public async Task<IViewComponentResult> InvokeAsync()
		{
			var categories = await _categoryService.GetCategories();
			var slider = await _bannerService.GetBannersByPosition(1);
			var mainSection = new SliderViewModel
			{
				categories = categories,
				slider = slider
			};
			return View(mainSection);
		}
	}
}
