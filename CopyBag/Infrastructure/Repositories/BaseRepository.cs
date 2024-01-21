using CopyBag.Models;
using System.Linq.Expressions;

namespace CopyBag.Infrastructure.Repositories
{
    public class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : class, new()
    {
        DbContext _context;
        public BaseRepository(DbContext dbContext)
        {
            _context = dbContext;
        }

        public async Task<bool> Add(TEntity entity)
        {
            await _context.InitAsync();
            return await _context.Database.InsertAsync(entity) > 0;
        }

        public async Task<bool> Edit(TEntity entity)
        {
            await _context.InitAsync();
            return await _context.Database.UpdateAsync(entity) > 0;
        }

        public async Task<bool> Exists(Expression<Func<TEntity, bool>> expression)
        {
            await _context.InitAsync();

            var result = await _context.Database.Table<TEntity>().FirstOrDefaultAsync(expression);

            if (result is null) return false;
            return true;
        }

        public async Task<TEntity> Get(int id)
        {
            await _context.InitAsync();
            return await _context.Database.GetAsync<TEntity>(id);
        }

        public async Task<List<TEntity>> GetAll()
        {
            await _context.InitAsync();
            return await _context.Database.Table<TEntity>().ToListAsync();
        }

        public async Task<bool> Remove(TEntity entity)
        {
            await _context.InitAsync();
            return await _context.Database.DeleteAsync(entity) > 0;
        }
    }
}
