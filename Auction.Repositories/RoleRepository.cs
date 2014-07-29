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
        private ApplicationDbContext context;

        private DbSet<IdentityRole> dbSet;

        public RoleRepository(ApplicationDbContext context)
        {
            this.context = context;
            dbSet = context.Set<IdentityRole>();
        }

        void IRoleRepository.Create(IdentityRole entity)
        {
            dbSet.Add(entity);
        }

        IEnumerable<IdentityRole> IRoleRepository.Read(Expression<Func<IdentityRole, bool>> filter = null, Func<IQueryable<IdentityRole>, IOrderedQueryable<IdentityRole>> orderBy = null, string includeProperties = "")
        {
            IQueryable<IdentityRole> query = dbSet;

            if (filter != null)
                query = query.Where(filter);

            query = includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Aggregate(query, (current, includeProperty) => current.Include(includeProperty));

            if (orderBy != null)
                return orderBy(query).ToList();

            return query;
        }

        IdentityRole IRoleRepository.ReadById(object id)
        {
            return dbSet.Find(id);
        }

        void IRoleRepository.Update(IdentityRole entity)
        {
            dbSet.Attach(entity);
            context.Entry(entity).State = EntityState.Modified;
        }

        void IRoleRepository.Delete(object id)
        {
            IdentityRole entity = dbSet.Find(id);
            Delete(entity);
        }

        private void Delete(IdentityRole entity)
        {
            if (context.Entry(entity).State == EntityState.Detached)
            {
                dbSet.Attach(entity);
            }
            dbSet.Remove(entity);
        }
    }
}
