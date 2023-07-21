using EShop.Models;
using EShop.Services;
using Microsoft.AspNetCore.Mvc;

namespace EShop.Controllers
{
    public class CatalogController : Controller
    {
        private readonly ProductService _productService;

        public CatalogController(ProductService productService)
        {
            _productService = productService;
        }

        public async Task<IActionResult> GetAllProducts(int pageIndex = 1, string searchString = "", string sortOrder = "date")
        {
            var productsPaginatedList = await _productService.GetPaginatedList(pageIndex: pageIndex, searchString: searchString, sortOrder: sortOrder);
            ViewBag.SearchString = searchString;
            ViewBag.SortOrder = sortOrder;
            return View(productsPaginatedList);
        }

        public async Task<IActionResult> GetProductsByCategory(int categoryId = 1, int pageIndex = 1, string sortOrder = "date")
        {
            var productsPaginatedList = await _productService.GetPaginatedList(categoryId: categoryId, pageIndex: pageIndex, sortOrder: sortOrder);
            ViewBag.CategoryId = categoryId;
            ViewBag.SortOrder = sortOrder;
			ViewBag.Category = productsPaginatedList.First().Category;
			return View(productsPaginatedList);
        }
    }
}
