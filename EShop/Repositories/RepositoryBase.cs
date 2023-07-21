using EShop.Data;
using EShop.Models.ViewModels;
using EShop.ViewModels;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace EShop.Repositories
{
    public class RepositoryBase<T> where T : class
    {
        protected readonly EShopDBContext _context;

        public RepositoryBase(EShopDBContext context)
        {
            _context = context;
        }

        public async Task<T> GetSingleById(int id)
        {
            return await _context.Set<T>().FindAsync(id);
        }

        public async Task<T> GetSingleByCondition(Expression<Func<T, bool>> expression)
        {
            return await _context.Set<T>().FirstOrDefaultAsync(expression);
        }

        public async Task<IQueryable<T>> GetAll()
        {
            return _context.Set<T>().AsQueryable();
        }

        public async Task<PaginatedList<T>> GetPaginatedList(int pageIndex = 1, int pageSize = 5)
        {
            var query = _context.Set<T>();
            return await PaginatedList<T>.CreateAsync(query, pageIndex, pageSize);
        }

        public async Task<T> Add(T entity)
        {
            await _context.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<T> Update(T entity)
        {
            _context.Update(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<IEnumerable<T>> UpdateRange(IEnumerable<T> entities)
        {
            _context.UpdateRange(entities);
            await _context.SaveChangesAsync();
            return entities;
        }

        public async Task Delete(int id)
        {
            var entity = await _context.Set<T>().FindAsync(id);
            _context.Remove(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteRange(IEnumerable<T> entities)
        {
            _context.RemoveRange(entities);
            await _context.SaveChangesAsync();
        }

    }
}
