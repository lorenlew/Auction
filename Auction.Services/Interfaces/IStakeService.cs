using Auction.Domain.DerivativeModels;
using Auction.Domain.Models;
using Auction.UoW.Interfaces;

namespace Auction.Services.Interfaces
{
    public interface IStakeService : IService
    {
        IRepository<Stake> GetRepository();

        Stake Create(int id, double? stakeIncrease, LotStake currentLot);
    }
}
