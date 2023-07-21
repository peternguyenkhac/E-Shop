using EShop.Models;
using EShop.Models.ViewModels;
using EShop.Repositories;
using EShop.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;

namespace EShop.Services
{
    public class CategoryService
    {
        private readonly CategoryRepository _categoryRepository;
        public CategoryService(CategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<Category> GetSingleById(int id)
        {
            return await _categoryRepository.GetSingleById(id);
        }

        public async Task<List<Category>> GetCategories()
        {
            var query = await _categoryRepository.GetAll();
            var categories = await query.OrderBy(c => c.DisplayOrder).ToListAsync();
            return categories;
        }

        public async Task<PaginatedList<Category>> GetPaginatedList(string? searchString = null, string? sortOrder = null, int categoryId = 0, int pageIndex = 1, int pageSize = 5, bool filterByStatus = true)
        {
            var query = await _categoryRepository.GetAll();

            if (!string.IsNullOrEmpty(searchString))
            {
                query = query.Where(c => c.Name.Contains(searchString));
            }

            switch (sortOrder)
            {
                case "name":
                    query = query.OrderBy(c => c.Name);
                    break;
                case "name_desc":
                    query = query.OrderByDescending(c => c.Name);
                    break;
                case "display_order":
                    query = query.OrderBy(c => c.DisplayOrder);
                    break;
                case "display_order_desc":
                    query = query.OrderByDescending(c => c.DisplayOrder);
                    break;
                default:
                    query = query.OrderByDescending(c => c.DisplayOrder);
                    break;
            }

            return await PaginatedList<Category>.CreateAsync(query, pageIndex, pageSize);
        }

        public async Task<Category> Add(Category category)
        {
            return await _categoryRepository.Add(category);
        }

        public async Task<Category> Update(Category category)
        {
            return await _categoryRepository.Update(category);
        }

        public async Task Delete(int id)
        {
            _categoryRepository.Delete(id);
        }
    }
}
