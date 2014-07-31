using System;
using Auction.DAL;
using Auction.Domain.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Auction.UoW.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        ApplicationDbContext Context { get; }

        UserManager<ApplicationUser> UserManager { get; }

        RoleManager<IdentityRole> RoleManager { get; }

        IRepository<Lot> LotRepository { get; }

        IRepository<Stake> StakeRepository { get; }

        void DisableValidationOnSave();

        void Save();
    }
}
