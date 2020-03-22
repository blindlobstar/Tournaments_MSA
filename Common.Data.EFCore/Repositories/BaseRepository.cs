﻿using Common.Core.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Common.Data.EFCore.Repositories
{
    public abstract class BaseRepository<TEntity, TKey> : IEFRepository<TEntity, TKey> where TEntity : class, IEntity<TKey>, new()
    {
        private readonly DbContext _dataContext;
        protected DbSet<TEntity> DbSet => _dataContext.Set<TEntity>();

        protected BaseRepository(DbContext dataContext)
        {
            _dataContext = dataContext;
        }

        public TEntity Add(TEntity entity)
        {
            DbSet.Add(entity);
            return entity;
        }

        public virtual void Delete(TEntity entity)
        {
            DbSet.Attach(entity);
            _dataContext.Entry(entity).State = EntityState.Deleted;
        }

        public virtual async Task<TEntity> Get(TKey id) =>
            await DbSet.FindAsync(id);

        public virtual async Task<TEntity> Get(TKey id, string include) =>
            await DbSet.Include(include).FirstOrDefaultAsync(x => x.Id.Equals(id));

        public virtual async Task<TEntity> Get(TKey id, IEnumerable<string> includes)
        {
            var query = DbSet.AsQueryable();
            query = includes.Aggregate(query, (current, include) => current.Include(include));
            return await query.SingleOrDefaultAsync(c => c.Id.Equals(id));

        }

        public virtual async Task<List<TEntity>> GetAll() =>
            await DbSet.ToListAsync();

        public virtual async Task<List<TEntity>> GetAll(string include) =>
            await DbSet.Include(include).ToListAsync();

        public virtual async Task<List<TEntity>> GetAll(IEnumerable<string> includes) =>
            await includes.Aggregate(DbSet.AsQueryable(), (cur, path) => 
                cur.Include(path)).ToListAsync();

        public virtual void Update(TEntity entity)
        {
            DbSet.Attach(entity);
            _dataContext.Entry(entity).State = EntityState.Modified;
        }

        public virtual void Delete(TKey id)
        {
            var entity = DbSet.Find(id);
            _dataContext.Entry(entity).State = EntityState.Deleted;
        }

        public virtual void SaveChanges() =>
            _dataContext.SaveChanges();
    }
}