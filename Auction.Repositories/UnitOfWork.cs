using System;
using System.Data.Entity;
using Auction.DAL;
using Auction.DAL.Migrations;
using Auction.Domain.Models;
using Auction.Interfaces;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Auction.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private bool _disposed;

        private ApplicationDbContext _context;

        private UserManager<ApplicationUser> _userManager;

        private IUserRepository _userRepository;

        private IRoleRepository _roleRepository;

        private IRepository<Lot> _lotRepository;

        private IRepository<Stake> _stakeRepository;

        public ApplicationDbContext Context
        {
            get { return _context; }
        }
        public UserManager<ApplicationUser> UserManager
        {
            get { return _userManager; }
        }

        public IUserRepository UserRepository
        {
            get
            {
                return _userRepository ?? (_userRepository = new UserRepository(_context));
            }
        }

        public IRoleRepository RoleRepository
        {
            get
            {
                return _roleRepository ?? (_roleRepository = new RoleRepository(_context));
            }
        }

        public IRepository<Lot> LotRepository
        {
            get
            {
                return _lotRepository ?? (_lotRepository = new BaseRepository<Lot>(_context));
            }
        }

        public IRepository<Stake> StakeRepository
        {
            get
            {
                return _stakeRepository ?? (_stakeRepository = new BaseRepository<Stake>(_context));
            }
        }

        private void Init()
        {
            var dbContext = ApplicationDbContext.Create();
            _context = _context ?? dbContext;
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(_context));
            _userManager = _userManager ?? userManager;
        }

        public UnitOfWork()
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<ApplicationDbContext, Configuration>());
            Init();
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
            {
                return;
            }
            if (disposing)
            {
                _context.Dispose();
                _userManager.Dispose();
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
            _context.Configuration.ValidateOnSaveEnabled = false;
        }

    }
}
