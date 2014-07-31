using System.Collections.Generic;
using Auction.Domain.DerivativeModels;
using Auction.Domain.Models;
using Auction.UoW.Interfaces;

namespace Auction.Services.Interfaces
{
    public interface ILotService : IService
    {
        IRepository<Lot> GetRepository();

        IEnumerable<LotStake> GetAll();

        IEnumerable<LotStake> GetAvailable();

        IEnumerable<LotStake> GetSold();

        LotStake FindById(int id);

    }
}
