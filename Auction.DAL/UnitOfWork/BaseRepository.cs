using System;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using Auction.DAL.Models;

namespace Auction.DAL.UnitOfWork
{
    public class BaseRepository<TEntity> : IRepository<TEntity> where TEntity : Entity
    {
        private readonly ApplicationDbContext _context;

        private readonly DbSet<TEntity> _dbSet;

        public BaseRepository(ApplicationDbContext context)
        {
            if (context == null) throw new ArgumentNullException("context");
            _context = context;
            _dbSet = context.Set<TEntity>();
        }

        void IRepository<TEntity>.Add(TEntity entity)
        {
            if (entity == null) throw new NoNullAllowedException();
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
            if (id == null) throw new NoNullAllowedException();
            return _dbSet.Find(id);
        }

        void IRepository<TEntity>.Update(TEntity entity)
        {
            if (entity == null)  throw new NoNullAllowedException();
            var entityToUpdate = _dbSet.Find(entity.Id);
            if (entityToUpdate == null) return;
            _context.Entry(entityToUpdate).State = EntityState.Detached;
            _dbSet.Attach(entity);
            _context.Entry(entity).State = EntityState.Modified;
        }

        void IRepository<TEntity>.Delete(TEntity entity)
        {
            if (entity == null) throw new NoNullAllowedException();

            var entityToDelete = _dbSet.Find(entity.Id);
            if (entityToDelete == null) return;
            _dbSet.Remove(entityToDelete);
        }
    }
}
