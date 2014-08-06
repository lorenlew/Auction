using System;
using System.Collections.Generic;
using System.Linq;
using Auction.DAL.Models;
using Auction.DAL.UnitOfWork;
using Auction.Domain.DerivativeModels;
using Auction.Domain.Models;
using Auction.Services.Interfaces;
using AutoMapper;

namespace Auction.Services
{
    public class LotService : BaseService, ILotService
    {
        public LotService(IUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
        }

        public IEnumerable<LotStakeDomainModel> GetAll()
        {
            var lotsAndStakes = (from lot in Uow.LotRepository.Read()
                                 join stake in Uow.StakeRepository.Read() on lot.Id equals stake.LotId into joinedLotsAndStakes
                                 from jLotsAndStakes in joinedLotsAndStakes.DefaultIfEmpty()
                                 select new LotStakeDomainModel()
                                 {
                                     LotId = lot.Id,
                                     Name = lot.Name,
                                     Description = lot.Description,
                                     ImagePath = lot.ImagePath,
                                     HoursDuration = lot.HoursDuration,
                                     InitialStake = lot.InitialStake,
                                     IsSold = lot.IsSold,
                                     LastDateOfStake = jLotsAndStakes.DateOfStake,
                                     StakeTimeout = jLotsAndStakes.StakeTimeout,
                                     LastStake = jLotsAndStakes.CurrentStake,
                                     ApplicationUserId = jLotsAndStakes.ApplicationUserId,
                                     IsAvailable = (bool?)(jLotsAndStakes.StakeTimeout > DateTime.Now) ?? true
                                 });

            var groupedLotStakes = (from lots in lotsAndStakes
                                    group lots by new { lots.LotId } into grp
                                    let firstOrDefault = grp.OrderByDescending(p => p.LastDateOfStake).FirstOrDefault()
                                    select firstOrDefault).ToList();
            return groupedLotStakes;
        }

        public IEnumerable<LotStakeDomainModel> GetAvailable()
        {
            var availableLotsAndStakes = (from lots in GetAll()
                                          where !lots.IsSold
                                          orderby lots.Name
                                          select lots).ToList();

            return availableLotsAndStakes;
        }

        public IEnumerable<LotStakeDomainModel> GetSold()
        {
            var availableLotsAndStakes = (from lots in GetAll()
                                          where lots.IsSold
                                          select lots).ToList();

            return availableLotsAndStakes;
        }

        public LotStakeDomainModel FindById(int id)
        {
            var currentLot = (from lots in GetAvailable()
                              where lots.LotId == id
                              select lots).SingleOrDefault();
            return currentLot;
        }

        public void Add(LotDomainModel entity)
        {
            if (entity == null) throw new ArgumentNullException("entity");
            var lot = Mapper.Map<LotDomainModel, Lot>(entity);
            Uow.LotRepository.Add(lot);
        }

        public IEnumerable<LotDomainModel> Read()
        {
            var lots = Uow.LotRepository.Read();
            var lotsDomain = Mapper.Map<IEnumerable<Lot>, IEnumerable<LotDomainModel>>(lots);
            return lotsDomain;
        }

        public LotDomainModel ReadById(int id)
        {
            var lot = Uow.LotRepository.ReadById(id);
            var lotDomain = Mapper.Map<Lot, LotDomainModel>(lot);
            return lotDomain;
        }

        public void Update(LotDomainModel entity)
        {
            if (entity == null) throw new ArgumentNullException("entity");
            var lot = Mapper.Map<LotDomainModel, Lot>(entity);
            Uow.LotRepository.Update(lot);
        }

        public void Delete(LotDomainModel entity)
        {
            if (entity == null) throw new ArgumentNullException("entity");
            var lot = Mapper.Map<LotDomainModel, Lot>(entity);
            Uow.LotRepository.Delete(lot);
        }

        public void Save()
        {
            Uow.Save();
        }

        public void DisableValidationOnSave()
        {
            Uow.DisableValidationOnSave();
        }
    }
}
