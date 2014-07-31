using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Auction.Domain.Models;

namespace Auction.Repositories.Interfaces
{
    public interface IUserRepository
    {
        void Create(ApplicationUser entity);

        IEnumerable<ApplicationUser> Read(Expression<Func<ApplicationUser, bool>> filter = null,
            Func<IQueryable<ApplicationUser>, IOrderedQueryable<ApplicationUser>> orderBy = null, String includeProperties = "");

        ApplicationUser ReadById(Object id);

        void Update(ApplicationUser entity);

        void Delete(Object id);
    }
}
