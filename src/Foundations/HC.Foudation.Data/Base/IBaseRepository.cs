using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace HC.Foundation.Data.Base
{
    public interface IBaseRepository<TEntity> where TEntity : class, IBaseEntity
    {
        Task<TEntity> AddAsync(TEntity entity);

        Task<IList<TEntity>> FindMultiple(
            Expression<Func<TEntity, bool>> expression = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> includes = null
            );

        Task<TEntity> FindSingle(
            Expression<Func<TEntity, bool>> expression = null,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> includes = null
            );

        Task<TEntity> GetByIdAsync(int id);

        Task<TEntity> GetByIdAsync(string id);

        Task<int> AddRangeAsync(List<TEntity> entities);

        Task UpdateAsync(TEntity entity);

        Task DeleteAsync(TEntity entity);

        Task<int> CountAsync(Expression<Func<TEntity, bool>> expression);

        Task<bool> IsExist(Expression<Func<TEntity, bool>> expression);

        Task<TEntity> GetFirstOrDefaultAsync(Expression<Func<TEntity, bool>> expression);

        Task<TEntity> GetFirstOrDefaultAsyncNoTracking(Expression<Func<TEntity, bool>> expression);

        Task DeleteRangeAsync(List<TEntity> entities);

        Task<int> UpdateRangeAsync(List<TEntity> entities);

        Task<List<TEntity>> GetAll();
    }
}