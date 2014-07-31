using Auction.Services.Interfaces;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Auction.Services
{
    public class RoleManagerService : BaseService, IRoleManagerService
    {
        public RoleManager<IdentityRole> GetAccess()
        {
            return Uow.RoleManager;
        }
    }
}
