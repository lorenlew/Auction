using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using Auction.Models.DomainModels;
using Auction.Models.ViewModels;
using Microsoft.AspNet.Identity.EntityFramework;

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


        public static IEnumerable<LotViewModel> GetLotsWithStakesViewModel()
        {
            ApplicationDbContext db = Create();

            var lotWithStakes = (from lot in db.Lots
                                 join stake in db.Stakes on lot.LotId equals stake.LotId into jlotStake
                                 from lotStake in jlotStake.DefaultIfEmpty()
                                 select new LotViewModel()
                                 {
                                     LotId = lot.LotId,
                                     Name = lot.Name,
                                     Description = lot.Description,
                                     ImagePath = lot.ImagePath,
                                     HoursDuration = lot.HoursDuration,
                                     InitialStake = lot.InitialStake,
                                     LastDateOfStake = lotStake.DateOfStake,
                                     HoursForAuctionEnd = lotStake.HoursForAuctionEnd,
                                     LastStake = lotStake.CurrentStake
                                 });

            var groupedLotStakes = (from groupedLots in lotWithStakes
                                    group groupedLots by new { groupedLots.LotId } into g
                                    let firstOrDefault = g.FirstOrDefault()
                                    where firstOrDefault != null
                                    select new LotViewModel() 
                                  { 
                                      LotId = g.Key.LotId,
                                      Name = firstOrDefault.Name,
                                      Description = firstOrDefault.Description,
                                      ImagePath = firstOrDefault.ImagePath,
                                      HoursDuration = firstOrDefault.HoursDuration,
                                      InitialStake = firstOrDefault.InitialStake,
                                      LastDateOfStake = firstOrDefault.LastDateOfStake,
                                      HoursForAuctionEnd = firstOrDefault.HoursForAuctionEnd,
                                      LastStake = firstOrDefault.LastStake
                                  }).ToList();
            return groupedLotStakes;
        }
    }
}