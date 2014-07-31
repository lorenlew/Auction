using System;
using System.Data.Entity;
using Auction.DAL;
using Auction.DAL.Migrations;
using Auction.Domain.Models;
using Auction.UoW.Interfaces;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Auction.UoW.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private bool _disposed;

        private RoleManager<IdentityRole> _roleManager;

        private IRepository<Lot> _lotRepository;

        private IRepository<Stake> _stakeRepository;

        public ApplicationDbContext Context { get; private set; }

        public UserManager<ApplicationUser> UserManager { get; private set; }

        public RoleManager<IdentityRole> RoleManager { get; private set; }


        public IRepository<Lot> LotRepository
        {
            get
            {
                return _lotRepository ?? (_lotRepository = new BaseRepository<Lot>(Context));
            }
        }

        public IRepository<Stake> StakeRepository
        {
            get
            {
                return _stakeRepository ?? (_stakeRepository = new BaseRepository<Stake>(Context));
            }
        }

        private void Init()
        {
            var dbContext = ApplicationDbContext.Create();
            Context = Context ?? dbContext;
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(Context));
            UserManager = UserManager ?? userManager;
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(Context));
            RoleManager = RoleManager ?? roleManager;
        }

        public UnitOfWork()
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<ApplicationDbContext, Configuration>());
            Init();
        }

        public void Save()
        {
            Context.SaveChanges();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
            {
                return;
            }
            if (disposing)
            {
                Context.Dispose();
                UserManager.Dispose();
                RoleManager.Dispose();
            }
            _disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public void DisableValidationOnSave()
        {
            Context.Configuration.ValidateOnSaveEnabled = false;
        }

    }
}
