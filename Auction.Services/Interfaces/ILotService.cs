using System.Collections.Generic;
using Auction.Domain.DerivativeModels;
using Auction.Domain.Models;

namespace Auction.Services.Interfaces
{
    public interface ILotService : IService
    {

        IEnumerable<LotStakeDomainModel> GetAll();

        IEnumerable<LotStakeDomainModel> GetAvailable();

        IEnumerable<LotStakeDomainModel> GetSold();

        LotStakeDomainModel FindById(int id);

        void Add(LotDomainModel entity);

        IEnumerable<LotDomainModel> Read();

        LotDomainModel ReadById(int id);

        void Update(LotDomainModel entity);

        void Delete(LotDomainModel entity);

        void Save();

        void DisableValidationOnSave();

    }
}
