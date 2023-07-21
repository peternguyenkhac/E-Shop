using EShop.Services;
using Microsoft.AspNetCore.Mvc;

namespace EShop.Components
{
    public class NewProductsViewComponent : ViewComponent
    {
        private readonly ProductService _productService;

		public NewProductsViewComponent(ProductService productService)
		{
			_productService = productService;
		}

		public async Task<IViewComponentResult> InvokeAsync()
		{
			var products = await _productService.GetNewProducts();
			return View(products);
		}
	}
}
