using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IRepository<TEntity> where TEntity : class
    {
        public Task CreateOne(TEntity entity);
        public Task<TEntity> GetOneAsync(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IQueryable<TEntity>> include = null,
            bool asNoTracking = false);
        public Task<IEnumerable<TEntity>> GetAsync(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            Func<IQueryable<TEntity>, IQueryable<TEntity>> include = null,
            bool asNoTracking = false,
            int? numberOfSkip = null,
            int? numberOfTake = null);
        public void UpdateOne(TEntity entity);
        public void DeleteOne(TEntity entity);
        public void DeleteOne(object id);
        public bool CheckExist(Expression<Func<TEntity, bool>> filter = null);
        public Task<IEnumerable<TEntity>> GetFullTextSearchByNameAsync(
                string keyword = null,
                string stringSqlRaw = null,
                int startRow = 1,
                int endRow = int.MaxValue);
    }
}
