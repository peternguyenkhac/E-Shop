using EShop.Services;
using Microsoft.AspNetCore.Mvc;

namespace EShop.Components
{
    public class NavigationViewComponent : ViewComponent
    {
        private readonly CategoryService _categoryService;

        public NavigationViewComponent(CategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var categories = await _categoryService.GetCategories();
            return View(categories);
        }
    }
}
