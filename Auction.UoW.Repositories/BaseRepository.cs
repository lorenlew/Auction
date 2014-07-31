using System;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using Auction.DAL;
using Auction.Domain.Models;
using Auction.UoW.Interfaces;

namespace Auction.UoW.Repositories
{
    public class BaseRepository<TEntity> : IRepository<TEntity> where TEntity : Entity
    {
        private readonly ApplicationDbContext _context;

        private readonly DbSet<TEntity> _dbSet;

        public BaseRepository(ApplicationDbContext context)
        {
            if (context == null) return;
            _context = context;
            _dbSet = context.Set<TEntity>();
        }

        void IRepository<TEntity>.Create(TEntity entity)
        {
            _dbSet.Add(entity);
        }

        IQueryable<TEntity> IRepository<TEntity>
            .Read(Expression<Func<TEntity, bool>> filter, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy, String includeProperties)
        {
            IQueryable<TEntity> query = _dbSet;

            if (filter != null)
                query = query.Where(filter);

            query = includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Aggregate(query, (current, includeProperty) => current.Include(includeProperty));

            if (orderBy != null)
                return orderBy(query).ToList().AsQueryable();

            return query.AsQueryable();
        }

        TEntity IRepository<TEntity>.ReadById(object id)
        {
            return _dbSet.Find(id);
        }

        void IRepository<TEntity>.Update(TEntity entity)
        {
            if (_context.Entry(entity).State == EntityState.Detached)
            {
                _dbSet.Attach(entity);
            }
            _context.Entry(entity).State = EntityState.Modified;
        }

        void IRepository<TEntity>.Delete(TEntity entity)
        {
            if (_context.Entry(entity).State == EntityState.Detached)
            {
                _dbSet.Attach(entity);
            }
            _dbSet.Remove(entity);
        }
    }
}
