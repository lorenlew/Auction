using System;
using System.Collections.Generic;
using System.Web;
using Auction.DAL.Models;
using Auction.DAL.UnitOfWork;
using Auction.Domain.DerivativeModels;
using Auction.Domain.Models;
using Auction.Services.Interfaces;
using AutoMapper;
using Microsoft.AspNet.Identity;

namespace Auction.Services
{
    public class StakeService : BaseService, IStakeService
    {
        public StakeService(IUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
        }

        private const double minStakeRate = 1.05;

        public double MinStakeRate
        {
            get { return minStakeRate; }
        }

        public StakeDomainModel Create(int id, double? stakeIncrease, LotStakeDomainModel currentLot)
        {
            if (currentLot == null) throw new ArgumentNullException("currentLot");
            var currentStake = new StakeDomainModel
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

        public void Add(StakeDomainModel entity)
        {
            if (entity == null) throw new ArgumentNullException("entity");
            var stake = Mapper.Map<StakeDomainModel, Stake>(entity);
            Uow.StakeRepository.Add(stake);
        }

        public IEnumerable<StakeDomainModel> Read()
        {
            var stakes = Uow.StakeRepository.Read();
            var stakesDomain = Mapper.Map<IEnumerable<Stake>, IEnumerable<StakeDomainModel>>(stakes);
            return stakesDomain;
        }

        public StakeDomainModel ReadById(int id)
        {
            var stake = Uow.StakeRepository.ReadById(id);
            var stakesDomain = Mapper.Map<Stake, StakeDomainModel>(stake);
            return stakesDomain;
        }

        public void Update(StakeDomainModel entity)
        {
            if (entity == null) throw new ArgumentNullException("entity");
            var stake = Mapper.Map<StakeDomainModel, Stake>(entity);
            Uow.StakeRepository.Update(stake);
        }

        public void Delete(StakeDomainModel entity)
        {
            if (entity == null) throw new ArgumentNullException("entity");
            var stake = Mapper.Map<StakeDomainModel, Stake>(entity);
            Uow.StakeRepository.Delete(stake);
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
