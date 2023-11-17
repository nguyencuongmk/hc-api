﻿using HC.Foundation.Core.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using System;
using System.Linq.Expressions;
using static HC.Foundation.Core.Constants.Constants;

namespace HC.Foundation.Data.Base
{
    public class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : class, IBaseEntity
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
            var lastEntity = _db.OrderByDescending(lt => lt.Id).FirstOrDefault();
            if (lastEntity != null)
            {
                entity.Id = lastEntity.Id + 1;
                await _db.AddAsync(entity);
            }
            else
            {
                entity.Id = 1;
                await _db.AddAsync(entity);
            }

            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<int> AddRangeAsync(List<TEntity> entities)
        {
            _db.AddRange(entities);
            var i = await _context.SaveChangesAsync();
            return i;
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

        public async Task<bool> IsExist(Expression<Func<TEntity, bool>> expression)
        {
            IQueryable<TEntity> query = _db;
            return await query.AnyAsync(expression);
        }

        public async Task UpdateAsync(TEntity entity)
        {
            _db.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<int> UpdateRangeAsync(List<TEntity> entities)
        {
            _db.UpdateRange(entities);
            var i = await _context.SaveChangesAsync();
            return i;
        }
    }
}