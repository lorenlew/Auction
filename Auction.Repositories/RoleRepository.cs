using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Auction.DAL;
using Auction.Interfaces;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Auction.Repositories
{
    public class RoleRepository : IRoleRepository
    {
        private readonly ApplicationDbContext _context;

        private readonly DbSet<IdentityRole> _dbSet;

        public RoleRepository(ApplicationDbContext context)
        {
            if (context == null) return;
            _context = context;
            _dbSet = context.Set<IdentityRole>();
        }

        void IRoleRepository.Create(IdentityRole entity)
        {
            _dbSet.Add(entity);
        }

        IEnumerable<IdentityRole> IRoleRepository.Read(Expression<Func<IdentityRole, bool>> filter, Func<IQueryable<IdentityRole>, IOrderedQueryable<IdentityRole>> orderBy, string includeProperties)
        {
            IQueryable<IdentityRole> query = _dbSet;

            if (filter != null)
                query = query.Where(filter);

            query = includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Aggregate(query, (current, includeProperty) => current.Include(includeProperty));

            if (orderBy != null)
                return orderBy(query).ToList();

            return query;
        }

        IdentityRole IRoleRepository.ReadById(object id)
        {
            return _dbSet.Find(id);
        }

        void IRoleRepository.Update(IdentityRole entity)
        {
            _dbSet.Attach(entity);
            _context.Entry(entity).State = EntityState.Modified;
        }

        void IRoleRepository.Delete(object id)
        {
            IdentityRole entity = _dbSet.Find(id);
            Delete(entity);
        }

        private void Delete(IdentityRole entity)
        {
            if (_context.Entry(entity).State == EntityState.Detached)
            {
                _dbSet.Attach(entity);
            }
            _dbSet.Remove(entity);
        }
    }
}
