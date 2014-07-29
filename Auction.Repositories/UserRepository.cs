using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using Auction.DAL;
using Auction.DAL.DomainModels;
using Auction.Interfaces;

namespace Auction.Repositories
{
    public class UserRepository : IUserRepository
    {

        private ApplicationDbContext context;

        private DbSet<ApplicationUser> dbSet;

        public UserRepository(ApplicationDbContext context)
        {
            this.context = context;
            dbSet = context.Set<ApplicationUser>();
        }

        void IUserRepository.Create(ApplicationUser entity)
        {
            dbSet.Add(entity);
        }

        IEnumerable<ApplicationUser> IUserRepository.Read(Expression<Func<ApplicationUser, bool>> filter = null, Func<IQueryable<ApplicationUser>, IOrderedQueryable<ApplicationUser>> orderBy = null, string includeProperties = "")
        {
            IQueryable<ApplicationUser> query = dbSet;

            if (filter != null)
                query = query.Where(filter);

            query = includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Aggregate(query, (current, includeProperty) => current.Include(includeProperty));

            if (orderBy != null)
                return orderBy(query).ToList();

            return query;
        }

        ApplicationUser IUserRepository.ReadById(object id)
        {
            return dbSet.Find(id);
        }

        void IUserRepository.Update(ApplicationUser entity)
        {
            dbSet.Attach(entity);
            context.Entry(entity).State = EntityState.Modified;
        }

        void IUserRepository.Delete(object id)
        {
            ApplicationUser entity = dbSet.Find(id);
            Delete(entity);
        }

        private void Delete(ApplicationUser entity)
        {
            if (context.Entry(entity).State == EntityState.Detached)
            {
                dbSet.Attach(entity);
            }
            dbSet.Remove(entity);
        }

    }
}
