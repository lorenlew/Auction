using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Auction.DAL;
using Auction.DAL.DomainModels;
using Auction.DAL.Migrations;
using Auction.Interfaces;

namespace Auction.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private bool disposed = false;

        private readonly ApplicationDbContext context = new ApplicationDbContext();

        private IUserRepository userRepository;

        private IRoleRepository roleRepository;

        private IRepository<Lot> lotRepository;

        private IRepository<Stake> stakeRepository;


        public ApplicationDbContext Context
        {
            get { return context; }
        }

        public IUserRepository UserRepository
        {
            get
            {
                return userRepository ?? (userRepository = new UserRepository(context));
            }
        }

        public IRoleRepository RoleRepository
        {
            get
            {
                return roleRepository ?? (roleRepository = new RoleRepository(context));
            }
        }

        public IRepository<Lot> LotRepository
        {
            get
            {
                return lotRepository ?? (lotRepository = new BaseRepository<Lot>(context));
            }
        }

        public IRepository<Stake> StakeRepository
        {
            get
            {
                return stakeRepository ?? (stakeRepository = new BaseRepository<Stake>(context));
            }
        }

        public UnitOfWork()
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<ApplicationDbContext, Configuration>());
        }

        void IUnitOfWork.Save()
        {
            context.SaveChanges();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    context.Dispose();
                }
            }
            this.disposed = true;
        }

        void IDisposable.Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
