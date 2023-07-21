using EShop.Services;
using Microsoft.AspNetCore.Mvc;
using SlugGenerator;

namespace EShop.Controllers
{
    public class ProductController : Controller
    {
        private readonly ProductService _productService;

        public ProductController(ProductService productService)
        {
            _productService = productService;
        }

        public async Task<IActionResult> Details(int id, string slug)
        {
            var product = await _productService.GetSingleById(id);
            string productName = product.Name.GenerateSlug();
/*            if (!slug.Equals(productName))
            {
                return RedirectToRoute(new { id = id, slug = slug });
            }*/
            ViewBag.Category = product.Category;
            return View(product);
        }
    }
}
