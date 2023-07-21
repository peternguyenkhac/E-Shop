using EShop.Models;
using EShop.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace EShop.Areas.Admin.Controllers
{
	public class ProductController : Controller
	{
		private readonly ProductService _productService;
		private readonly CategoryService _categoryService;

		public ProductController(ProductService productService, CategoryService categoryService)
		{
			_productService = productService;
			_categoryService = categoryService;
		}

		public async Task<IActionResult> Index(int pageIndex = 1, int categoryId = 1, string searchString = "", string sortOrder = "date")
		{
			var productsPaginatedList = await _productService.GetPaginatedList(categoryId: categoryId, pageIndex: pageIndex, sortOrder: sortOrder);
			ViewBag.CategoryId = categoryId;
			ViewBag.SortOrder = sortOrder;
			ViewBag.SearchString = searchString;
			ViewBag.Category = productsPaginatedList.First().Category;
			return View(productsPaginatedList);
		}

		[HttpGet]
		public async Task<IActionResult> Create()
		{
			ViewBag.CategorySelectlist = new SelectList(await _categoryService.GetCategories(), "Id", "Name");
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> Create(Product product)
		{
			await _productService.Add(product);
			return RedirectToAction("Index");
		}

		[HttpGet]
		public async Task<IActionResult> Edit(int productId)
		{
			var product = await _productService.GetSingleById(productId);
			ViewBag.CategorySelectlist = new SelectList(await _categoryService.GetCategories(), "Id", "Name", product.CategoryId);
			return View(product);
		}

		[HttpPost]
		public async Task<IActionResult> Edit(Product product)
		{
			await _productService.Update(product);
			return RedirectToAction("Index");
		}

		public async Task<IActionResult> Delete(int productId)
		{
			await _productService.Delete(productId);
			return RedirectToAction("Index");
		}
	}
}
