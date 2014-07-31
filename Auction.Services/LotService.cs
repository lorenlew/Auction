using System;
using System.Collections.Generic;
using System.Linq;
using Auction.Domain;
using Auction.Repositories.Interfaces;
using Auction.Services.Interfaces;

namespace Auction.Services
{
    public class LotService : BaseService, ILotService
    {
        public IRepository<Domain.Models.Lot> Get()
        {
            return Uow.LotRepository;
        }

        public  IEnumerable<LotStake> GetAll()
        {
            var lotsAndStakes = (from lot in Uow.LotRepository.Read()
                                 join stake in Uow.StakeRepository.Read() on lot.Id equals stake.LotId into joinedLotsAndStakes
                                 from jLotsAndStakes in joinedLotsAndStakes.DefaultIfEmpty()
                                 select new LotStake()
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

        public IEnumerable<LotStake> GetAvailable()
        {
            var availableLotsAndStakes = (from lots in GetAll()
                                          where !lots.IsSold
                                          orderby lots.Name
                                          select lots).ToList();

            return availableLotsAndStakes;
        }

        public IEnumerable<LotStake> GetSold()
        {
            var availableLotsAndStakes = (from lots in GetAll()
                                          where lots.IsSold
                                          select lots).ToList();

            return availableLotsAndStakes;
        }

        public  LotStake GetCurrentLot(int id)
        {
            var currentLot = (from lots in GetAvailable()
                              where lots.LotId == id
                              select lots).SingleOrDefault();
            return currentLot;
        }
    }
}
