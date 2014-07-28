﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Auction.Models.ViewModels;
using DomainModels.DomainModels;
using Microsoft.AspNet.Identity;

namespace Auction.Models
{
    public class ViewModelsContext
    {
        public static IEnumerable<LotViewModel> GetLotsAndStakesViewModel(ApplicationDbContext db)
        {
            var lotsAndStakes = (from lot in db.Lots
                                 join stake in db.Stakes on lot.LotId equals stake.LotId into joinedLotsAndStakes
                                 from jLotsAndStakes in joinedLotsAndStakes.DefaultIfEmpty()
                                 select new LotViewModel()
                                 {
                                     LotId = lot.LotId,
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
        public static IEnumerable<LotViewModel> GetAvailableLotsAndStakesViewModel(ApplicationDbContext db)
        {
            var availableLotsAndStakes = (from lots in GetLotsAndStakesViewModel(db)
                                          where !lots.IsSold
                                          select lots).ToList();

            return availableLotsAndStakes;
        }

        public static IEnumerable<LotViewModel> GetSoldLotsAndStakesViewModel(ApplicationDbContext db)
        {
            var availableLotsAndStakes = (from lots in GetLotsAndStakesViewModel(db)
                                          where lots.IsSold
                                          select lots).ToList();

            return availableLotsAndStakes;
        }

        public static LotViewModel GetCurrentLot(int id, ApplicationDbContext db)
        {
            var currentLot = (from lots in GetAvailableLotsAndStakesViewModel(db)
                              where lots.LotId == id
                              select lots).Single();
            return currentLot;
        }

        public static Stake GetCurrentStake(int id, double? stakeIncrease, LotViewModel currentLot)
        {
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