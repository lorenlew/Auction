using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Auction.Interfaces;

namespace Auction.Web.ViewModels
{
    public class LotStakeViewModel
    {
        public int LotId { get; set; }

        [Display(Name = "Lot name")]
        public string Name { get; set; }

        [Display(Name = "Lot description")]
        public string Description { get; set; }

        [Display(Name = "Image")]
        public string ImagePath { get; set; }

        [Display(Name = "Duration (hours)")]
        public int HoursDuration { get; set; }

        [Display(Name = "Initial stake")]
        public int InitialStake { get; set; }

        [Display(Name = "Last date of stake")]
        [DisplayFormat(DataFormatString = "{0:dd'.'MM'.'yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? LastDateOfStake { get; set; }

        [Display(Name = "Stake timeout")]
        [DisplayFormat(DataFormatString = "{0:dd'.'MM'.'yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? StakeTimeout { get; set; }

        [Display(Name = "Last stake")]
        public int? LastStake { get; set; }

        public string ApplicationUserId { get; set; }

        [Display(Name = "Is available")]
        public bool IsAvailable { get; set; }

        public bool IsSold { get; set; }

        public static IEnumerable<LotStakeViewModel> GetAll(IUnitOfWork uow)
        {
            var lotsAndStakes = (from lot in uow.LotRepository.Read()
                                 join stake in uow.StakeRepository.Read() on lot.Id equals stake.LotId into joinedLotsAndStakes
                                 from jLotsAndStakes in joinedLotsAndStakes.DefaultIfEmpty()
                                 select new LotStakeViewModel()
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

        public static IEnumerable<LotStakeViewModel> GetAvailable(IUnitOfWork uow)
        {
            var availableLotsAndStakes = (from lots in GetAll(uow)
                                          where !lots.IsSold
                                          orderby lots.Name
                                          select lots).ToList();

            return availableLotsAndStakes;
        }

        public static IEnumerable<LotStakeViewModel> GetSold(IUnitOfWork uow)
        {
            var availableLotsAndStakes = (from lots in GetAll(uow)
                                          where lots.IsSold
                                          select lots).ToList();

            return availableLotsAndStakes;
        }

        public static LotStakeViewModel GetCurrentLot(int id, IUnitOfWork uow)
        {
            var currentLot = (from lots in GetAvailable(uow)
                              where lots.LotId == id
                              select lots).SingleOrDefault();
            return currentLot;
        }
    }
}