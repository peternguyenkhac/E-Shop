using EShop.Models;
using EShop.Models.ViewModels;
using EShop.Repositories;
using EShop.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace EShop.Services
{
    public class ProductService
    {
        private readonly ProductRepository _productRepository;
        public ProductService(ProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<Product> GetSingleById(int id, bool filterByStatus = true)
        {
            var product = await _productRepository.GetSingleById(id);
            if (product.Status == false && filterByStatus == true)
            {
                return null;
            }
            return product;
        }

        public async Task<List<Product>> GetNewProducts(int limit = 8)
        {
            var query = await _productRepository.GetAll();
            var products = await query
                .OrderByDescending(p => p.CreatedDate)
                .Where(p => p.Status == true)
                .Take(limit)
                .ToListAsync();
            return products;
        }

        public async Task<PaginatedList<Product>> GetPaginatedList(string? searchString = null, string? sortOrder = null, int categoryId = 0, int pageIndex = 1, int pageSize = 4, bool filterByStatus = true)
        {
            var query = await _productRepository.GetAll();
            query = query.Include(p => p.Category);

            if(filterByStatus == true)
            {
                query = query.Where(p => p.Status == filterByStatus);
            }

            if (categoryId != 0)
            {
                query = query.Where(a => a.CategoryId == categoryId);
            }

            if (!string.IsNullOrEmpty(searchString))
            {
                query = query.Where(p => p.Name.Contains(searchString));
            }

            switch (sortOrder)
            {
                case "name":
                    query = query.OrderBy(p => p.Name);
                    break;
                case "name_desc":
                    query = query.OrderByDescending(p => p.Name);
                    break;
                case "date":
                    query = query.OrderBy(p => p.CreatedDate);
                    break;
                case "date_desc":
                    query = query.OrderByDescending(p => p.CreatedDate);
                    break;
                case "price":
                    query = query.OrderBy(p => p.Price);
                    break;
                case "price_desc":
                    query = query.OrderByDescending(p => p.Price);
                    break;
                default:
                    query = query.OrderByDescending(p => p.CreatedDate);
                    break;
            }

            return await PaginatedList<Product>.CreateAsync(query, pageIndex, pageSize);
        }

        public async Task<Product> Add(Product product)
        {
            return await _productRepository.Add(product);
        }

        public async Task<Product> Update(Product product)
        {
            return await _productRepository.Update(product);
        }

        public async Task Delete(int id)
        {
            await _productRepository.Delete(id);
        }
    }
}
