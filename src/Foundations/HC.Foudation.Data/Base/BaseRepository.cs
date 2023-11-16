using HC.Foundation.Core.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Linq.Expressions;

namespace HC.Foundation.Data.Base
{
    public class BaseRepository<TEntity, T> : IBaseRepository<TEntity> where TEntity : class, IBaseEntity
    {
        protected AppSettings appSettings;
        protected IDistributedCache distributedCache;

        public int PageNumber { get; set; } = 1;

        public int PageSize { get; set; } = 10;

        public int TotalRecords { get; set; }

        public BaseRepository()
        {
            PageNumber = 1;
            PageSize = 10;
        }

        public Task<TEntity> AddAsync(TEntity entity)
        {
            throw new NotImplementedException();
        }

        public Task<int> AddRangeAsync(List<TEntity> entity)
        {
            throw new NotImplementedException();
        }

        public Task<TEntity[]> AddRangeAsync(TEntity[] entity)
        {
            throw new NotImplementedException();
        }

        public Task<int> CountAsync(Expression<Func<TEntity, bool>> expression)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(TEntity entity)
        {
            throw new NotImplementedException();
        }

        public Task DeleteRangeAsync(List<TEntity> entity)
        {
            throw new NotImplementedException();
        }

        public Task<IList<TEntity>> FindMultiple(Expression<Func<TEntity, bool>> expression = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> includes = null)
        {
            throw new NotImplementedException();
        }

        public Task<TEntity> FindSingle(Expression<Func<TEntity, bool>> expression = null, Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> includes = null)
        {
            throw new NotImplementedException();
        }

        public Task<List<TEntity>> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<TEntity> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<TEntity> GetByIdAsync(string id)
        {
            throw new NotImplementedException();
        }

        public Task<TEntity> GetFirstOrDefaultAsync(Expression<Func<TEntity, bool>> expression)
        {
            throw new NotImplementedException();
        }

        public Task<TEntity> GetFirstOrDefaultAsyncNoTracking(Expression<Func<TEntity, bool>> expression)
        {
            throw new NotImplementedException();
        }

        public Task<bool> IsExist(Expression<Func<TEntity, bool>> expression)
        {
            throw new NotImplementedException();
        }

        public Task<IReadOnlyList<TEntity>> ListAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<IReadOnlyList<TEntity>> ListAsync(Expression<Func<TEntity, bool>> expression)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(TEntity entity)
        {
            throw new NotImplementedException();
        }

        public Task<int> UpdateRange(TEntity[] entities)
        {
            throw new NotImplementedException();
        }

        public Task<int> UpdateRangeAsync(List<TEntity> entities)
        {
            throw new NotImplementedException();
        }
    }
}