using EShop.Models;
using EShop.Services;
using Microsoft.AspNetCore.Mvc;

namespace EShop.Areas.Admin.Controllers
{
	public class CategoryController : Controller
	{
		private readonly CategoryService _categoryService;

		public CategoryController(CategoryService categoryService)
		{
			_categoryService = categoryService;
		}

		public async Task<IActionResult> Index(int pageIndex)
		{
			var categoriesPaginatedList = await _categoryService.GetPaginatedList(pageIndex: pageIndex);
			return View(categoriesPaginatedList);
		}

		[HttpGet]
		public async Task<IActionResult> Create()
		{
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> Create(Category category)
		{
			await _categoryService.Add(category);
			return RedirectToAction("Index");
		}

		[HttpGet]
		public async Task<IActionResult> Edit(int categoryId)
		{
			var category = _categoryService.GetSingleById(categoryId);
			return View(category);
		}

		[HttpPost]
		public async Task<IActionResult> Edit(Category category)
		{
			await _categoryService.Update(category);
			return RedirectToAction("Index");
		}

		[HttpGet]
		public async Task<IActionResult> Delete(int categoryId)
		{
			await _categoryService.Delete(categoryId);
			return RedirectToAction("Index");
		}
	}
}
