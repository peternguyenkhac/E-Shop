using EShop.Models;
using EShop.Models.ViewModels;
using EShop.Repositories;
using EShop.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace EShop.Services
{
    public class BannerService
    {
        private readonly BannerRepository _bannerRepository;
        
        public BannerService(BannerRepository bannerRepository)
        {
            _bannerRepository = bannerRepository;
        }

        public async Task<List<Banner>> GetBannersByPosition(int position, int limit = 0)
        {
            var query = await _bannerRepository.GetAll();
            var banners = query.Where(b => b.Position == position);
            if(limit != 0)
            {
                banners.Take(limit);
            }
            return await banners.ToListAsync();
        }

        public async Task<PaginatedList<Banner>> GetPaginatedList(string? searchString = null, int pageIndex = 1, int pageSize = 5)
        {
            var query = await _bannerRepository.GetAll();

            if (!string.IsNullOrEmpty(searchString))
            {
                query = query.Where(b => b.Name.Contains(searchString));
            }

            return await PaginatedList<Banner>.CreateAsync(query, pageIndex, pageSize);
        }

        public async Task<Banner> GetSingleById(int bannerId)
        {
            var banner = await _bannerRepository.GetSingleById(bannerId);
            return banner;
        }

        public async Task<Banner> Add(Banner banner)
        {
            return await _bannerRepository.Add(banner);
        }

        public async Task<Banner> Update(Banner banner)
        {
            return await _bannerRepository.Update(banner);
        }

        public async Task Delete(int id)
        {
            await _bannerRepository.Delete(id);
        }
    }
}
