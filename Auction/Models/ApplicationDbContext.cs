using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using Auction.Models.DomainModels;
using Auction.Models.ViewModels;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Ninject.Infrastructure.Language;

namespace Auction.Models
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Lot> Lots { get; set; }

        public DbSet<Stake> Stakes { get; set; }

        public ApplicationDbContext()
            : base("AuctionString", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        public static IEnumerable<LotViewModel> GetLotsAndStakesViewModel()
        {
            ApplicationDbContext db = Create();

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
        public static IEnumerable<LotViewModel> GetAvailableLotsAndStakesViewModel()
        {
            var availableLotsAndStakes = (from lots in GetLotsAndStakesViewModel()
                                 where !lots.IsSold
                                 select lots).ToList();

            return availableLotsAndStakes;
        }

        public static IEnumerable<LotViewModel> GetSoldLotsAndStakesViewModel()
        {
            var availableLotsAndStakes = (from lots in GetLotsAndStakesViewModel()
                                          where lots.IsSold
                                          select lots).ToList();

            return availableLotsAndStakes;
        }

        public static LotViewModel GetCurrentLot(int id)
        {
            var currentLot = (from lots in GetAvailableLotsAndStakesViewModel()
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