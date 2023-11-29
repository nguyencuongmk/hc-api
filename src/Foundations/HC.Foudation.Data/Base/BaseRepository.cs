using HC.Foundation.Common.Constants;
using HC.Foundation.Data.Base.IBase;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.Extensions.Caching.Distributed;
using System.Linq.Expressions;
using static HC.Foundation.Common.Constants.Constants;

namespace HC.Foundation.Data.Base
{
    public class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : class, IBaseEntity, new()
    {
        private readonly BaseDbContext _context;
        private readonly DbSet<TEntity> _db;
        private readonly AppSettings _appSettings;
        private readonly IDistributedCache _distributedCache;

        public int PageNumber { get; set; } = 1;

        public int PageSize { get; set; } = 10;

        public int TotalRecords { get; set; }

        public BaseRepository(BaseDbContext context)
        {
            _context = context;
            _db = _context.Set<TEntity>();
        }

        public async Task<TEntity> AddAsync(TEntity entity)
        {
            try
            {
                _context.ChangeTracker.AutoDetectChangesEnabled = false;
                _db.Add(entity);
                await _context.SaveChangesAsync();
                _context.ChangeTracker.AutoDetectChangesEnabled = true;
                return entity;
            }
            catch
            {
                return null;
            }
        }

        public async Task<int> AddRangeAsync(List<TEntity> entities)
        {
            _db.AddRange(entities);
            return await _context.SaveChangesAsync();
        }

        public async Task<int> CountAsync(Expression<Func<TEntity, bool>> expression)
        {
            return await _db.Where(expression).CountAsync();
        }

        public async Task DeleteAsync(TEntity entity)
        {
            entity.Status = Status.Deleted;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteRangeAsync(List<TEntity> entities)
        {
            entities.ForEach(x => x.Status = Status.Deleted);
            await _context.SaveChangesAsync();
        }

        public async Task<IList<TEntity>> FindMultiple(Expression<Func<TEntity, bool>> expression = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> includes = null)
        {
            IQueryable<TEntity> query = _db;

            if (expression != null)
            {
                query = query.Where(expression);
            }

            if (orderBy != null)
            {
                query = orderBy(query);
            }

            if (includes != null)
            {
                query = includes(query);
            }

            return await query.ToListAsync();
        }

        public async Task<TEntity> FindSingle(Expression<Func<TEntity, bool>> expression = null, Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> includes = null)
        {
            IQueryable<TEntity> query = _db;

            if (includes != null)
            {
                query = includes(query);
            }

            return await query.FirstOrDefaultAsync(expression);
        }

        public async Task<List<TEntity>> GetAll()
        {
            return await _db.ToListAsync();
        }

        public async Task<TEntity> GetByIdAsync(int id)
        {
            return await _db.FindAsync(id);
        }

        public async Task<TEntity> GetByIdAsync(string id)
        {
            return await _db.FindAsync(id);
        }

        public async Task<TEntity> GetFirstOrDefaultAsync(Expression<Func<TEntity, bool>> expression)
        {
            return await _db.Where(expression).FirstOrDefaultAsync();
        }

        public async Task<TEntity> GetFirstOrDefaultAsyncNoTracking(Expression<Func<TEntity, bool>> expression)
        {
            return await _db.AsNoTracking().Where(expression).FirstOrDefaultAsync();
        }

        public async Task<bool> IsExists(Expression<Func<TEntity, bool>> expression)
        {
            IQueryable<TEntity> query = _db;
            return await query.AnyAsync(expression);
        }

        public async Task<TEntity> UpdateAsync(TEntity entity)
        {
            try
            {
                _db.Update(entity);
                await _context.SaveChangesAsync();
                return entity;
            }
            catch
            {
                return null;
            }
        }

        public async Task<int> UpdateRangeAsync(List<TEntity> entities)
        {
            _db.UpdateRange(entities);
            return await _context.SaveChangesAsync();
        }
    }
}