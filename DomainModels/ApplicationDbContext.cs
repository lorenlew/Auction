using System.Data.Entity;
using Auction.DAL.DomainModels;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Auction.DAL
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
    }
}