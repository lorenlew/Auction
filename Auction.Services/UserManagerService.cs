using Auction.Domain.Models;
using Auction.Services.Interfaces;
using Microsoft.AspNet.Identity;

namespace Auction.Services
{
    public class UserManagerService : BaseService, IUserManagerService
    {
        public UserManager<ApplicationUser> Get()
        {
            return Uow.UserManager;
        }
    }
}
