using DDHelpers.Api;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace DDHelpers.EntityFramework
{
    public class BaseRepository<T, DBCONTEXT> 
        where T : BaseEntity 
        where DBCONTEXT : DbContext
    {
        private readonly DBCONTEXT? _context;

        public DBCONTEXT? AppContext => _context;

        public BaseRepository(DBCONTEXT context)
        {
            _context = context;
        }

        public virtual async Task<T> CreateAsync(T entity)
        {
            _context.Add(entity);
            await _context.SaveChangesAsync();

            return entity;
        }

        public virtual async Task<T> UpdateAsync(T entity)
        {
            _context.Update(entity);
            await _context.SaveChangesAsync();

            return entity;
        }

        public virtual async Task<bool> DeleteAsync(T entity)
        {
            _context.Remove(entity);
            var result = await _context.SaveChangesAsync();

            return result != 0;
        }

        public virtual async Task<T?> GetAsync(int id)
        {
            return await _context.Set<T>().Where(e => e.Id == id).FirstOrDefaultAsync();
        }

        public virtual async Task<T?> GetAsync(Expression<Func<T, bool>> whereExpression)
        {
            return await _context.Set<T>().Where(whereExpression).FirstOrDefaultAsync();
        }

        public virtual async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _context.Set<T>().ToListAsync();
        }
    }
}
