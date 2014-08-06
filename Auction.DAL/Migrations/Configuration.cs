using System.Data.Entity.Migrations;
using Auction.DAL.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Auction.DAL.Migrations
{
    public sealed class Configuration : DbMigrationsConfiguration<ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            ContextKey = "Auction.DAL.ApplicationDbContext";
        }

        protected override void Seed(ApplicationDbContext context)
        {
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));

            const string administratorRoleName = "Administrator";
            const string moderatorRoleName = "Moderator";
            const string administratorName = "admin@admin.net";
            const string administratorPassword = "1q2w3eQ`";

            if (!roleManager.RoleExists(administratorRoleName))
            {
                var adminRole = roleManager.Create(new IdentityRole(administratorRoleName));
            }
            if (!roleManager.RoleExists(moderatorRoleName))
                roleManager.Create(new IdentityRole(moderatorRoleName));


            if (userManager.FindByName(administratorName) != null) return;
            var user = new ApplicationUser
            {
                UserName = administratorName,
                Email = administratorName, 
                EmailConfirmed = true, 
            };
            userManager.Create(user, administratorPassword);

            userManager.AddToRole(user.Id, administratorRoleName);
        }
    }
}
