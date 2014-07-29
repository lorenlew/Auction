using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using Auction.DAL;
using Auction.Domain.Models;
using Auction.Interfaces;

namespace Auction.Repositories
{
    public class UserRepository : IUserRepository
    {

        private readonly ApplicationDbContext _context;

        private readonly DbSet<ApplicationUser> _dbSet;

        public UserRepository(ApplicationDbContext context)
        {
            if (context == null) return;
            _context = context;
            _dbSet = context.Set<ApplicationUser>();
        }

        void IUserRepository.Create(ApplicationUser entity)
        {
            _dbSet.Add(entity);
        }

        IEnumerable<ApplicationUser> IUserRepository.Read(Expression<Func<ApplicationUser, bool>> filter, Func<IQueryable<ApplicationUser>, IOrderedQueryable<ApplicationUser>> orderBy, string includeProperties)
        {
            IQueryable<ApplicationUser> query = _dbSet;

            if (filter != null)
                query = query.Where(filter);

            query = includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Aggregate(query, (current, includeProperty) => current.Include(includeProperty));

            if (orderBy != null)
                return orderBy(query).ToList();

            return query;
        }

        ApplicationUser IUserRepository.ReadById(object id)
        {
            return _dbSet.Find(id);
        }

        void IUserRepository.Update(ApplicationUser entity)
        {
            _dbSet.Attach(entity);
            _context.Entry(entity).State = EntityState.Modified;
        }

        void IUserRepository.Delete(object id)
        {
            ApplicationUser entity = _dbSet.Find(id);
            Delete(entity);
        }

        private void Delete(ApplicationUser entity)
        {
            if (_context.Entry(entity).State == EntityState.Detached)
            {
                _dbSet.Attach(entity);
            }
            _dbSet.Remove(entity);
        }

    }
}
