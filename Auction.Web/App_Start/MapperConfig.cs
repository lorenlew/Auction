
using Auction.Domain.DerivativeModels;
using Auction.Domain.Models;
using Auction.Web.ViewModels;
using AutoMapper;

namespace Auction.Web
{
    public static class MapperConfig
    {
        public static void RegisterMaps()
        {
            Mapper.CreateMap<LotViewModel, LotDomainModel>();
            Mapper.CreateMap<LotDomainModel, LotViewModel>();
            Mapper.CreateMap<StakeViewModel, StakeDomainModel>();
            Mapper.CreateMap<StakeDomainModel, StakeViewModel>();
            Mapper.CreateMap<ApplicationUserDomainModel, ApplicationUserViewModel>();
            Mapper.CreateMap<ApplicationUserViewModel, ApplicationUserDomainModel>();
            Mapper.CreateMap<LotStakeDomainModel, LotStakeViewModel>();
        }
    }
}