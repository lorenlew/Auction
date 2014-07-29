using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Auction.DAL;
using Auction.DAL.DomainModels;

namespace Auction.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        ApplicationDbContext Context { get; }

        IUserRepository UserRepository { get; }

        IRoleRepository RoleRepository { get; }

        IRepository<Lot> LotRepository { get; }

        IRepository<Stake> StakeRepository { get; }

        void Save();
    }
}
