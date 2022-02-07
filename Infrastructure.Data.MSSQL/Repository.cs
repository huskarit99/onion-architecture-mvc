using Domain.Interfaces;
using Infrastructure.Data.MSSQL;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Infrastructure.Data.MSSQL
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        private readonly TestDBContext _context;
        internal DbSet<TEntity> dbSet;
        public Repository(TestDBContext context)
        {
            _context = context;
            dbSet = _context.Set<TEntity>();
        }

        public async Task CreateOne(TEntity entity)
        {
            await _context.Set<TEntity>().AddAsync(entity);
        }
        public void UpdateOne(TEntity entity)
        {
            if (_context.Entry(entity).State == EntityState.Detached)
            {
                dbSet.Attach(entity);
                _context.Entry(entity).State = EntityState.Modified;
            }
        }
        public void DeleteOne(TEntity entity)
        {
            if (_context.Entry(entity).State == EntityState.Detached)
            {
                dbSet.Attach(entity);
            }
            dbSet.Remove(entity);
        }
        public void DeleteOne(object id)
        {
            TEntity entity = dbSet.Find(id);
            DeleteOne(entity);
        }
        public bool CheckExist(Expression<Func<TEntity, bool>> filter = null)
        {
            return dbSet.Any(filter);
        }
        public async Task<TEntity> GetOneAsync(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IQueryable<TEntity>> include = null,
            bool asNoTracking = false)
        {
            IQueryable<TEntity> query = dbSet;
            query = include is not null ? include(query) : query;
            query = asNoTracking ? query.AsNoTracking() : query;
            return filter is not null ? await query.FirstOrDefaultAsync(filter) : null;
        }
        public async Task<IEnumerable<TEntity>> GetAsync(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            Func<IQueryable<TEntity>, IQueryable<TEntity>> include = null,
            bool asNoTracking = false,
            int? numberOfSkip = null,
            int? numberOfTake = null)
        {
            IQueryable<TEntity> query = dbSet;
            query = filter is not null ? query.Where(filter) : query;
            query = orderBy is not null ? orderBy(query) : query;
            query = include is not null ? include(query) : query;
            query = asNoTracking ? query.AsNoTracking() : query;
            query = numberOfSkip is not null ? query.Skip((int)numberOfSkip) : query;
            query = numberOfTake is not null ? query.Take((int)numberOfTake) : query;
            return await query.ToListAsync();
        }
        public async Task<IEnumerable<TEntity>> GetFullTextSearchByNameAsync(
                string keyword = null,
                string stringSqlRaw = null,
                int startRow = 1,
                int endRow = int.MaxValue)
        {
            var nameParam = new SqlParameter("@name", "\"*" + keyword + "*\"");
            var startRowParam = new SqlParameter("@startRow", startRow);
            var endRowParam = new SqlParameter("@endRow", endRow);
            return await dbSet.FromSqlRaw(stringSqlRaw, nameParam, startRowParam, endRowParam)
                        .ToListAsync();
        }
    }
}
