using System.Collections.Generic;
using Auction.Domain.DerivativeModels;
using Auction.Domain.Models;

namespace Auction.Services.Interfaces
{
    public interface IStakeService : IService
    {
        double MinStakeRate { get; }

        StakeDomainModel Create(int id, double? stakeIncrease, LotStakeDomainModel currentLot);

        void Add(StakeDomainModel entity);

        IEnumerable<StakeDomainModel> Read();

        StakeDomainModel ReadById(int id);

        void Update(StakeDomainModel entity);

        void Delete(StakeDomainModel entity);

        void Save();

        void DisableValidationOnSave();

    }
}
