using System;
using System.Web;
using Auction.Domain.DerivativeModels;
using Auction.Domain.Models;
using Auction.Services.Interfaces;
using Auction.UoW.Interfaces;
using Microsoft.AspNet.Identity;

namespace Auction.Services
{
    public class StakeService :BaseService, IStakeService
    {
        public IRepository<Stake> GetRepository()
        {
            return Uow.StakeRepository;
        }

        public Stake Create(int id, double? stakeIncrease, LotStake currentLot)
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
