using System;
using System.Linq;
using System.Linq.Expressions;
using Auction.Domain.Models;

namespace Auction.Repositories.Interfaces
{
    public interface IRepository<TEntity> where TEntity : Entity
    {
        void Create(TEntity entity);

        IQueryable<TEntity> Read(Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, String includeProperties = "");

        TEntity ReadById(Object id);

        void Update(TEntity entity);

        void Delete(TEntity entity);
    }
}
