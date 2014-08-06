using System;
using Auction.DAL.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Auction.DAL.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        ApplicationDbContext Context { get; }

        UserManager<ApplicationUser> UserManager { get; set; }

        RoleManager<IdentityRole> RoleManager { get; }

        IRepository<Lot> LotRepository { get; }

        IRepository<Stake> StakeRepository { get; }

        void DisableValidationOnSave();

        void Save();
    }
}
