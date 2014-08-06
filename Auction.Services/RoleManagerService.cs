using Auction.DAL.UnitOfWork;
using Auction.Services.Interfaces;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Auction.Services
{
    public class RoleManagerService : BaseService, IRoleManagerService
    {
        public RoleManagerService(IUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
        }

        public RoleManager<IdentityRole> RoleManager
        {
            get { return Uow.RoleManager; }
        }
    }
}
