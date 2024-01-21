using System.Linq.Expressions;

namespace CopyBag.Models
{
    public interface IBaseRepository<TEntity> where TEntity : class
    {
        Task<bool> Add(TEntity entity);
        Task<bool> Edit(TEntity entity);
        Task<bool> Remove(TEntity entity);
        Task<bool> Exists(Expression<Func<TEntity, bool>> expression);
        Task<TEntity> Get(int id);
        Task<List<TEntity>> GetAll();
    }
}
