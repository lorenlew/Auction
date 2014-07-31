using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Auction.Services.Interfaces
{
    public interface IRoleManagerService : IService
    {
        RoleManager<IdentityRole> GetAccess();
    }
}
