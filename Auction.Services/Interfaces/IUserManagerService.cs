using Auction.Domain.Models;
using Microsoft.AspNet.Identity;

namespace Auction.Services.Interfaces
{
    public interface IUserManagerService : IService
    {
        UserManager<ApplicationUser> GetAccess();
    }
}
