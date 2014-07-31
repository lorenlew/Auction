using System.Collections.Generic;
using System.Linq;
using Auction.Domain;
using Auction.Domain.Models;
using Auction.Repositories.Interfaces;

namespace Auction.Services.Interfaces
{
    public interface ILotService : IService
    {
        IRepository<Lot> Get();

        IEnumerable<LotStake> GetAll();

        IEnumerable<LotStake> GetAvailable();

        IEnumerable<LotStake> GetSold();

        LotStake GetCurrentLot(int id);

    }
}
