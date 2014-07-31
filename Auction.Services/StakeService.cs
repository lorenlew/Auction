using Auction.Repositories.Interfaces;
using Auction.Services.Interfaces;

namespace Auction.Services
{
    public class StakeService :BaseService, IStakeService
    {
        public IRepository<Domain.Models.Stake> Get()
        {
            return Uow.StakeRepository;
        }
    }
}
