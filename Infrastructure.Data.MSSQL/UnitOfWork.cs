using Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Data.MSSQL
{
    public class UnitOfWork: IUnitOfWork
    {
        private TestDBContext _context;

        public UnitOfWork(TestDBContext context)
        {
            _context = context;
        }
        
        public Dictionary<Type, object> repositories = new Dictionary<Type, object>();

        public void SaveChanges()
        {
             _context.SaveChanges();
        }

        public IRepository<TEntity> Repository<TEntity>() where TEntity : class
        {
            if (repositories.Keys.Contains(typeof(TEntity)))
            {
                return (IRepository<TEntity>)repositories[typeof(TEntity)];
            }
            IRepository<TEntity> repo = new Repository<TEntity>(_context);

            repositories.Add(typeof(TEntity), repo);
            return repo;
        }
    }
}
