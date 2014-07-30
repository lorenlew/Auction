using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Auction.DAL;
using Auction.Domain.Models;
using Auction.Repositories;
using Auction.Web.ViewModels;
using Microsoft.AspNet.Identity;
using Ninject.Infrastructure.Language;

namespace Auction.Web
{
    public class ViewModelsLogic
    {
        public static IEnumerable<LotViewModel> GetLotsAndStakesViewModel(UnitOfWork uow)
        {
            var lotsAndStakes = (from lot in uow.LotRepository.Read()
                                 join stake in uow.StakeRepository.Read() on lot.Id equals stake.LotId into joinedLotsAndStakes
                                 from jLotsAndStakes in joinedLotsAndStakes.DefaultIfEmpty()
                                 select new LotViewModel()
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

        public static IEnumerable<LotViewModel> GetAvailableLotsAndStakesViewModel(UnitOfWork uow)
        {
            var availableLotsAndStakes = (from lots in GetLotsAndStakesViewModel(uow)
                                          where !lots.IsSold
                                          orderby lots.Name
                                          select lots).ToList();

            return availableLotsAndStakes;
        }

        public static IEnumerable<LotViewModel> GetSoldLotsAndStakesViewModel(UnitOfWork uow)
        {
            var availableLotsAndStakes = (from lots in GetLotsAndStakesViewModel(uow)
                                          where lots.IsSold
                                          select lots).ToList();

            return availableLotsAndStakes;
        }

        public static LotViewModel GetCurrentLot(int id, UnitOfWork uow)
        {
            var currentLot = (from lots in GetAvailableLotsAndStakesViewModel(uow)
                              where lots.LotId == id
                              select lots).SingleOrDefault();
            return currentLot;
        }

        public static Stake GetCurrentStake(int id, double? stakeIncrease, LotViewModel currentLot)
        {
            if (currentLot == null) throw new ArgumentNullException("currentLot");
            var currentStake = new Stake
            {
                LotId = id,
                ApplicationUserId = HttpContext.Current.User.Identity.GetUserId(),
                DateOfStake = DateTime.Now
            };

            if (currentLot.LastStake == null)
            {
                currentStake.StakeTimeout = DateTime.Now.AddHours(currentLot.HoursDuration);
                currentStake.CurrentStake = currentLot.InitialStake;
            }
            else
            {
                currentStake.StakeTimeout = currentLot.StakeTimeout.GetValueOrDefault().AddMinutes(1);
                currentStake.CurrentStake = (int)(currentLot.LastStake * stakeIncrease);
            }
            return currentStake;
        }
    }
}