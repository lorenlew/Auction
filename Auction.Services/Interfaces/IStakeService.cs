using Auction.Domain.Models;
using Auction.Repositories.Interfaces;

namespace Auction.Services.Interfaces
{
    public interface IStakeService : IService
    {
        IRepository<Stake> Get();
    }
}
