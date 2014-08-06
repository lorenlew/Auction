using Auction.DAL.Models;
using Auction.Domain.Models;
using AutoMapper;

namespace Auction.Services.Config
{
    public static class MapperConfig
    {
        public static void RegisterMaps()
        {
            Mapper.CreateMap<Lot, LotDomainModel>();
            Mapper.CreateMap<LotDomainModel, Lot>();
            Mapper.CreateMap<Stake, StakeDomainModel>();
            Mapper.CreateMap<StakeDomainModel, Stake>();
            Mapper.CreateMap<ApplicationUser, ApplicationUserDomainModel>();
            Mapper.CreateMap<ApplicationUserDomainModel, ApplicationUser>();
        }
    }
}