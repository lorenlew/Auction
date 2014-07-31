using System;
using Auction.DAL;
using Auction.Domain.Models;
using Microsoft.AspNet.Identity;

namespace Auction.Repositories.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        ApplicationDbContext Context { get; }

        UserManager<ApplicationUser> UserManager { get; }

        IUserRepository UserRepository { get; }

        IRoleRepository RoleRepository { get; }

        IRepository<Lot> LotRepository { get; }

        IRepository<Stake> StakeRepository { get; }

        void DisableValidationOnSave();

        void Save();
    }
}
