using Auction.Domain.Models;
using Auction.Web.ViewModels;
using AutoMapper;

namespace Auction.Web.App_Start
{
    public class MapperConfig
    {
        public static void RegisterMaps()
        {
            Mapper.CreateMap<LotViewModel, Lot>();
            Mapper.CreateMap<Lot, LotViewModel>();
            Mapper.CreateMap<StakeViewModel, Stake>();
            Mapper.CreateMap<Stake, StakeViewModel>();
            Mapper.CreateMap<ApplicationUser, ApplicationUserViewModel>();
            Mapper.CreateMap<ApplicationUserViewModel, ApplicationUser>();
        }
    }
}